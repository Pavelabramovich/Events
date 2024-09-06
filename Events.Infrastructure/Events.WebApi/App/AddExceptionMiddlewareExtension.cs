
using Events.DataBase;
using Events.Domain;
using Events.WebApi.Authentication;
using Events.WebApi.ExceptionMiddleware;
using Microsoft.AspNetCore.Authorization;

namespace Events.WebApi.App;


public static class AddExceptionMiddlewareExtension
{
    public static WebApplicationBuilder AddExceptionMiddleware(this WebApplicationBuilder @this)
    {
        @this.Services.AddExceptionHandler<ValidationExceptionHandler>();
        @this.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return @this;
    }
}
