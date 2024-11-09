using PetProject.Application.Dto;

namespace PetProject.Application.VolunteersManagement.UpdateSocialLinks;

public record UpdateSocialLinksRequest(
    Guid Id, 
    UpdateSocialLinksDto Dto);