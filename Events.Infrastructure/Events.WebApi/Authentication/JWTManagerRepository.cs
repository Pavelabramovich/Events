using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Events.WebApi.Authentication;


public class JWTManagerRepository : IJWTManagerRepository
{
    private readonly IConfiguration _configuration;


    public JWTManagerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public Tokens GenerateToken(string userName, params Claim[] additionalClaims)
    {
        return GenerateJWTTokens(userName, additionalClaims);
    }

    public Tokens GenerateRefreshToken(string username, params Claim[] additionalClaims)
    {
        return GenerateJWTTokens(username, additionalClaims);
    }


    public Tokens GenerateJWTTokens(string userName, params Claim[] additionalClaims)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            // токен будет с нужным алго и ключём, истекать через одну минуту и с именем юзера в payload
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, userName), ..additionalClaims]),

                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            // создаём токены
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            // возвращаем токены
            return new Tokens() { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public string GenerateRefreshToken()
    {
        // просто генерим рандомный токен
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // достаём секретный ключ
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        // указываем как проверять токен
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        // проверяем токен на ключ
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        // и проверяем на нужный алгоритм шифрования
        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}