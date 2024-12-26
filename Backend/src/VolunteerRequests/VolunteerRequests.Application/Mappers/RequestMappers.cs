using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using VolunteerRequests.Contracts.Requests;

namespace VolunteerRequests.Application.Mappers;

public static class RequestMappers
{
    public static CreateVolunteerRequestCommand ToCommand(this CreateVolunteerRequestRequest request)
    {
        return new CreateVolunteerRequestCommand
        {
            UserId = request.UserId,
            FullName = request.FullNameDto,
            Description = request.VolunteerDescription,
            AgeExperience = request.WorkExperience,
            PhoneNumber = request.PhoneNumber,
            SocialNetworksDto = request.SocialNetworksDto
        };
    }
}