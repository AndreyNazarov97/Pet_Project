using PetProject.Application.Dto;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;

namespace PetProject.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber)
{
    public UpdateVolunteerCommand ToCommand(Guid idVolunteer) => new()
    {
        VolunteerId = idVolunteer,
        FullName = FullName,
        Description = Description,
        AgeExperience = AgeExperience,
        PhoneNumber = PhoneNumber
    };
}