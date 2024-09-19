using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using DomainClaim = Events.Domain.Claim;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.Application.UseCases;


public class RefreshUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<Tokens, Tokens>(unitOfWork, mapper)
{
    public override Tokens Execute(Tokens tokens)
    {
        throw new NotImplementedException();
    }

    public override async Task<Tokens> ExecuteAsync(Tokens tokens, CancellationToken cancellationToken = default)
    {
        var principal = JwtTokenManager.GetPrincipalFromExpiredToken(tokens.AccessToken);

        var claims = principal.Claims;

        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!;
        int userId = int.Parse(userIdClaim.Value);

        var savedRefreshToken = await _unitOfWork.RefreshTokenRepository.GetSavedRefreshTokedAsync(userId, tokens.RefreshToken);

        if (savedRefreshToken?.Value != tokens.RefreshToken)
            throw new UnauthorizedAccessException();

        var newTokens = JwtTokenManager.GenerateRefreshToken(userId, claims.ToArray());

        if (newTokens is null)
            throw new UnauthorizedAccessException();

        var newRefreshToken = new RefreshToken
        {
            UserId = userId,
            Value = newTokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(newRefreshToken);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException();

        return newTokens;
    }
}