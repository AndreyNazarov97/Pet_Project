using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetPetById;

public record GetPetByIdQuery : IRequest<Result<PetDto, ErrorList>>
{
    public required Guid PetId { get; init; }
}