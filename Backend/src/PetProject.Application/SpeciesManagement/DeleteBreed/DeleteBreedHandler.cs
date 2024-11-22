using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Abstractions;
using PetProject.Application.Models;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.DeleteBreed;

public class DeleteBreedHandler : IRequestHandler<DeleteBreedCommand, UnitResult<ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        ISpeciesRepository speciesRepository,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(DeleteBreedCommand request, CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(request.SpeciesName).Value;
        var breedName = BreedName.Create(request.BreedName).Value;
        var species = await _speciesRepository.GetByName(speciesName, cancellationToken);
        if(species.IsFailure)
            return Errors.General.NotFound().ToErrorList();
        
        var existedBreed = species.Value.Breeds.FirstOrDefault(x => x.BreedName == breedName);
        if (existedBreed == null)
            return Errors.General.NotFound().ToErrorList();

        var volunteerQuery = new VolunteerQueryModel()
        {
            BreedIds = [existedBreed.Id]
        };
        var volunteers = (await _volunteersRepository.Query(volunteerQuery, cancellationToken)).Value;
        if (volunteers.Length > 0)
            return Error.Conflict("species.is.used.by.volunteers", "Species is used by volunteers").ToErrorList();

        species.Value.RemoveBreed(existedBreed);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ErrorList>();
    }
}