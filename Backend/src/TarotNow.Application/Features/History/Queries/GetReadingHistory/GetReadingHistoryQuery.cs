using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingHistory;

// Query lấy lịch sử reading sessions của user.
public class GetReadingHistoryQuery : IRequest<GetReadingHistoryResponse>
{
    // Định danh user cần lấy lịch sử.
    public Guid UserId { get; set; }

    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 10;

    // Loại trải bài cần lọc (tùy chọn).
    public string? SpreadType { get; set; }

    // Ngày trải bài cần lọc (tùy chọn).
    public DateTime? Date { get; set; }
}

// DTO kết quả lịch sử reading theo phân trang.
public class GetReadingHistoryResponse
{
    // Trang hiện tại.
    public int Page { get; set; }

    // Kích thước trang.
    public int PageSize { get; set; }

    // Tổng số trang.
    public int TotalPages { get; set; }

    // Tổng số bản ghi.
    public int TotalCount { get; set; }

    // Danh sách reading session DTO.
    public IEnumerable<ReadingSessionDto> Items { get; set; } = new List<ReadingSessionDto>();
}

// DTO một reading session trong lịch sử.
public class ReadingSessionDto
{
    // Định danh session.
    public string Id { get; set; } = string.Empty;

    // Loại spread đã dùng.
    public string SpreadType { get; set; } = string.Empty;

    // Cờ session đã hoàn thành hay chưa.
    public bool IsCompleted { get; set; }

    // Thời điểm tạo session.
    public DateTime CreatedAt { get; set; }
}

// Handler truy vấn lịch sử reading của user.
public class GetReadingHistoryQueryHandler : IRequestHandler<GetReadingHistoryQuery, GetReadingHistoryResponse>
{
    private readonly IReadingSessionRepository _readingRepo;

    /// <summary>
    /// Khởi tạo handler get reading history.
    /// Luồng xử lý: nhận reading repository để truy vấn lịch sử theo user.
    /// </summary>
    public GetReadingHistoryQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    /// <summary>
    /// Xử lý query lấy lịch sử reading.
    /// Luồng xử lý: tải sessions theo user + phân trang, map DTO và trả metadata phân trang.
    /// </summary>
    public async Task<GetReadingHistoryResponse> Handle(GetReadingHistoryQuery request, CancellationToken cancellationToken)
    {
        var normalizedPage = request.Page < 1 ? 1 : request.Page;
        var normalizedPageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 100);

        var (items, totalCount) = await _readingRepo.GetSessionsByUserIdAsync(
            request.UserId,
            normalizedPage,
            normalizedPageSize,
            request.SpreadType,
            request.Date,
            cancellationToken);

        var dtos = items.Select(s => new ReadingSessionDto
        {
            Id = s.Id,
            SpreadType = s.SpreadType,
            IsCompleted = s.IsCompleted,
            CreatedAt = s.CreatedAt
        });

        return new GetReadingHistoryResponse
        {
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            TotalCount = totalCount,
            // Tính số trang theo tổng bản ghi và page size.
            TotalPages = (int)Math.Ceiling(totalCount / (double)normalizedPageSize),
            Items = dtos
        };
    }
}
