
using Events.DataBase;
using Events.Domain;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Events.WebApi.App;


public static class AddServicesExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder @this)
    {
        @this.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
        @this.Services.AddSingleton<IJwtManagerRepository, JwtManagerRepository>();

        @this.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        @this.Services.AddScoped<IUnitOfWorkWithTokens, UnitOfWorkWithTokens>();

        return @this;
    }
}
