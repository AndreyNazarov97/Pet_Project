using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Models;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
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
        var speciesName = SpeciesName.Create(request.SpeciesName).Value;
        var speciesToDeleteResult = await _speciesRepository.GetByName(speciesName, cancellationToken);
        if(speciesToDeleteResult.IsFailure)
            return Errors.General.NotFound().ToErrorList();

        var volunteerQuery = new VolunteerQueryModel()
        {
            SpeciesIds = [speciesToDeleteResult.Value.Id]
        };
        var volunteers = (await _volunteersRepository.Query(volunteerQuery, cancellationToken)).Value;
        if (volunteers.Length > 0)
            return Error.Conflict("species.is.used.by.volunteers", "Species is used by volunteers").ToErrorList();

        await _speciesRepository.Delete(speciesToDeleteResult.Value.Id, cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}