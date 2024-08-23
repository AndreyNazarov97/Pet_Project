using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public class UpdateRequisitesRequest
{
    public List<RequisiteDto> Requisites { get; set; }
}