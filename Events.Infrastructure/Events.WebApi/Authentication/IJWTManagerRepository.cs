using System.Security.Claims;


namespace Events.WebApi.Authentication;


public interface IJWTManagerRepository
{
    Tokens GenerateToken(string userName, params Claim[] additionalClaims);
    Tokens GenerateRefreshToken(string userName, params Claim[] additionalClaims);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
