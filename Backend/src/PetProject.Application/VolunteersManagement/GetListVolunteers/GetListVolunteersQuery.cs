using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetListVolunteers;

public record GetListVolunteersQuery : IRequest<Result<VolunteerDto[], ErrorList>>
{
    public int Offset { get; init; } = 0;
    public int Limit { get; init; } = 10;
}