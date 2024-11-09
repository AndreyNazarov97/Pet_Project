using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Application.Authorization.DataModels;
using PetProject.Domain.Shared;

namespace PetProject.Application.Authorization.Commands.RegisterUser;

public class RegisterUserHandler
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            Email = command.Email,
            UserName = command.Username
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {username} was registered", user.UserName);
            return UnitResult.Success<ErrorList>();
        }

        var errors = result.Errors.Select(e =>
            Error.Failure(e.Code, e.Description)).ToList();

        return new ErrorList(errors);
    }
}