using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TarotNow.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        if (elapsedMs > 500)
        {
            _logger.LogWarning(
                "[MediatR] Slow request {RequestName} took {ElapsedMs}ms",
                typeof(TRequest).Name,
                elapsedMs);
        }

        return response;
    }
}

