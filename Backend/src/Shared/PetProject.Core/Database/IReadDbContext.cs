using PetProject.Core.Dtos;

namespace PetProject.Core.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<BreedDto> Breeds { get; }
    IQueryable<SpeciesDto> Species { get; }
}
