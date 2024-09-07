using Events.DataBase;
using Events.Application;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using Events.Application.UseCases;


namespace Events.WebApi.App;


public static class AddServicesExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder @this)
    {
        @this.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
        @this.Services.AddSingleton<IJwtManagerRepository, JwtManagerRepository>();

        @this.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        @this.Services.AddScoped<IUnitOfWorkWithTokens, UnitOfWorkWithTokens>();

        @this.Services.AddScoped<EventUseCases.GetAll>();
        @this.Services.AddScoped<EventUseCases.GetById>();
        @this.Services.AddScoped<EventUseCases.GetByName>();
        @this.Services.AddScoped<EventUseCases.GetAllWithParticipants>();
        @this.Services.AddScoped<EventUseCases.GetParticipantsById>();
        @this.Services.AddScoped<EventUseCases.GetPage>();
        @this.Services.AddScoped<EventUseCases.Create>();
        @this.Services.AddScoped<EventUseCases.Update>();
        @this.Services.AddScoped<EventUseCases.UpdatePaticipation>();
        @this.Services.AddScoped<EventUseCases.Remove>();
        @this.Services.AddScoped<EventUseCases.RemoveParticipation>();

        @this.Services.AddScoped<RoleUseCases.GetAll>();
        @this.Services.AddScoped<RoleUseCases.Exists>();
        @this.Services.AddScoped<RoleUseCases.Create>();
        @this.Services.AddScoped<RoleUseCases.Update>();
        @this.Services.AddScoped<RoleUseCases.Remove>();

        @this.Services.AddScoped<UserUseCases.GetAll>();
        @this.Services.AddScoped<UserUseCases.GetAllWithParticipants>();
        @this.Services.AddScoped<UserUseCases.GetById>();
        @this.Services.AddScoped<UserUseCases.GetByLogin>();
        @this.Services.AddScoped<UserUseCases.GetParticipantsById>();
        @this.Services.AddScoped<UserUseCases.GetPage>();
        @this.Services.AddScoped<UserUseCases.Create>();
        @this.Services.AddScoped<UserUseCases.Update>();
        @this.Services.AddScoped<UserUseCases.Remove>();

        @this.Services.AddScoped<AuthUseCases.Authenticate>();
        @this.Services.AddScoped<AuthUseCases.Refresh>();


        return @this;
    }
}
