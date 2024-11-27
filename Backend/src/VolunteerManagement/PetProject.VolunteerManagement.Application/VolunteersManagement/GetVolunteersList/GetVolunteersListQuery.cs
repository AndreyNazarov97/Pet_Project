using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteersList;

public record GetVolunteersListQuery : IRequest<Result<VolunteerDto[], ErrorList>>
{
    public int Offset { get; init; } 
    public int Limit { get; init; } 
}