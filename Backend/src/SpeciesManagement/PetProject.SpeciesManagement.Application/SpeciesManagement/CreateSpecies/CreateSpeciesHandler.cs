using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database.Models;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.CreateSpecies;

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
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = command.Name
        };
        var species = (await _repository.Query(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species != null)
            return Errors.General.AlreadyExist("Species").ToErrorList();
        
        var speciesId = SpeciesId.NewId();
        var speciesEntity = new Species(speciesId, SpeciesName.Create(command.Name).Value, []);
        
        var result = await _repository.Add(speciesEntity, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();
        
        _logger.Log(LogLevel.Information, "Species {speciesName} was created", command.Name);
        
        return result.Value.Id;
    }
}