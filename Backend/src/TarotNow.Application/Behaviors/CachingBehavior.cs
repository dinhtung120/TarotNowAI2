using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Behaviors;

// Pipeline behavior thêm lớp cache cho request có khai báo khả năng lưu đệm.
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo behavior cache cho MediatR pipeline.
    /// Luồng xử lý: nhận ICacheService để đọc/ghi cache theo key của request.
    /// </summary>
    public CachingBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Xử lý request theo chiến lược cache-aside khi request hỗ trợ cache.
    /// Luồng xử lý: bỏ qua cache nếu không hỗ trợ, thử đọc cache, fallback xử lý thật rồi ghi cache.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableRequest<TResponse> cacheableRequest || cacheableRequest.BypassCache)
        {
            // Nhánh bypass: request không cacheable hoặc chủ động tắt cache thì đi thẳng handler gốc.
            return await next();
        }

        var cacheHit = await _cacheService.GetAsync<TResponse>(cacheableRequest.CacheKey, cancellationToken);
        if (cacheHit is not null)
        {
            // Cache hit: trả dữ liệu ngay để giảm tải DB/service downstream.
            return cacheHit;
        }

        // Cache miss: xử lý nghiệp vụ thật rồi ghi lại cache cho lần gọi kế tiếp.
        var response = await next();
        await _cacheService.SetAsync(cacheableRequest.CacheKey, response, cacheableRequest.Expiration, cancellationToken);
        return response;
    }
}
