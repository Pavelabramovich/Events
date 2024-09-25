using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;


namespace Events.WebApi.ExceptionMiddleware;


public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(validationException.Message, cancellationToken);

            return true;
        }

        return false;
    }
}
