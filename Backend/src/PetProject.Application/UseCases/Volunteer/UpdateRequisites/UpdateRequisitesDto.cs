using PetProject.Domain.Dto;

namespace PetProject.Application.UseCases.Volunteer.UpdateRequisites;

public record UpdateRequisitesDto(
    List<RequisiteDto> Requisites);