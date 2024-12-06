using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateSpecies;

public class CreateSpeciesHandler : IRequestHandler<CreateSpeciesCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _repository;
    private readonly IReadRepository _readRepository;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        ISpeciesRepository repository,
        IReadRepository readRepository,
        ILogger<CreateSpeciesHandler> logger)
    {
        _repository = repository;
        _readRepository = readRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = command.Name
        };
        var species = (await _readRepository.QuerySpecies(speciesQuery, cancellationToken)).SingleOrDefault();
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