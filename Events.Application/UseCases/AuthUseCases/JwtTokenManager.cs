using Events.Application.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using DomainClaim = Events.Domain.Claim;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.Application.UseCases;


internal static class JwtTokenManager
{
    private static readonly string _jwtKey = "This is my secret key for jwt refresh token";


    public static Tokens? GenerateToken(int userId, params SystemClaim[] additionalClaims)
    {
        return GenerateJWTTokens(userId, additionalClaims);
    }

    public static Tokens? GenerateRefreshToken(int userId, params SystemClaim[] additionalClaims)
    {
        return GenerateJWTTokens(userId, additionalClaims);
    }


    public static Tokens? GenerateJWTTokens(int userId, params SystemClaim[] additionalClaims)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtKey);

            var idClaim = new SystemClaim(ClaimTypes.NameIdentifier, userId.ToString());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([idClaim, .. additionalClaims]),

                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            return new Tokens() { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}