using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;

namespace PetProject.Core.Database.Repository;

public interface IReadRepository
{
    Task<SpeciesDto[]> Query(SpeciesQueryModel query, CancellationToken cancellationToken = default);
    
    Task<VolunteerDto[]> Query(VolunteerQueryModel query, CancellationToken cancellationToken = default);
    
    Task<PetDto[]> Query(PetQueryModel query, CancellationToken cancellationToken = default);
}