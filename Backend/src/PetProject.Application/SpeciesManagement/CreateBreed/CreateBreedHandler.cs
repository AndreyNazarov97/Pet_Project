using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

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
        var speciesName = SpeciesName.Create(command.SpeciesName).Value;
        var existedSpecies = await _speciesRepository.GetByName(speciesName, cancellationToken);

        if (existedSpecies.IsFailure)
            return Errors.General.NotFound().ToErrorList();

        if(existedSpecies.Value.Breeds.Any(x => x.BreedName.Value == command.BreedName))
            return Errors.General.AlreadyExist("Breed").ToErrorList();
        
        var breedId = BreedId.NewId();
        var breedName = BreedName.Create(command.BreedName).Value;
        var breed = new Breed(breedId, breedName);
        
        existedSpecies.Value.AddBreeds([breed]);
        
        _logger.Log(LogLevel.Information, "Breed {breedName} was created", breedName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return breed.Id.Id;
    }
}