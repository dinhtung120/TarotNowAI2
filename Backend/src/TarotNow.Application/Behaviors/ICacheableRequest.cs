using MediatR;

namespace TarotNow.Application.Behaviors;

public interface ICacheableRequest<out TResponse> : IRequest<TResponse>
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
    bool BypassCache { get; }
}

