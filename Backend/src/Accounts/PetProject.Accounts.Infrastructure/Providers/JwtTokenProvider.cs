using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Application;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.Options;
using PetProject.Core.Models;

namespace PetProject.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly IPermissionManager _permissionManager;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        IPermissionManager permissionManager,
        IOptions<JwtOptions> options)
    {
        _permissionManager = permissionManager;
        _jwtOptions = options.Value;
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(r => 
            new Claim(CustomClaims.Role, r.Name ?? string.Empty));
        
        var permissions = await _permissionManager.GetUserPermissionsAsync(user.Id);
        var permissionClaims = permissions.Select(p => 
            new Claim(CustomClaims.Permission, p.Code));
        
        var claims = new[]
        {
            new Claim(CustomClaims.Id, user.Id.ToString()),
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
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return tokenString;
    }
}