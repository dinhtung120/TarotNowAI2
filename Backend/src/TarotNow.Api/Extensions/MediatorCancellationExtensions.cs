using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Extensions;

public static class MediatorCancellationExtensions
{
    public static Task<TResponse> SendWithRequestCancellation<TResponse>(
        this IMediator mediator,
        HttpContext? httpContext,
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var effectiveToken = ResolveEffectiveToken(httpContext?.RequestAborted ?? CancellationToken.None, cancellationToken);
        return mediator.Send(request, effectiveToken);
    }

    public static Task SendWithRequestCancellation(
        this IMediator mediator,
        HttpContext? httpContext,
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        var effectiveToken = ResolveEffectiveToken(httpContext?.RequestAborted ?? CancellationToken.None, cancellationToken);
        return mediator.Send(request, effectiveToken);
    }

    public static Task<TResponse> SendWithConnectionCancellation<TResponse>(
        this IMediator mediator,
        HubCallerContext context,
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var effectiveToken = ResolveEffectiveToken(context.ConnectionAborted, cancellationToken);
        return mediator.Send(request, effectiveToken);
    }

    public static Task SendWithConnectionCancellation(
        this IMediator mediator,
        HubCallerContext context,
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        var effectiveToken = ResolveEffectiveToken(context.ConnectionAborted, cancellationToken);
        return mediator.Send(request, effectiveToken);
    }

    private static CancellationToken ResolveEffectiveToken(
        CancellationToken fallbackToken,
        CancellationToken explicitToken)
    {
        return explicitToken.CanBeCanceled ? explicitToken : fallbackToken;
    }
}
