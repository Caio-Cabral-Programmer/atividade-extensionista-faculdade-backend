using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            BusinessRuleException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            ForbiddenException => (StatusCodes.Status403Forbidden, "Acesso negado"),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning(exception, "Handled exception ({StatusCode}): {Message}", statusCode, exception.Message);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
