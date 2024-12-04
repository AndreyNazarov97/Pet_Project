using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.CreateVolunteer;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber)
{
    public CreateVolunteerCommand ToCommand() => new()
    {
        FullName = FullName,
        Description = Description,
        AgeExperience = AgeExperience,
        PhoneNumber = PhoneNumber
    };
}