using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.CreateSpecies;

public record CreateSpeciesCommand : IRequest<Result<Guid, ErrorList>>
{
    public required string Name { get; init; }
}