using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public class CreateSpeciesHandler : IRequestHandler<CreateSpeciesCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _repository;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(ISpeciesRepository repository,
        ILogger<CreateSpeciesHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesName = SpeciesName.Create(command.Name);

        var existedSpecies = await _repository.GetByName(speciesName.Value, cancellationToken);
        
        if (existedSpecies.IsSuccess)
            return Errors.Model.AlreadyExist("Species").ToErrorList();
        
        var speciesId = SpeciesId.NewId();
        var species = new Species(speciesId, speciesName.Value, []);
        
        var result = await _repository.Add(species, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();
        
        _logger.Log(LogLevel.Information, "Species {speciesName} was created", speciesName);
        
        return result.Value.Id;
    }
}