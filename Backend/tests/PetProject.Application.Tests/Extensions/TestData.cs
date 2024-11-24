using Bogus;
using PetProject.Application.Dto;
using PetProject.Application.Tests.Creators;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;
using Random = PetProject.Application.Tests.Creators.Random;

namespace PetProject.Application.Tests.Extensions;

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
            SpeciesName.Create("Test species").Value,
            BreedName.Create("Test breed").Value),
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
        new PetPhotosList([])
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