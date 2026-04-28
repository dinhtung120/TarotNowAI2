using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Services;

public sealed record AiStreamEndpointProblem(
    int Status,
    string Title,
    string Detail,
    string Type)
{
    public ProblemDetails ToProblemDetails()
    {
        return new ProblemDetails
        {
            Status = Status,
            Title = Title,
            Detail = Detail,
            Type = Type
        };
    }
}

public static class AiStreamEndpointProblemExtensions
{
    public static IActionResult ToActionResult(this AiStreamEndpointProblem problem)
    {
        return new ObjectResult(problem.ToProblemDetails()) { StatusCode = problem.Status };
    }

    public static Task WriteAsync(
        this AiStreamEndpointProblem problem,
        HttpResponse response,
        CancellationToken cancellationToken)
    {
        response.StatusCode = problem.Status;
        return response.WriteAsJsonAsync(problem.ToProblemDetails(), cancellationToken);
    }
}
