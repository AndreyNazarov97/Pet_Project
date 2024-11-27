using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.GetBreedsList;

public record GetBreedsListQuery : IRequest<Result<BreedDto[], ErrorList>>
{
    public required string SpeciesName { get; init; }
    public int Offset { get; init; }
    public int Limit { get; init; }
}