using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cacheService;

    public CachingBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableRequest<TResponse> cacheableRequest || cacheableRequest.BypassCache)
        {
            return await next();
        }

        var cacheHit = await _cacheService.GetAsync<TResponse>(cacheableRequest.CacheKey, cancellationToken);
        if (cacheHit is not null)
        {
            return cacheHit;
        }

        var response = await next();
        await _cacheService.SetAsync(cacheableRequest.CacheKey, response, cacheableRequest.Expiration, cancellationToken);
        return response;
    }
}

