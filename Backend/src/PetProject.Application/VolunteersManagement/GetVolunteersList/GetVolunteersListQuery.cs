using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetVolunteersList;

public record GetVolunteersListQuery : IRequest<Result<VolunteerDto[], ErrorList>>
{
    public int Offset { get; init; } 
    public int Limit { get; init; } 
}