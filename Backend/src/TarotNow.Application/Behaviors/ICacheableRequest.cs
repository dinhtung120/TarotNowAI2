using MediatR;

namespace TarotNow.Application.Behaviors;

// Marker interface khai báo request hỗ trợ cache trong MediatR pipeline.
public interface ICacheableRequest<out TResponse> : IRequest<TResponse>
{
    // Khóa cache duy nhất cho request hiện tại.
    string CacheKey { get; }
    // Thời gian sống cache tùy chọn; null nghĩa là dùng mặc định của cache service.
    TimeSpan? Expiration { get; }
    // Cờ cho phép bỏ qua cache khi cần dữ liệu mới nhất.
    bool BypassCache { get; }
}
