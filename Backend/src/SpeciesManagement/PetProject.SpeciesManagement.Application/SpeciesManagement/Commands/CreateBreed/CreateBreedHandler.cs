using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SpeciesManagement.Application.Extensions;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.SpeciesManagement.Domain.Entities;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateBreed;

public class CreateBreedHandler : IRequestHandler<CreateBreedCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBreedHandler> _logger;

    public CreateBreedHandler(ISpeciesRepository speciesRepository, 
        IReadRepository readRepository,
        [FromKeyedServices(Constants.Context.SpeciesManagement)]IUnitOfWork unitOfWork,
        ILogger<CreateBreedHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = command.SpeciesName
        };
        var species = (await _readRepository.QuerySpecies(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound().ToErrorList();

        if(species.Breeds.Any(x => x.BreedName == command.BreedName))
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