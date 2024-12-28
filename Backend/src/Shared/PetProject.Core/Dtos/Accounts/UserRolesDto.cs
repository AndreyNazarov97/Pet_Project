namespace PetProject.Core.Dtos.Accounts;

public record UserRolesDto
{
    public required long UserId { get; init; }
    public UserDto User { get; init; } = default!;
    public required long RoleId { get; init; }
    public RoleDto Role { get; init; } = default!;
}