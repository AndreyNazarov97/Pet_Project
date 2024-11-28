using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.GetBreedsList;

public record GetBreedsListQuery : IRequest<Result<BreedDto[], ErrorList>>
{
    public required string SpeciesName { get; init; }
    public int Offset { get; init; }
    public int Limit { get; init; }
}