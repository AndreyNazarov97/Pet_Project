using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetPetById;

public record GetPetByIdQuery : IRequest<Result<PetDto, ErrorList>>
{
    public required Guid PetId { get; init; }
}