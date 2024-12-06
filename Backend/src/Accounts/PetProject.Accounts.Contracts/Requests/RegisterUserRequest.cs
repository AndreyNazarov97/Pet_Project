using PetProject.Core.Dtos;

namespace PetProject.Accounts.Contracts.Requests;

public record RegisterUserRequest(FullNameDto FullName, string Email, string UserName, string Password);
