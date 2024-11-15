using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public record CreateBreedCommand : IRequest<Result<Guid, Error>>
{
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
}