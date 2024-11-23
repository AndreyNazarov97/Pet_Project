using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Models;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.DeleteSpecies;

public class DeleteSpeciesHandler : IRequestHandler<DeleteSpeciesCommand, UnitResult<ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IVolunteersRepository _volunteersRepository;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IVolunteersRepository volunteersRepository)
    {
        _speciesRepository = speciesRepository;
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(DeleteSpeciesCommand request, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = request.SpeciesName
        };
        var species = (await _speciesRepository.Query(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound().ToErrorList();

        var volunteerQuery = new VolunteerQueryModel()
        {
            SpeciesIds = [species.Id]
        };
        var volunteers = await _volunteersRepository.Query(volunteerQuery, cancellationToken);
        if (volunteers.Length > 0)
            return Error.Conflict("species.is.used.by.volunteers", "Species is used by volunteers").ToErrorList();

        await _speciesRepository.Delete(SpeciesId.Create(species.Id), cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}