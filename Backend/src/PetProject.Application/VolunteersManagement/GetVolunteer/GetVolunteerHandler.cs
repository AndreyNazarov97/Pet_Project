using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Application.Extensions;
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
        var volunteerId = VolunteerId.Create(query.VolunteerId);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        var volunteer = result.Value;
        var volunteerDto = new VolunteerDto
        {
            FullName = volunteer.FullName.ToDto(),
            GeneralDescription = volunteer.GeneralDescription.Value,
            PhoneNumber = volunteer.PhoneNumber.Value,
            AgeExperience = volunteer.Experience.Years,
            Requisites = volunteer.Requisites.Select(r => r.ToDto()).ToArray(),
            SocialLinks = volunteer.SocialLinks.Select(s => s.ToDto()).ToArray()
        };

        return volunteerDto;
    }
}