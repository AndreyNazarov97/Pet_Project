using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Models;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);
    Task<Result<Claim[], Error>> GetUserClaimsFromJwtToken(string jwtToken, CancellationToken cancellationToken = default);

}