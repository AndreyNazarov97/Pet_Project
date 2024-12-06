using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Application.Repository;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.DeleteSpecies;

public class DeleteSpeciesHandler : IRequestHandler<DeleteSpeciesCommand, UnitResult<ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadRepository _readRepository;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IReadRepository readRepository)
    {
        _speciesRepository = speciesRepository;
        _readRepository = readRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(DeleteSpeciesCommand request, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = request.SpeciesName
        };
        var species = (await _readRepository.QuerySpecies(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound().ToErrorList();

        var volunteerQuery = new VolunteerQueryModel()
        {
            SpeciesNames = [request.SpeciesName]
        };
        var volunteers = await _readRepository.QueryVolunteers(volunteerQuery, cancellationToken);
        if (volunteers.Length > 0)
            return Error.Conflict("species.is.used.by.volunteers", "Species is used by volunteers").ToErrorList();

        await _speciesRepository.Delete(SpeciesId.Create(species.Id), cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}