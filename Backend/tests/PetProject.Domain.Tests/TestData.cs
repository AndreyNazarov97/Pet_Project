using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Domain.Tests;

public static class TestData
{
    public static Volunteer Volunteer => new(
        VolunteerId.NewId(),
        FullName.Create("Ivan", "Ivanov").Value,
        Description.Create("Test description").Value,
        Experience.Create(3).Value,
        PhoneNumber.Create("78005553535").Value,
        new SocialLinksList([]),
        new RequisitesList([])
    );

    public static Pet Pet => new(
        PetId.NewId(),
        PetName.Create("Test pet").Value,
        Description.Create("Test description").Value,
        Description.Create("Test Health info").Value,
        new AnimalType(
            SpeciesName.Create("Test species").Value, 
            BreedName.Create("Test breed").Value),
        Address.Create(
            "Test address",
            "Test city", 
            "Test street", 
            "Test house", 
            "Test flat")
            .Value,
        PetPhysicalAttributes.Create(10, 10).Value,
        Volunteer.PhoneNumber,
        new DateOnly(2023, 1, 1),
        true,
        true,
        HelpStatus.NeedsHelp,
        new RequisitesList([]),
        new PetPhotosList([])
    );
}