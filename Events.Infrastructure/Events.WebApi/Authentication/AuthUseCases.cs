using AutoMapper;
using Events.Application.Dto;
using Events.Application.UseCases;
using Events.DataBase;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DomainClaim = Events.Domain.Claim;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.WebApi.Authentication;


public static class AuthUseCases
{
    public class Authenticate(IUnitOfWorkWithTokens unitOfWork, IMapper mapper, IJwtManagerRepository jwtManager) 
        : FuncUseCase<UserLoginDto, Tokens>(unitOfWork, mapper)
    {
        private readonly IJwtManagerRepository _jwtManager = jwtManager;


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

            var tokens = _jwtManager.GenerateToken(user.Id, claims);

            if (tokens is null)
                throw new UnauthorizedAccessException();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Value = tokens.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            ((IUnitOfWorkWithTokens)_unitOfWork).RefreshTokenRepository.UpsertUserRefreshToken(refreshToken);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException();

            return tokens;
        }
    }

    public class Refresh(IUnitOfWorkWithTokens unitOfWork, IMapper mapper, IJwtManagerRepository jwtManager) : FuncUseCase<Tokens, Tokens>(unitOfWork, mapper)
    {
        private readonly IJwtManagerRepository _jwtManager = jwtManager;


        public override Tokens Execute(Tokens tokens)
        {
            throw new NotImplementedException();
        }

        public override async Task<Tokens> ExecuteAsync(Tokens tokens, CancellationToken cancellationToken = default)
        {
            var principal = _jwtManager.GetPrincipalFromExpiredToken(tokens.AccessToken);

            var claims = principal.Claims;

            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!;
            int userId = int.Parse(userIdClaim.Value);

            var savedRefreshToken = await ((IUnitOfWorkWithTokens)_unitOfWork).RefreshTokenRepository.GetSavedRefreshTokedAsync(userId, tokens.RefreshToken);

            if (savedRefreshToken?.Value != tokens.RefreshToken)
                throw new UnauthorizedAccessException();

            var newTokens = _jwtManager.GenerateRefreshToken(userId, claims.ToArray());

            if (newTokens is null)
                throw new UnauthorizedAccessException();

            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                Value = newTokens.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(30)
            };

            ((IUnitOfWorkWithTokens)_unitOfWork).RefreshTokenRepository.UpsertUserRefreshToken(newRefreshToken);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException();

            return newTokens;
        }
    }
}
