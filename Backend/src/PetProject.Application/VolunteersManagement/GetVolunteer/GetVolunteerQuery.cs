using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetVolunteer;

public record GetVolunteerQuery() : IRequest<Result<VolunteerDto, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
}