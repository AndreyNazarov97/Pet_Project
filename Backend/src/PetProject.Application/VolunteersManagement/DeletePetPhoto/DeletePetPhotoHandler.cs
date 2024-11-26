using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.DeletePetPhoto;

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
        var pet = volunteer.GetById(PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var filePath = FilePath.Create(command.FilePath).Value;
            var result = pet.DeletePhoto(filePath);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            var fileMetaDataDto = new FileMetaDataDto(command.FilePath, BucketName);
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