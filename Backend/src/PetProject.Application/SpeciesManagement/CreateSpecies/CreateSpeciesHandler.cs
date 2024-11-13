using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public class CreateSpeciesHandler(
    ISpeciesRepository repository,
    ILogger<CreateSpeciesHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(CreateSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesName = SpeciesName.Create(command.Name);

        var existedSpecies = await repository.GetByName(speciesName.Value, cancellationToken);
        
        if (existedSpecies.IsSuccess)
            return Errors.Model.AlreadyExist("Species");
        
        var speciesId = SpeciesId.NewId();
        var species = new Species(speciesId, speciesName.Value, []);
        
        var result = await repository.Add(species, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.Log(LogLevel.Information, "Species {speciesName} was created", speciesName);
        
        return result.Value.Id;
    }
}