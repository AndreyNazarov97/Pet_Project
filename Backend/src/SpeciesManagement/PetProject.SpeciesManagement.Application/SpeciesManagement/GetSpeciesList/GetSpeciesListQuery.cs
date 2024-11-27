using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.GetSpeciesList;

public record GetSpeciesListQuery  : IRequest<Result<SpeciesDto[], ErrorList>>
{
    public int Offset { get; init; }
    public int Limit { get; init; }
}