using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteer;

public record GetVolunteerQuery() : IRequest<Result<VolunteerDto, ErrorList>>
{
    public required Guid VolunteerId { get; init; } 
}