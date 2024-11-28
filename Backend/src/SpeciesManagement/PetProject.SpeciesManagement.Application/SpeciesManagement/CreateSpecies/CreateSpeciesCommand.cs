using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.CreateSpecies;

public record CreateSpeciesCommand : IRequest<Result<Guid, ErrorList>>
{
    public required string Name { get; init; }
}