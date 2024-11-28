using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;

namespace PetProject.Core.Database.Repository;

public interface IReadRepository
{
    Task<SpeciesDto[]> QuerySpecies(SpeciesQueryModel query, CancellationToken cancellationToken = default);
    
    Task<VolunteerDto[]> QueryVolunteers(VolunteerQueryModel query, CancellationToken cancellationToken = default);
    
    Task<PetDto[]> QueryPets(PetQueryModel query, CancellationToken cancellationToken = default);
}