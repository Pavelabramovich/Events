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


public class AuthenticateUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    : FuncUseCase<UserLoginDto, Tokens>(unitOfWork, mapper)
{
    public override Tokens Execute(UserLoginDto userLoginDto)
    {
        throw new NotImplementedException();
    }

    public override async Task<Tokens> ExecuteAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        bool isUserValid = await _unitOfWork.UserRepository.AuthenticateAsync(userLoginDto.Login, userLoginDto.HashedPassword);

        if (!isUserValid)
            throw new UnauthorizedAccessException();

        var user = await _unitOfWork.UserRepository.FindByLoginAsync(userLoginDto.Login, cancellationToken);

        var userRoles = await _unitOfWork.RoleRepository.GetUserRolesAsync(user!.Id, cancellationToken).ToArrayAsync(cancellationToken);
        var userClaims = await _unitOfWork.ClaimRepository.GetUserClaimsAsync(user!.Id, cancellationToken).ToArrayAsync(cancellationToken);

        var claimsFromRoles = userRoles.Select(r => new SystemClaim(ClaimTypes.Role, r.Name));
        var claimsFromClaims = userClaims.Select(c => new SystemClaim(c.Type, c.Value));

        SystemClaim[] claims = [.. claimsFromRoles, .. claimsFromClaims];

        var tokens = JwtTokenManager.GenerateToken(user.Id, claims);

        if (tokens is null)
            throw new UnauthorizedAccessException();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Value = tokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(refreshToken);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException();

        return tokens;
    }
}

