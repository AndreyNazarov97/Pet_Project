using CSharpFunctionalExtensions;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.SpeciesManagement.Application.Repository;

public interface ISpeciesRepository
{
    Task<Result<SpeciesId, Error>> Add(Species species, CancellationToken cancellationToken = default);
    
    Task<Result<SpeciesId, Error>> Delete(SpeciesId id, CancellationToken cancellationToken = default);
}