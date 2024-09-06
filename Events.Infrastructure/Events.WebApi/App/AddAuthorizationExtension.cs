
using Events.DataBase;
using Events.Domain;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Events.WebApi.App;


public static class AddAuthorizationExtension
{
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder @this)
    {
        @this.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.AddRequirements(new RoleRequirement("Admin")));
        });

        return @this;
    }
}
