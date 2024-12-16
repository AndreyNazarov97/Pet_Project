using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateSpecies;

public record CreateSpeciesCommand : IRequest<Result<Guid, ErrorList>>
{
    public required string Name { get; init; }
}