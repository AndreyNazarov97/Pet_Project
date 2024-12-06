using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Contracts.Responses;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.LoginUser;

public class LoginHandler : IRequestHandler<LoginUserCommand, Result<LoginResponse, ErrorList>>
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

    public async Task<Result<LoginResponse, ErrorList>> Handle(
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

        var tokenResult = await _tokenProvider.GenerateAccessToken(user, cancellationToken);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, tokenResult.Jti, cancellationToken);

        _logger.LogInformation("User {UserId} logged in", user.Id);

        return new LoginResponse(tokenResult.AccessToken, refreshToken);
    }
}