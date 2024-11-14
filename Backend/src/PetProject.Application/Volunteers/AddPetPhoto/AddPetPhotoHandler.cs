﻿using CSharpFunctionalExtensions;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoHandler
{
    private const string BucketName = "pet-photo";

    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;

    public AddPetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        List<PetPhoto> petPhotos = [];
        foreach (var photo in command.Photos)
        {
            var extension = Path.GetExtension(photo.FileName);

            var filePath = FilePath.Create(Guid.NewGuid().ToString(), extension);
            if (filePath.IsFailure)
                return filePath.Error;

            var fileData = new FileDataDto(photo.Content, filePath.Value.Path, BucketName);

            var uploadResult = await _fileProvider.UploadFile(fileData, cancellationToken);
            if (uploadResult.IsFailure) 
                return uploadResult.Error;
            
            petPhotos.Add(new PetPhoto(filePath.Value));
        }
        
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
        await _volunteersRepository.Save(volunteer, cancellationToken);

        return "Success";
    }
}