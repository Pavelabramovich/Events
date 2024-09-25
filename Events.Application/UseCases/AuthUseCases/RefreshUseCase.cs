using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;
using Events.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace Events.Application.UseCases;


public class RefreshUseCase : FuncUseCase<TokensDto, TokensDto>
{
    private readonly JwtTokenManager _jwtTokenManager;


    public RefreshUseCase(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        : base(unitOfWork, mapper)
    {
        _jwtTokenManager = new JwtTokenManager(configuration);
    }


    public override TokensDto Execute(TokensDto tokens)
    {
        throw new NotImplementedException();
    }

    public override async Task<TokensDto> ExecuteAsync(TokensDto tokens, CancellationToken cancellationToken = default)
    {
        var principal = _jwtTokenManager.GetPrincipalFromExpiredToken(tokens.AccessToken);

        var claims = principal.Claims;

        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!;
        int userId = int.Parse(userIdClaim.Value);

        var savedRefreshToken = await _unitOfWork.RefreshTokenRepository.GetSavedRefreshTokedAsync(userId, tokens.RefreshToken, cancellationToken);

        if (savedRefreshToken?.Value != tokens.RefreshToken)
            throw new UnauthorizedAccessException();

        var newTokens = _jwtTokenManager.GenerateRefreshToken(userId, claims.ToArray()) 
            ?? throw new UnauthorizedAccessException();

        var newRefreshToken = new RefreshToken
        {
            UserId = userId,
            Value = newTokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newTokens;
    }
}
