using Events.WebApi.Dto;


namespace Events.WebApi.Authentication;


public interface IUserServiceRepository
{
    Task<bool> IsValidUserAsync(UserLoginDto users);

    UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);
    UserRefreshTokens GetSavedRefreshTokens(string username, string refreshtoken);

    void DeleteUserRefreshTokens(string username, string refreshToken);
}
