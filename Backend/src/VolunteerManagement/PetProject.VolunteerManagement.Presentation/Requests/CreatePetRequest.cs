using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.CreatePet;
using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.VolunteerManagement.Presentation.Requests;

public record CreatePetRequest
{
    public required string Name { get; init; }
    public required string GeneralDescription { get; init; }
    public required string HealthInformation { get; init; }
    public required string Species { get; init; }
    public required string Breed { get; init; }
    public required AddressDto Address { get; init; }
    public required double Weight { get; init; }
    public required double Height { get; init; }
    public required int  BirthDateYear { get; init; }
    public required int  BirthDateMonth { get; init; }
    public required int  BirthDateDay { get; init; }
    public required bool IsCastrated { get; init; }
    public required bool IsVaccinated { get; init; }
    public required HelpStatus HelpStatus { get; init; }

    public CreatePetCommand ToCommand(Guid volunteerId)
    {
        var command = new CreatePetCommand
        {
            VolunteerId = volunteerId,
            Name = Name,
            GeneralDescription = GeneralDescription,
            HealthInformation = HealthInformation,
            SpeciesName = Species,
            BreedName = Breed,
            Address = Address,
            Weight = Weight,
            Height = Height,
            BirthDate = new DateOnly(BirthDateYear, BirthDateMonth, BirthDateDay),
            IsCastrated = IsCastrated,
            IsVaccinated = IsVaccinated,
            HelpStatus = HelpStatus
        };
        return command;
    }
}