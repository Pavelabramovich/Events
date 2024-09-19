
namespace Events.WebApi.App;


public static class AddConfigurationExtension
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder @this)
    {
        @this.Configuration.AddJsonFile("secrets.json");

        return @this;
    }
}
