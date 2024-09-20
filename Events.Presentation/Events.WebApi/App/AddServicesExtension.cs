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
        @this.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        @this.Services.AddScoped<GetAllEventsUseCase>();
        @this.Services.AddScoped<GetEventByIdUseCase>();
        @this.Services.AddScoped<GetEventByNameUseCase>();
        @this.Services.AddScoped<GetAllEventsWithParticipantsUseCase>();
        @this.Services.AddScoped<GetParticipantsByEventIdUseCase>();
        @this.Services.AddScoped<GetEventsPageUseCase>();
        @this.Services.AddScoped<CreateEventUseCase>();
        @this.Services.AddScoped<UpdateEventUseCase>();
        @this.Services.AddScoped<UpdateEventParticipationUseCase>();
        @this.Services.AddScoped<RemoveEventUseCase>();
        @this.Services.AddScoped<RemoveEventParticipationUseCase>();

        @this.Services.AddScoped<GetAllRolesUseCase>();
        @this.Services.AddScoped<IsRoleExistsUseCase>();
        @this.Services.AddScoped<CreateRoleUseCase>();
        @this.Services.AddScoped<UpdateRoleUseCase>();
        @this.Services.AddScoped<RemoveRoleUseCase>();

        @this.Services.AddScoped<GetAllUsersUseCase>();
        @this.Services.AddScoped<GetAllUsersWithParticipationsUseCase>();
        @this.Services.AddScoped<GetUserByIdUseCase>();
        @this.Services.AddScoped<GetUserByLoginUseCase>();
        @this.Services.AddScoped<GetParticipationsByUserIdUseCase>();
        @this.Services.AddScoped<GetUsersPageUseCase>();
        @this.Services.AddScoped<CreateUserUseCase>();
        @this.Services.AddScoped<UpdateUserUseCase>();
        @this.Services.AddScoped<RemoveUserUseCase>();

        @this.Services.AddScoped<AuthenticateUseCase>();
        @this.Services.AddScoped<RefreshUseCase>();


        return @this;
    }
}
