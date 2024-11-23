namespace PetProject.Application.Dto;

public record FullNameDto()
{
    public required string Name { get; init; } 
    public required string Surname { get; init; } 
    public string? Patronymic { get; init; }
}
