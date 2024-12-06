using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Providers;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.DeletePetPhoto;

public class DeletePetPhotoHandler : IRequestHandler<DeletePetPhotoCommand, UnitResult<ErrorList>>
{
    private const string BucketName = "pet-project";
    
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetPhotoHandler> _logger;

    public DeletePetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        ILogger<DeletePetPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
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
            var filePath = FilePath.Create(command.FilePath).Value;
            var result = volunteer.DeletePetPhoto(petId, filePath);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            var fileMetaDataDto = new FileMetaDataDto
            {
                ObjectName = command.FilePath,
                BucketName = BucketName
            };
            var deleteResult = await _fileProvider.DeleteFile(fileMetaDataDto, cancellationToken);
            if (deleteResult.IsFailure)
                return deleteResult.Error;
            
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