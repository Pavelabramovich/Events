using Events.WebApi.Db;
using Microsoft.AspNetCore.Identity;
using Events.WebApi.Dto;
using Microsoft.EntityFrameworkCore;


namespace Events.WebApi.Authentication;


public class UserServiceRepository : IUserServiceRepository
{
    private readonly EventsContext _context;


    public UserServiceRepository(UserManager<IdentityUser> userManager, EventsContext context)
    {
        _context = context;
    }

    public RefreshToken UpsertUserRefreshToken(RefreshToken user)
    {
        _context.UserRefreshToken.Upsert(user).Run();
        _context.SaveChanges();

        return user;
    }

    public void DeleteUserRefreshToken(string email, string refreshToken)
    {
        int userId = _context.Users.First(u => u.Email == email).Id;

        var item = _context.UserRefreshToken.FirstOrDefault(t => t.UserId == userId && t.Value == refreshToken);

        if (item is not null)
        {
            _context.UserRefreshToken.Remove(item);
        }
    }

    public RefreshToken GetSavedRefreshToken(string email, string refreshToken)
    {
        var userId = _context.Users.First(u => u.Email == email).Id;

        return _context.UserRefreshToken.FirstOrDefault(t => t.UserId == userId && t.Expires >= DateTime.UtcNow);
    }

    public async Task<bool> IsValidUserAsync(UserLoginDto users)
    {
        var u = _context.Users.FirstOrDefault(o => o.Email == users.Login && o.Password == users.Password);

        return u is not null;
    }
}