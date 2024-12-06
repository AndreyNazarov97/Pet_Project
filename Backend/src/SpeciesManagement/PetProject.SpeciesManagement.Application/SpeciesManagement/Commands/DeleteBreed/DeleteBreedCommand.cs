using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.DeleteBreed;

public record DeleteBreedCommand : IRequest<UnitResult<ErrorList>>
{
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
}