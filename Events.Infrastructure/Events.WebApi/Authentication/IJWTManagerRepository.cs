using System.Security.Claims;


namespace Events.WebApi.Authentication;


public interface IJwtManagerRepository
{
    Tokens? GenerateToken(int userId, params Claim[] additionalClaims);
    Tokens? GenerateRefreshToken(int userId, params Claim[] additionalClaims);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
