using PetProject.Core.Dtos;

namespace VolunteerRequests.Contracts.Requests;

public record UpdateRequest(
    Guid VolunteerRequestId,
    FullNameDto FullNameDto,
    string Email,
    string PhoneNumber,
    int WorkExperience,
    string VolunteerDescription,
    IEnumerable<SocialNetworkDto> SocialNetworkDtos);