using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.DeleteVolunteer;

public record DeleteVolunteerCommand(Guid Id) : IRequest<Result<Guid, Error>>
{
    
}