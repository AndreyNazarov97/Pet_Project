namespace PetProject.Core.Dtos;

public record PetDto
{
    public string? PetName { get; init; }
    public string? GeneralDescription { get; init; }
    public string? HealthInformation { get; init; }
    public string? SpeciesName { get; init; }
    public string? BreedName { get; init; }
    public AddressDto? Address { get; init; }
    public double? Weight { get; init; }
    public double? Height { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? BirthDate { get; init; }
    public bool? IsCastrated { get; init; }
    public bool? IsVaccinated { get; init; }
    public string? HelpStatus { get; init; }
   }

