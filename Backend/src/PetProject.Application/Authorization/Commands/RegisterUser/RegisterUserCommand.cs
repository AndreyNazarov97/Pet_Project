namespace PetProject.Application.Authorization.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Username ,string Password);