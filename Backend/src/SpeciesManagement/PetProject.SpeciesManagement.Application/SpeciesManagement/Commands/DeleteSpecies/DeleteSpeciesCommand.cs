using CSharpFunctionalExtensions;
using MediatR;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.DeleteSpecies;

public record DeleteSpeciesCommand : IRequest<UnitResult<ErrorList>>
{
    public required string SpeciesName { get; init; } 
}