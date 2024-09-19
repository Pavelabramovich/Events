using Events.WebApi.Authentication;


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
