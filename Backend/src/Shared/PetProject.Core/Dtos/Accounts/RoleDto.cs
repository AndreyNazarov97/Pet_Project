namespace PetProject.Core.Dtos.Accounts;

public record RoleDto
{
    public required long RoleId { get; init; }
    public required string Name { get; init; }
}