using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.AddPetPhoto;

public class AddPetPhotoHandler : IRequestHandler<AddPetPhotoCommand, Result<string, Error>>
{
    private const string BucketName = "pet-project";

    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetPhotoHandler> _logger;

    public AddPetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        ILogger<AddPetPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var filesData = command.Photos
                .Select(photo => new FileDataDto(photo.Content, photo.FileName, BucketName))
                .ToList();

            var uploadResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
            if (uploadResult.IsFailure) 
                return uploadResult.Error;
        
            var petPhotos = uploadResult.Value
                .Select(p => new PetPhoto(p))
                .ToList();
        
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;
        
            var volunteer = volunteerResult.Value;

            var petId = PetId.Create(command.PetId);
            var pet = volunteer.GetById(petId);
            if (pet is null)
                return Errors.General.NotFound(command.PetId);
        
            pet.AddPhotos(petPhotos);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();
            return "Success";

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding pet photo");
            transaction.Rollback();
            return Error.Failure("could.not.add.pet.photo", "Could not add pet photo");
        }
        
    }
}