using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public record UpdateSocialLinksCommand(
    Guid Id, 
    UpdateSocialLinksDto Dto);