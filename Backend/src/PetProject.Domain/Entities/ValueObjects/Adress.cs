using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class Adress : ValueObject
{
    private Adress(){}
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
        if(string.IsNullOrWhiteSpace(country) || country.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Adress>.Failure(new("Invalid country", $"{nameof(country)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));
        if(string.IsNullOrWhiteSpace(city) || city.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Adress>.Failure(new("Invalid city", $"{nameof(city)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));
        if(string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Adress>.Failure(new("Invalid street", $"{nameof(street)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));
        if(string.IsNullOrWhiteSpace(house) || house.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Adress>.Failure(new("Invalid house", $"{nameof(house)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));
        if(string.IsNullOrWhiteSpace(flat) || flat.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Result<Adress>.Failure(new("Invalid flat", $"{nameof(flat)} cannot be null or empty or longer than {Constants.MAX_SHORT_TEXT_LENGTH} characters"));

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
}

