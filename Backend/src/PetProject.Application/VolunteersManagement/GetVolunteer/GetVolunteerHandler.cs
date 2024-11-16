using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.VolunteersManagement.GetVolunteer;

public class GetVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerDto, Error>> Handle(GetVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error;
        }

        var volunteerDto = new VolunteerDto
        {
            FullName = string.Join(" ", result.Value.FullName.Name, result.Value.FullName.Surname,
                result.Value.FullName.Patronymic),
            GeneralDescription = result.Value.GeneralDescription.Value,
            PhoneNumber = result.Value.PhoneNumber.Value,
            AgeExperience = result.Value.Experience.Years
        };

        return volunteerDto;
    }
}