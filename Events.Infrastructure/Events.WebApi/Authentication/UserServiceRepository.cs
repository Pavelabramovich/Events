using Events.WebApi.Db;
using Microsoft.AspNetCore.Identity;
using Events.WebApi.Dto;


namespace Events.WebApi.Authentication;


public class UserServiceRepository : IUserServiceRepository
{
    private readonly EventsContext _db;


    public UserServiceRepository(UserManager<IdentityUser> userManager, EventsContext db)
    {
        this._db = db;
    }

    public UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user)
    {
        _db.UserRefreshToken.Add(user);
        _db.SaveChanges();
        return user;
    }

    public void DeleteUserRefreshTokens(string username, string refreshToken)
    {
        var item = _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken);
        if (item != null)
        {
            _db.UserRefreshToken.Remove(item);
        }
    }

    public UserRefreshTokens GetSavedRefreshTokens(string username, string refreshToken)
    {
        return _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true);
    }

    public async Task<bool> IsValidUserAsync(UserLoginDto users)
    {
        var u = _db.Users.FirstOrDefault(o => o.Email == users.Login && o.Password == users.Password);

        if (u != null)
            return true;
        else
            return false;
    }
}