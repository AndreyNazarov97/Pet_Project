using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetProject.Application.AccountManagement.DataModels;
using PetProject.Domain.Shared;

namespace PetProject.Application.AccountManagement.Commands;

public class RegisterUserHandler
{
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(
        UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<Error>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var existedUser = await _userManager.FindByEmailAsync(command.Email); 
        if (existedUser != null)
            return Errors.Model.AlreadyExist("user");
        
        var result = await _userManager.CreateAsync(new User(), command.Password);

        return UnitResult.Success<Error>();
    }
}