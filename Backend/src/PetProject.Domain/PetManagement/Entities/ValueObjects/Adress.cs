using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public class Adress : ValueObject
{
    private Adress()
    {
    }

    private Adress(string country, string city, string street, string house, string flat)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string House { get; }
    public string Flat { get; }


    public static Result<Adress> Create(string country, string city, string street, string house, string flat)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(country));
        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(city));
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(street));
        if (string.IsNullOrWhiteSpace(house) || house.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(house));
        if (string.IsNullOrWhiteSpace(flat) || flat.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(flat));
         
        var address = new Adress(country, city, street, house, flat);
        return Result<Adress>.Success(address);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return House;
        yield return Flat;
    }

    public override string ToString()
    {
        return $"{Country}, {City}, {Street}, {House}, {Flat}";
    }
}