namespace PetProject.Core.Dtos;

public record AddressDto()
{
    public required string Country { get; init; }
    public required string City { get; init; } 
    public required string Street { get; init; } 
    public required string House { get; init; }
    public required string Flat { get; init; } 
}