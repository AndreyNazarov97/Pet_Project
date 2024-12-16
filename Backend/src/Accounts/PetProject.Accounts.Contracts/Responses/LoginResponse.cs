namespace PetProject.Accounts.Contracts.Responses;

public record LoginResponse(string AccessToken, Guid RefreshToken, long UserId, string Email);