

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Queries.GetModerationQueue;

// Query lấy hàng đợi moderation report của community post.
public class GetModerationQueueQuery : IRequest<(IEnumerable<ReportDto> Items, long TotalCount)>
{
    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;

    // Bộ lọc trạng thái report (tùy chọn).
    public string? StatusFilter { get; set; }
}

// Handler truy vấn moderation queue.
public class GetModerationQueueQueryHandler : IRequestHandler<GetModerationQueueQuery, (IEnumerable<ReportDto> Items, long TotalCount)>
{
    private readonly IReportRepository _reportRepo;

    /// <summary>
    /// Khởi tạo handler moderation queue.
    /// Luồng xử lý: nhận report repository để truy vấn hàng đợi moderation theo phân trang.
    /// </summary>
    public GetModerationQueueQueryHandler(IReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    /// <summary>
    /// Xử lý query lấy moderation queue.
    /// Luồng xử lý: chuẩn hóa page/pageSize, rồi truy vấn report theo status filter với targetType cố định là post.
    /// </summary>
    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> Handle(GetModerationQueueQuery request, CancellationToken cancellationToken)
    {
        // Chuẩn hóa paging để tránh giá trị âm hoặc quá lớn.
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);

        return await _reportRepo.GetPaginatedAsync(
            page,
            pageSize,
            request.StatusFilter,
            targetType: "post",
            cancellationToken: cancellationToken);
    }
}
