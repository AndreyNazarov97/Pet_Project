using Bogus;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.SharedTestData.Creators;
using PetProject.SpeciesManagement.Domain.Aggregate;
using PetProject.SpeciesManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;
using Address = PetProject.SharedKernel.Shared.ValueObjects.Address;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.SharedTestData;

public class TestData
{
    private static readonly Faker Faker = new();

    public static Volunteer Volunteer => new(
        VolunteerId.NewId(),
        FullName.Create(Faker.Name.FirstName(), Faker.Name.LastName()).Value,
        Description.Create(Random.Words).Value,
        Experience.Create(Random.Experience).Value,
        PhoneNumber.Create(Random.PhoneNumber).Value
    );

    public static Pet Pet => new(
        PetId.NewId(),
        PetName.Create(Random.Name).Value,
        Description.Create(Random.Words).Value,
        Description.Create(Random.Words).Value,
        new AnimalType(
            SpeciesName.Create(Random.Name).Value,
            BreedName.Create(Random.Name).Value),
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
        Random.DateTimeOffset,
        true,
        true,
        HelpStatus.NeedsHelp,
        []
    );

    public static Species Species => new(
        SpeciesId.NewId(),
        SpeciesName.Create("Dog").Value,
        new List<Breed>()
        {
            new(BreedId.NewId(), BreedName.Create("Labrador").Value),
            new(BreedId.NewId(), BreedName.Create("Golden Retriever").Value),
            new(BreedId.NewId(), BreedName.Create("Bulldog").Value)
        }
    );

    public static VolunteerDto VolunteerDto => new()
    {
        FullName = VolunteerCreator.CreateFullNameDto(),
        GeneralDescription = Random.Words,
        AgeExperience = Random.Experience,
        PhoneNumber = Random.PhoneNumber
    };

    public static PetDto PetDto => new()
    {
        PetName = Random.Name,
        GeneralDescription = Random.Words,
        HealthInformation = Random.Words,
        SpeciesName = "Dog",
        BreedName = "Bulldog",
        Address = VolunteerCreator.CreateAddressDto(),
        Weight = Random.Double,
        Height = Random.Double,
        PhoneNumber = Random.PhoneNumber,
        BirthDate = Random.DateTimeOffset,
        IsCastrated = Random.Bool,
        IsVaccinated = Random.Bool,
        HelpStatus = Random.HelpStatus.ToString(),
    };

    public static SpeciesDto SpeciesDto => new()
    {
        SpeciesId = Guid.NewGuid(),
        SpeciesName = "Dog",
        Breeds = new List<BreedDto>()
        {
            new()
            {
                BreedId = Guid.NewGuid(),
                BreedName = "Labrador"
            },
            new()
            {
                BreedId = Guid.NewGuid(),
                BreedName = "Golden Retriever"
            },
            new()
            {
                BreedId = Guid.NewGuid(),
                BreedName = "Bulldog"
            },
        }
    };

    public static FileDto FileDto => new()
    {
        FileName = Random.Name + ".jpg",
        ContentType = "image/jpeg",
        Content = new MemoryStream()
    };

    public static VolunteerInfo VolunteerInfo => new(
        FullName.Create(Faker.Name.FirstName(), Faker.Name.LastName()).Value,
        PhoneNumber.Create(Random.PhoneNumber).Value,
        Experience.Create(Random.Experience).Value,
        Description.Create(Random.Words).Value,
        [SocialNetwork.Create("VK", "https://vk.com").Value]
    );
}