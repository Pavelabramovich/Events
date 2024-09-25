using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;
using Events.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DomainClaim = Events.Domain.Entities.Claim;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.Application.UseCases;


public class AuthenticateUseCase : FuncUseCase<UserLoginDto, TokensDto>
{
    private readonly JwtTokenManager _jwtTokenManager;


    public AuthenticateUseCase(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        : base(unitOfWork, mapper)
    {
        _jwtTokenManager = new JwtTokenManager(configuration);
    }


    public override TokensDto Execute(UserLoginDto userLoginDto)
    {
        throw new NotImplementedException();
    }

    public override async Task<TokensDto> ExecuteAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        bool isUserValid = await _unitOfWork.UserRepository.AuthenticateAsync(userLoginDto.Login, userLoginDto.HashedPassword);

        if (!isUserValid)
            throw new UnauthorizedAccessException();

        var user = await _unitOfWork.UserRepository.FindByLoginAsync(userLoginDto.Login, cancellationToken)
            ?? throw new EntityNotFoundException(userLoginDto.Login, "Invalid user login");

        var userRoles = await _unitOfWork.RoleRepository.GetUserRolesAsync(user!.Id, cancellationToken).ToArrayAsync(cancellationToken);
        var userClaims = await _unitOfWork.ClaimRepository.GetUserClaimsAsync(user!.Id, cancellationToken).ToArrayAsync(cancellationToken);

        var claimsFromRoles = userRoles.Select(r => new SystemClaim(ClaimTypes.Role, r.Name));
        var claimsFromClaims = userClaims.Select(c => new SystemClaim(c.Type, c.Value));

        SystemClaim[] claims = [.. claimsFromRoles, .. claimsFromClaims];

        var tokens = _jwtTokenManager.GenerateToken(user.Id, claims)
            ?? throw new UnauthorizedAccessException();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Value = tokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return tokens;
    }
}
