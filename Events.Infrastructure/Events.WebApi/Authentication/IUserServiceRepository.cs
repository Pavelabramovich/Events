using Events.WebApi.Dto;


namespace Events.WebApi.Authentication;


public interface IUserServiceRepository
{
    Task<bool> IsValidUserAsync(UserLoginDto users);

    RefreshToken UpsertUserRefreshToken(RefreshToken user);
    RefreshToken GetSavedRefreshToken(string username, string refreshtoken);

    void DeleteUserRefreshToken(string username, string refreshToken);
}
