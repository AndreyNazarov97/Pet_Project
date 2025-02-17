using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Communication.Contracts.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.DeletePetPhoto;

public class DeletePetPhotoHandler : IRequestHandler<DeletePetPhotoCommand, UnitResult<ErrorList>>
{
    private const string BucketName = "pet-project";
    
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly FileHttpClient _fileHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetPhotoHandler> _logger;

    public DeletePetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        FileHttpClient fileHttpClient,
        [FromKeyedServices(Constants.Context.VolunteerManagement)]IUnitOfWork unitOfWork,
        ILogger<DeletePetPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileHttpClient = fileHttpClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeletePetPhotoCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var volunteer = volunteerResult.Value;
        var petId = PetId.Create(command.PetId);

        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = volunteer.DeletePetPhoto(petId, command.FileId);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            var deleteRequest = new DeleteFilesRequest([command.FileId]);
            var deleteResult = await _fileHttpClient.DeletePresignedUrlAsync(deleteRequest, cancellationToken);
            if (deleteResult.IsFailure)
                return Errors.Minio.CouldNotDeleteFile().ToErrorList();
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting pet photo");
            transaction.Rollback();
            return Error
                .Failure("could.not.delete.pet.photo", "Could not delete pet photo")
                .ToErrorList();
        }
        
    }
}