using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateRequisites;

public record UpdateRequisitesCommand: IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}