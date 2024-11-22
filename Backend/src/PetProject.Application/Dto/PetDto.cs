using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.Application.Dto;

public record PetDto
{
    public required string PetName { get; init; }
    public required string GeneralDescription { get; init; }
    public required string HealthInformation { get; init; }
    public required Guid SpeciesId { get; init; }
    public required Guid BreedId { get; init; }
    public required AddressDto Address { get; init; }
    public required double Weight { get; init; }
    public required double Height { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime BirthDate { get; init; }
    public required bool IsCastrated { get; init; }
    public required bool IsVaccinated { get; init; }
    public required HelpStatus HelpStatus { get; init; }
   }

