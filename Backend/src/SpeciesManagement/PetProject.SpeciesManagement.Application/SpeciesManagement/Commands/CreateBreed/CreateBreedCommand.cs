using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.CreateBreed;

public record CreateBreedCommand : IRequest<Result<Guid, ErrorList>>
{
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
}