using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Application;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Application.Models;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.Options;
using PetProject.Core.Common;
using PetProject.Core.Models;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountsDbContext _context;
    private readonly IPermissionManager _permissionManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        AccountsDbContext context,
        IPermissionManager permissionManager,
        IOptions<JwtOptions> options,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _permissionManager = permissionManager;
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = options.Value;
    }

    public async Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles.Select(r =>
            new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var permissions = await _permissionManager.GetUserPermissionsAsync(user.Id, cancellationToken);
        var permissionClaims = permissions.Select(p =>
            new Claim(CustomClaims.Permission, p.Code));

        var jti = Guid.NewGuid();

        var claims = new[]
        {
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? ""),
            new Claim(CustomClaims.Username, user.UserName!)
        }.ToList();

        claims.AddRange(roleClaims);
        claims.AddRange(permissionClaims);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: credentials,
            claims: claims
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtTokenResult(accessTokenString, jti);
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            Jti = jti,
            UserId = user.Id,
            CreatedAt = _dateTimeProvider.Now,
            ExpiresAt = _dateTimeProvider.Now.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            RefreshToken = Guid.NewGuid()
        };

        _context.RefreshSessions.Add(refreshSession);
        await _context.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }

    public async Task<Result<Claim[], Error>> GetUserClaimsFromJwtToken(string jwtToken, CancellationToken cancellationToken = default)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var validationParameters = TokenValidationParametersFactory.Create(_jwtOptions, false);
        
        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
        if (!validationResult.IsValid)
        {
            return Errors.Tokens.InvalidToken();
        }
        
        var claims = validationResult.ClaimsIdentity.Claims.ToArray();

        return claims;
    }
}