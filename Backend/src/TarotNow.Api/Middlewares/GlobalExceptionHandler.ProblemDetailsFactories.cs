using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    private static ProblemDetails CreateBadRequestProblem(string detail)
        => CreateClientProblem(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            detail);

    private static ProblemDetails CreateNotFoundProblem(string detail)
        => CreateClientProblem(
            StatusCodes.Status404NotFound,
            "Not Found",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            detail);

    private static ProblemDetails CreateConflictProblem(string detail)
        => CreateClientProblem(
            StatusCodes.Status409Conflict,
            "Conflict",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            detail);

    private static ProblemDetails CreateInvalidOperationProblem(string detail)
        => CreateClientProblem(
            StatusCodes.Status422UnprocessableEntity,
            "Invalid Operation",
            "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
            detail);

    private static ProblemDetails CreateUnauthorizedProblem()
        => CreateClientProblem(
            StatusCodes.Status401Unauthorized,
            "Unauthorized",
            "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            "You are not authorized to access this resource.");

    private static ProblemDetails CreateServerProblem()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred while processing your request. Please try again later."
        };
    }

    private static ProblemDetails CreateClientProblem(int status, string title, string type, string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = type,
            Detail = detail
        };
    }
}
