namespace PetProject.Application.AccountManagement.Commands;

public record RegisterUserCommand(string Email, string Username ,string Password);