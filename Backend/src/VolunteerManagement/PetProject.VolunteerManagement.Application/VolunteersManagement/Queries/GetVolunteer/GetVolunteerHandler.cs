using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Extensions;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteer;

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
        var volunteerId = VolunteerId.Create(query.VolunteerId);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        var volunteer = result.Value;
        var volunteerDto = volunteer.ToDto();

        return volunteerDto;
    }
}