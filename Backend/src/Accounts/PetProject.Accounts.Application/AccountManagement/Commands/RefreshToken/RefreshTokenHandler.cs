using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.Interfaces;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Contracts.Responses;
using PetProject.Core.Database;
using PetProject.Core.Models;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse, ErrorList>>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenHandler(
        ITokenProvider tokenProvider,
        IRefreshSessionManager refreshSessionManager,
        IDateTimeProvider dateTimeProvider,
        [FromKeyedServices(Constants.Context.Accounts)]IUnitOfWork unitOfWork)
    {
        _tokenProvider = tokenProvider;
        _refreshSessionManager = refreshSessionManager;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshSession = await _refreshSessionManager
            .GetByRefreshTokenAsync(command.RefreshToken, cancellationToken);

        if (refreshSession is null)
            return Errors.General.NotFound(command.RefreshToken).ToErrorList();

        if (refreshSession.ExpiresAt < _dateTimeProvider.UtcNow)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }
        
        _refreshSessionManager.Delete(refreshSession);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var jwtTokenResult = await _tokenProvider.GenerateAccessToken(refreshSession.User, cancellationToken);
        var refreshToken =
            await _tokenProvider.GenerateRefreshToken(refreshSession.User, jwtTokenResult.Jti, cancellationToken);

        return new LoginResponse(jwtTokenResult.AccessToken, refreshToken, refreshSession.User.Id, refreshSession.User.Email!);
    }
}