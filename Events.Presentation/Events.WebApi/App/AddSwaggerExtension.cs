using Microsoft.OpenApi.Models;


namespace Events.WebApi.App;


public static class AddSwaggerExtension
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder @this)
    {
        @this.Services.AddSwaggerGen(setup =>
        { 
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Events API",
                Version = "v1"
            });
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer 1safsfsdfdfd\"",
            });
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                [
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }
                ] = []
            });
        });

        return @this;
    }
}
