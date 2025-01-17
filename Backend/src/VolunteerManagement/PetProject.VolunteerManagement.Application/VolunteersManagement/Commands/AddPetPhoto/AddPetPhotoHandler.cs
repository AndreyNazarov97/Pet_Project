﻿using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Domain.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;

public class AddPetPhotoHandler : IRequestHandler<AddPetPhotoCommand, Result<FilePath[], ErrorList>>
{
    private const string BucketName = "pet-project";

    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetPhotoHandler> _logger;

    public AddPetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Constants.Context.VolunteerManagement)]
        IUnitOfWork unitOfWork,
        ILogger<AddPetPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FilePath[], ErrorList>> Handle(AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;
            var petId = PetId.Create(command.PetId);
            var petResult = volunteer.GetById(petId);
            if (petResult is null)
                return Errors.General.NotFound(command.PetId).ToErrorList();
            
            var filePaths = command.Keys.Select(key => FilePath.Create(key).Value).ToArray();
            var petPhotos = filePaths
                .Select(path => new PetPhoto(path))
                .ToList();

            var petPhotosResult = volunteer.AddPetPhoto(petId, petPhotos);
            if (petPhotosResult.IsFailure)
                return petPhotosResult.Error.ToErrorList();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();
            return filePaths;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding pet photo");
            transaction.Rollback();
            return Error
                .Failure("could.not.add.pet.photo", "Could not add pet photo")
                .ToErrorList();
        }
    }
}