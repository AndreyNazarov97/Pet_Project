using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Application.Authorization.DataModels;
using PetProject.Domain.Shared;

namespace PetProject.Application.Authorization.Commands.LoginUser;

public class LoginHandler : IRequestHandler<LoginUserCommand, Result<string,ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    public async Task<Result<string,ErrorList>> Handle(
        LoginUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            return Errors.General.NotFound().ToErrorList();
        }
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);

        if (!passwordConfirmed)
        {
            return Errors.User.InvalidCredentials().ToErrorList();
        }
        
        var token = _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("User {UserId} logged in", user.Id);
        
        return token;
    }

}