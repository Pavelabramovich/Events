using Events.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Events.WebApi.ExceptionMiddleware;


public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;


    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }


    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var problemDetails = CreateProblemDetails(context, exception);
        var json = ToJson(problemDetails);

        string contentType = "application/problem+json";
        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(json, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);

        if (string.IsNullOrEmpty(reasonPhrase))
        {
            reasonPhrase = "An unhandled exception has occurred while executing the request.";
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = reasonPhrase
        };

        problemDetails.Detail = exception.Message;
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    private string ToJson(ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails, SerializerOptions);
        }
        catch (Exception exception)
        {
            string jsonErrorMessage = "An exception has occurred while serializing error to JSON";
            _logger.LogError(exception, jsonErrorMessage);
        }

        return string.Empty;
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        EntityNotFoundException => StatusCodes.Status404NotFound,
        ParticipationsOverflowException => StatusCodes.Status400BadRequest,
        DuplicatedIdentifierException => StatusCodes.Status400BadRequest,

        _ => StatusCodes.Status500InternalServerError
    };
}
