using System.Security.Claims;


namespace Events.WebApi.Authentication;


public interface IJWTManagerRepository
{
    Tokens GenerateToken(string userName);
    Tokens GenerateRefreshToken(string userName);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
