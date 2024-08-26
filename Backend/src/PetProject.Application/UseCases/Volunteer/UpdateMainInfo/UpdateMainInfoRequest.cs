using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.Volunteer.UpdateMainInfo;

public record UpdateMainInfoRequest(
    Guid VolunteerId,
    UpdateMainInfoDto Dto);