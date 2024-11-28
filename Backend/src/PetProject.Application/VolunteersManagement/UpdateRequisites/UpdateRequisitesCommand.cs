using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.UpdateRequisites;

public record UpdateRequisitesCommand: IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}