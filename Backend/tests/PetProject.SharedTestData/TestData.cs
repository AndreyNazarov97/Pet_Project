using Bogus;
using PetProject.Application.Dto;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;
using PetProject.SharedTestData.Creators;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.SharedTestData;

public class TestData
{
    private static readonly Faker Faker = new();

    public static Volunteer Volunteer => new(
        VolunteerId.NewId(),
        FullName.Create(Faker.Name.FirstName(), Faker.Name.LastName()).Value,
        Description.Create(Random.LoremParagraph).Value,
        Experience.Create(Random.Experience).Value,
        PhoneNumber.Create(Random.PhoneNumber).Value,
        new SocialLinksList([]),
        new RequisitesList([])
    );

    public static Pet Pet => new(
        PetId.NewId(),
        PetName.Create(Faker.Name.FirstName()).Value,
        Description.Create(Random.LoremParagraph).Value,
        Description.Create(Random.LoremParagraph).Value,
        new AnimalType(
            SpeciesName.Create("Dog").Value,
            BreedName.Create("Bulldog").Value),
        Address.Create(
                Random.Address.Country(),
                Random.Address.City(),
                Random.Address.StreetName(),
                Random.Address.BuildingNumber(),
                new Faker().Random.Number(1, 100).ToString()
                )
            .Value,
        PetPhysicalAttributes.Create(10, 10).Value,
        PhoneNumber.Create(Random.PhoneNumber).Value,
        new DateOnly(2023, 1, 1),
        true,
        true,
        HelpStatus.NeedsHelp,
        new RequisitesList([]),
        []
    );

    public static VolunteerDto VolunteerDto => new()
    {
        FullName = VolunteerCreator.CreateFullNameDto(),
        GeneralDescription = Random.LoremParagraph,
        AgeExperience = Random.Experience,
        PhoneNumber = Random.PhoneNumber,
        Requisites = [],
        SocialLinks = []
    };
    
    public static PetDto PetDto => new()
    {
        PetName = Random.Name,
        GeneralDescription = Random.LoremParagraph,
        HealthInformation = Random.LoremParagraph,
        SpeciesName = "Dog",
        BreedName = "Bulldog",
        Address = VolunteerCreator.CreateAddressDto(),
        Weight = Random.Double,
        Height = Random.Double,
        PhoneNumber = Random.PhoneNumber,
        BirthDate = new DateTime(Random.DateOnly, TimeOnly.MinValue, DateTimeKind.Utc),
        IsCastrated = Random.Bool,
        IsVaccinated = Random.Bool,
        HelpStatus = Random.HelpStatus,
    };

    public static SpeciesDto SpeciesDto => new(
        Guid.NewGuid(),
        "Dog",
        new List<BreedDto>()
        {
            new(
                Guid.NewGuid(),
                "Labrador"),
            new(
                Guid.NewGuid(),
                "Golden Retriever"),
            new(
                Guid.NewGuid(),
                "Bulldog")
        }
    );
    
    public static FileDto FileDto => new(
        Random.Name + ".jpg", 
        "image/jpeg", 
        new MemoryStream());
}