using CSharpFunctionalExtensions;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement;

public interface ISpeciesRepository
{
    Task<Result<List<Species>, Error>> GetAll(CancellationToken cancellationToken = default);
    
    Task<Result<Species, Error>> Get(SpeciesId id, CancellationToken cancellationToken = default);
    
    Task<Result<Species[],Error>> Query(SpeciesQueryModel query, CancellationToken cancellationToken = default);
    
    Task<Result<Species, Error>> GetByName(SpeciesName name, CancellationToken cancellationToken = default);
    
    public Task<Result<SpeciesId, Error>> Save(Species species, CancellationToken cancellationToken = default);
    
    Task<Result<SpeciesId, Error>> Add(Species species, CancellationToken cancellationToken = default);
    
    Task<Result<SpeciesId, Error>> Delete(SpeciesId id, CancellationToken cancellationToken = default);
}