using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public class CreateBreedHandler(ISpeciesRepository speciesRepository, ILogger<CreateBreedHandler> logger)
{
    public async Task<Result<BreedId, Error>> Handle(CreateBreedRequest request, CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(request.SpeciesName).Value;
        var existedSpecies = await speciesRepository.GetByName(speciesName, cancellationToken);

        if (existedSpecies.IsFailure)
            return Errors.General.NotFound();

        if(existedSpecies.Value.Breeds.Any(x => x.BreedName.Value == request.BreedName))
            return Errors.Model.AlreadyExist("Breed");
        
        var breedId = BreedId.NewId();
        var breedName = BreedName.Create(request.BreedName).Value;
        var breed = new Breed(breedId, breedName);
        
        existedSpecies.Value.AddBreeds([breed]);
        
        await speciesRepository.Save(existedSpecies.Value, cancellationToken);
        
        return breed.Id;
    }
}