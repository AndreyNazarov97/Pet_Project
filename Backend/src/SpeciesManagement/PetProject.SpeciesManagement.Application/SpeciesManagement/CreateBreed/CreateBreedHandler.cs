﻿using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Application.Extensions;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Domain.Entities;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.CreateBreed;

public class CreateBreedHandler : IRequestHandler<CreateBreedCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBreedHandler> _logger;

    public CreateBreedHandler(ISpeciesRepository speciesRepository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateBreedHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = command.SpeciesName
        };
        var species = (await _speciesRepository.Query(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound().ToErrorList();

        if(species.Breeds.Any(x => x.Name == command.BreedName))
            return Errors.General.AlreadyExist("Breed").ToErrorList();
        
        var breedId = BreedId.NewId();
        var breedName = BreedName.Create(command.BreedName).Value;
        var breed = new Breed(breedId, breedName);
        
        var speciesEntity = species.ToEntity();
        speciesEntity.AddBreeds([breed]);
        
        _logger.Log(LogLevel.Information, "Breed {breedName} was created", breedName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return breed.Id.Id;
    }
}