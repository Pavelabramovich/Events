using Events.WebApi.ExceptionMiddleware;


namespace Events.WebApi.App;


public static class AddExceptionMiddlewareExtension
{
    public static WebApplicationBuilder AddExceptionMiddleware(this WebApplicationBuilder @this)
    {
        @this.Services.AddProblemDetails();

        @this.Services.AddExceptionHandler<ValidationExceptionHandler>();
        @this.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return @this;
    }
}
