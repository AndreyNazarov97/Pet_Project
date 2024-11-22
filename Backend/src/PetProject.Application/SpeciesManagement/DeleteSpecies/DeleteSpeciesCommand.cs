using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.DeleteSpecies;

public record DeleteSpeciesCommand : IRequest<UnitResult<ErrorList>>
{
    public required string SpeciesName { get; init; } 
}