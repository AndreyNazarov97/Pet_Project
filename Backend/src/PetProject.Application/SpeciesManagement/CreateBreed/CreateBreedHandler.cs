using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public class CreateBreedHandler(ISpeciesRepository speciesRepository, ILogger<CreateBreedHandler> logger)
{
    public async Task<Result<BreedId, Error>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(command.SpeciesName).Value;
        var existedSpecies = await speciesRepository.GetByName(speciesName, cancellationToken);

        if (existedSpecies.IsFailure)
            return Errors.General.NotFound();

        if(existedSpecies.Value.Breeds.Any(x => x.BreedName.Value == command.BreedName))
            return Errors.Model.AlreadyExist("Breed");
        
        var breedId = BreedId.NewId();
        var breedName = BreedName.Create(command.BreedName).Value;
        var breed = new Breed(breedId, breedName);
        
        existedSpecies.Value.AddBreeds([breed]);
        
        await speciesRepository.Save(existedSpecies.Value, cancellationToken);
        
        return breed.Id;
    }
}