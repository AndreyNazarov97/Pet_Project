using Bogus;
using PetProject.Core.Dtos;

namespace PetProject.SharedTestData.Creators;

public class VolunteerCreator
{
    private static readonly Faker<FullNameDto> FullNameFaker = new Faker<FullNameDto>()
        .RuleFor(v => v.Name, faker => faker.Name.FirstName())
        .RuleFor(v => v.Surname, faker => faker.Name.LastName())
        .RuleFor(v => v.Patronymic, faker => faker.Name.FirstName()+ "ich");

    private static readonly Faker<AddressDto> AddressFaker = new Faker<AddressDto>()
        .RuleFor(v => v.Country, faker => faker.Address.Country())
        .RuleFor(v => v.City, faker => faker.Address.City())
        .RuleFor(v => v.Street, faker => faker.Address.StreetName())
        .RuleFor(v => v.House, faker => faker.Address.BuildingNumber())
        .RuleFor(v => v.Flat, faker => faker.Random.Number(1, 100).ToString());
    
    public static FullNameDto CreateFullNameDto() => FullNameFaker.Generate();
    
    public static AddressDto CreateAddressDto() => AddressFaker.Generate();
}