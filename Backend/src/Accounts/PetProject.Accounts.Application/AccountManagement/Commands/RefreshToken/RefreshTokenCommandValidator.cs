using FluentValidation;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        
    }
}