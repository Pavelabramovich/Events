using System.Security.Claims;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.WebApi.Authentication;


public interface IJwtManagerRepository
{
    Tokens? GenerateToken(int userId, params SystemClaim[] additionalClaims);
    Tokens? GenerateRefreshToken(int userId, params SystemClaim[] additionalClaims);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
