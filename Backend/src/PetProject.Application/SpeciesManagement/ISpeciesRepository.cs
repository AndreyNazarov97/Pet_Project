using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement;

public interface ISpeciesRepository
{
    Task<SpeciesDto[]> Query(SpeciesQueryModel query, CancellationToken cancellationToken = default);
    
    Task<Result<SpeciesId, Error>> Add(Species species, CancellationToken cancellationToken = default);
    
    Task<Result<SpeciesId, Error>> Delete(SpeciesId id, CancellationToken cancellationToken = default);
}