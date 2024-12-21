
namespace PetProject.Core.Dtos;

public record SpeciesDto()
{
    public required Guid SpeciesId { get; init; }
    public required string SpeciesName { get; init; }
    public List<BreedDto> Breeds { get; set; } = [];
};