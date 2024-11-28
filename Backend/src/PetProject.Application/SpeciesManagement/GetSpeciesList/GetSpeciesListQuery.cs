using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.GetSpeciesList;

public record GetSpeciesListQuery  : IRequest<Result<SpeciesDto[], ErrorList>>
{
    public int Offset { get; init; }
    public int Limit { get; init; }
}