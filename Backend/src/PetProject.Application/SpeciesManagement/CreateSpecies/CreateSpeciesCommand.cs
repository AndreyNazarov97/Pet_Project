using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public record CreateSpeciesCommand : IRequest<Result<Guid, Error>>
{
    public required string Name { get; init; }
}