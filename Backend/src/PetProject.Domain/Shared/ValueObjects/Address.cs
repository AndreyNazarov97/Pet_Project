using CSharpFunctionalExtensions;

namespace PetProject.Domain.Shared.ValueObjects;

public record Address
{
    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string House { get; }
    public string Flat { get; }

    private Address(string country, string city, string street, string house, string flat)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }

    public static Result<Address, Error> Create(string country, string city, string street, string house, string flat)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(country));
        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(city));
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(street));
        if (string.IsNullOrWhiteSpace(house) || house.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(house));
        if (string.IsNullOrWhiteSpace(flat) || flat.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(flat));

        var address = new Address(country, city, street, house, flat);
        return address;
    }
}