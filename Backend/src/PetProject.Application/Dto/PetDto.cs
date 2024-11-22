using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record PetDto
{
    public required Guid Id { get; init; }
    public required string PetName { get; init; }
    public required string GeneralDescription { get; init; }
    public required string HealthInformation { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required AddressDto Address { get; init; }
    public required double Weight { get; init; }
    public required double Height { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime BirthDate { get; init; }
    public required bool IsCastrated { get; init; }
    public required bool IsVaccinated { get; init; }
    public required HelpStatus HelpStatus { get; init; }
    public RequisiteDto[] Requisites { get; init; } = [];
    public PetPhoto[] Photos { get; init; } = [];

    public Pet ToEntity()
    {
        var pet = new Pet(
            PetId.Create(Id),
            Domain.VolunteerManagement.ValueObjects.PetName.Create(PetName).Value,
            Description.Create(GeneralDescription).Value,
            Description.Create(HealthInformation).Value,
            new AnimalType(
                Domain.SpeciesManagement.ValueObjects.SpeciesName.Create(SpeciesName).Value,
                Domain.SpeciesManagement.ValueObjects.BreedName.Create(BreedName).Value),
            Address.ToEntity(),
            PetPhysicalAttributes.Create(Weight, Height).Value,
            Domain.Shared.ValueObjects.PhoneNumber.Create(PhoneNumber).Value,
            new DateOnly(BirthDate.Year, BirthDate.Month, BirthDate.Day),
            IsCastrated,
            IsVaccinated,
            HelpStatus,
            new RequisitesList(Requisites.Select(r => r.ToEntity())),
            new PetPhotosList(Photos));

        return pet;
    }
}