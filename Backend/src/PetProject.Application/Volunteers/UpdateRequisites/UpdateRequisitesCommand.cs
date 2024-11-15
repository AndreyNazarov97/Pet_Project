using PetProject.Application.Dto;

namespace PetProject.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand()
{
    public required Guid Id { get; init; } 
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}