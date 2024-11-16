using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public class CreateBreedHandler : IRequestHandler<CreateBreedCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateBreedHandler> _logger;

    public CreateBreedHandler(ISpeciesRepository speciesRepository, 
        ILogger<CreateBreedHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(command.SpeciesName).Value;
        var existedSpecies = await _speciesRepository.GetByName(speciesName, cancellationToken);

        if (existedSpecies.IsFailure)
            return Errors.General.NotFound().ToErrorList();

        if(existedSpecies.Value.Breeds.Any(x => x.BreedName.Value == command.BreedName))
            return Errors.Model.AlreadyExist("Breed").ToErrorList();
        
        var breedId = BreedId.NewId();
        var breedName = BreedName.Create(command.BreedName).Value;
        var breed = new Breed(breedId, breedName);
        
        existedSpecies.Value.AddBreeds([breed]);
        
        await _speciesRepository.Save(existedSpecies.Value, cancellationToken);
        
        return breed.Id.Id;
    }
}