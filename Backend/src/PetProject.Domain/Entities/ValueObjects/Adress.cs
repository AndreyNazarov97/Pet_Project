using PetProject.Domain.Consts;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities.ValueObjects;

public record Adress
{
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Street { get;  private set; }
    public string House { get; private set; } 
    public string Flat { get; private set; }

    private Adress() { }
    private Adress(string country, string city, string street, string house, string flat)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }
    
    public static Result<Adress> Create(string country, string city, string street, string house, string flat)
    {
        if(string.IsNullOrWhiteSpace(country))
            return Result<Adress>.Failure(AddressError.CountryRequired);
        if(country.Length > AdressConsts.CountryMaxLength)
            return Result<Adress>.Failure(AddressError.CountryTooLong);
        if(country.Length < AdressConsts.CountryMinLength)
            return Result<Adress>.Failure(AddressError.CountryTooShort);
        if(string.IsNullOrWhiteSpace(city))
            return Result<Adress>.Failure(AddressError.CityRequired);
        if(city.Length > AdressConsts.CityMaxLength)
            return Result<Adress>.Failure(AddressError.CityTooLong);
        if(city.Length < AdressConsts.CityMinLength)
            return Result<Adress>.Failure(AddressError.CityTooShort);
        if(string.IsNullOrWhiteSpace(street))
            return Result<Adress>.Failure(AddressError.StreetRequired);
        if(street.Length > AdressConsts.StreetMaxLength)
            return Result<Adress>.Failure(AddressError.StreetTooLong);
        if(street.Length < AdressConsts.StreetMinLength)
            return Result<Adress>.Failure(AddressError.StreetTooShort);
        if(string.IsNullOrWhiteSpace(house))
            return Result<Adress>.Failure(AddressError.HouseRequired);
        if(house.Length > AdressConsts.HouseMaxLength)
            return Result<Adress>.Failure(AddressError.HouseTooLong);
        if(house.Length < AdressConsts.HouseMinLength)
            return Result<Adress>.Failure(AddressError.HouseTooShort);
        if(string.IsNullOrWhiteSpace(flat))
            return Result<Adress>.Failure(AddressError.FlatRequired);
        if(flat.Length > AdressConsts.FlatMaxLength)
            return Result<Adress>.Failure(AddressError.FlatTooLong);
        if(flat.Length < AdressConsts.FlatMinLength)
            return Result<Adress>.Failure(AddressError.FlatTooShort);
        
        return new Adress(country, city, street, house, flat);
    }
}

