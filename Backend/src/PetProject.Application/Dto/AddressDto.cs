using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Dto;

public record AddressDto(string Country, string City, string Street, string House, string Flat)
{
    public Address ToEntity()
    {
        var address = Address.Create(Country, City, Street, House, Flat).Value;
        
        return address;
    }
}