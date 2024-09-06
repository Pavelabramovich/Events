
using Events.DataBase;
using Events.Domain;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Events.WebApi.App;


public static class AddConfigurationExtension
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder @this)
    {
        @this.Configuration.AddJsonFile("secrets.json");

        return @this;
    }
}
