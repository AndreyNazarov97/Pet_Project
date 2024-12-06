using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Infrastructure.Options;

namespace PetProject.Accounts.Infrastructure;

public static class TokenValidationParametersFactory
{
    public static TokenValidationParameters Create(JwtOptions jwtOptions, bool withLifeTime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = withLifeTime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    }
}