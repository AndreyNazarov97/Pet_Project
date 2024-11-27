using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateVolunteer;

namespace PetProject.VolunteerManagement.Presentation.Requests;

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