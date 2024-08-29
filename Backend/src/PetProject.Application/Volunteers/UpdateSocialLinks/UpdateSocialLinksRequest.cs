using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public record UpdateSocialLinksRequest(
    Guid Id, 
    UpdateSocialLinksDto Dto);