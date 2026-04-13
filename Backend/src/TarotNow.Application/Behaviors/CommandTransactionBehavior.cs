using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Behaviors;

/// <summary>
/// Pipeline behavior bọc command trong transaction thống nhất để bảo toàn atomicity business write + outbox.
/// </summary>
public sealed class CommandTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo behavior transaction cho command pipeline.
    /// </summary>
    public CommandTransactionBehavior(ITransactionCoordinator transactionCoordinator)
    {
        _transactionCoordinator = transactionCoordinator;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (IsCommandRequest() == false)
        {
            return await next();
        }

        var response = default(TResponse);
        await _transactionCoordinator.ExecuteAsync(
            async _ => response = await next(),
            cancellationToken);

        return response!;
    }

    private static bool IsCommandRequest()
    {
        var requestType = typeof(TRequest);
        var requestNamespace = requestType.Namespace ?? string.Empty;
        return requestNamespace.Contains(".Commands.", StringComparison.Ordinal)
               || requestType.Name.EndsWith("Command", StringComparison.Ordinal);
    }
}
