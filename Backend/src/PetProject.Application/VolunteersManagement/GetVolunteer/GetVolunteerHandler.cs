using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Application.Extensions;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.VolunteersManagement.GetVolunteer;

public class GetVolunteerHandler : IRequestHandler<GetVolunteerQuery, Result<VolunteerDto, ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(GetVolunteerQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            VolunteerIds = [query.VolunteerId]
        };

        var volunteer = (await _volunteersRepository.Query(volunteerQuery, cancellationToken)).SingleOrDefault();
        if(volunteer == null) 
            return Errors.General.NotFound().ToErrorList();

        return volunteer;
    }
}