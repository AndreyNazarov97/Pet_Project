﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Application;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.Models;
using PetProject.Accounts.Infrastructure.Options;

namespace PetProject.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(r => 
            new Claim(CustomClaim.Role, r.Name ?? string.Empty));
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
        }.ToList();
        
        claims.AddRange(roleClaims);
        
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