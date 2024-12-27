using PetProject.Core.Dtos;

namespace VolunteerRequests.Contracts.Requests;

public record UpdateVolunteerRequestRequest(
    long UserId,
    FullNameDto FullNameDto,
    string PhoneNumber,
    int WorkExperience,
    string VolunteerDescription,
    IEnumerable<SocialNetworkDto> SocialNetworksDto);