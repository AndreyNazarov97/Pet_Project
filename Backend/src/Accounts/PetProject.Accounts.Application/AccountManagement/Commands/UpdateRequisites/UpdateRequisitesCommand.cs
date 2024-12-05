using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;

public record UpdateRequisitesCommand: IRequest<Result<long, ErrorList>>
{
    public required long UserId { get; init; } 
    public required IEnumerable<RequisiteDto> Requisites { get; init; } 
}