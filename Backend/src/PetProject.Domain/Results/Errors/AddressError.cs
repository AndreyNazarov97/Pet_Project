namespace PetProject.Domain.Results.Errors;

public class AddressError : Error
{
    public AddressError(string code, string description) : base(code, description)
    {
    }
    public static readonly AddressError CountryRequired = new("AddressError.CountryRequired", "Country is required");
    public static readonly AddressError CityRequired = new("AddressError.CityRequired", "City is required");
    public static readonly AddressError StreetRequired = new("AddressError.StreetRequired", "Street is required");
    public static readonly AddressError HouseRequired = new("AddressError.HouseRequired", "House is required");
    public static readonly AddressError FlatRequired = new("AddressError.FlatRequired", "Flat is required"); 
    
    public static readonly AddressError CountryTooLong = new("AddressError.CountryTooLong", "Country is too long");
    public static readonly AddressError CityTooLong = new("AddressError.CityTooLong", "City is too long");
    public static readonly AddressError StreetTooLong = new("AddressError.StreetTooLong", "Street is too long");
    public static readonly AddressError HouseTooLong = new("AddressError.HouseTooLong", "House is too long");
    public static readonly AddressError FlatTooLong = new("AddressError.FlatTooLong", "Flat is too long");
    
    public static readonly AddressError CountryTooShort = new("AddressError.CountryTooShort", "Country is too short");
    public static readonly AddressError CityTooShort = new("AddressError.CityTooShort", "City is too short");
    public static readonly AddressError StreetTooShort = new("AddressError.StreetTooShort", "Street is too short");
    public static readonly AddressError HouseTooShort = new("AddressError.HouseTooShort", "House is too short");
    public static readonly AddressError FlatTooShort = new("AddressError.FlatTooShort", "Flat is too short");
    
}