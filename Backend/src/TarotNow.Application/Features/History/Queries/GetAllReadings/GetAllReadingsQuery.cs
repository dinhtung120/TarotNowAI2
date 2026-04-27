using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

// Query admin lấy danh sách tất cả reading sessions theo bộ lọc.
public class GetAllReadingsQuery : IRequest<GetAllReadingsResponse>
{
    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 10;

    // Bộ lọc theo username (tùy chọn).
    public string? Username { get; set; }

    // Bộ lọc theo loại spread (tùy chọn).
    public string? SpreadType { get; set; }

    // Bộ lọc ngày bắt đầu (tùy chọn).
    public DateTime? StartDate { get; set; }

    // Bộ lọc ngày kết thúc (tùy chọn).
    public DateTime? EndDate { get; set; }
}

// DTO kết quả danh sách reading cho admin.
public class GetAllReadingsResponse
{
    // Trang hiện tại.
    public int Page { get; set; }

    // Kích thước trang.
    public int PageSize { get; set; }

    // Tổng số trang.
    public int TotalPages { get; set; }

    // Tổng số bản ghi.
    public int TotalCount { get; set; }

    // Danh sách reading item.
    public IEnumerable<AdminReadingDto> Items { get; set; } = new List<AdminReadingDto>();
}

// DTO một reading item cho màn hình admin.
public class AdminReadingDto
{
    // Định danh session.
    public string Id { get; set; } = string.Empty;

    // Định danh user.
    public string UserId { get; set; } = string.Empty;

    // Username hiển thị của user.
    public string Username { get; set; } = string.Empty;

    // Loại spread.
    public string SpreadType { get; set; } = string.Empty;

    // Câu hỏi đầu vào (nếu có).
    public string? Question { get; set; }

    // Cờ session đã hoàn thành hay chưa.
    public bool IsCompleted { get; set; }

    // Thời điểm tạo session.
    public DateTime CreatedAt { get; set; }
}

// Handler truy vấn all readings cho admin.
public partial class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, GetAllReadingsResponse>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Khởi tạo handler get all readings.
    /// Luồng xử lý: nhận reading repository và user repository để tải session và map username.
    /// </summary>
    public GetAllReadingsQueryHandler(IReadingSessionRepository readingRepo, IUserRepository userRepo)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Xử lý query lấy toàn bộ readings.
    /// Luồng xử lý: resolve user filter theo username, tải dữ liệu phân trang theo bộ lọc, map DTO và dựng response.
    /// </summary>
    public async Task<GetAllReadingsResponse> Handle(GetAllReadingsQuery request, CancellationToken cancellationToken)
    {
        var normalizedPage = request.Page < 1 ? 1 : request.Page;
        var normalizedPageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 200);
        var normalizedRequest = new GetAllReadingsQuery
        {
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            Username = request.Username,
            SpreadType = request.SpreadType,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var filteredUserIds = await ResolveUserFilterAsync(normalizedRequest.Username, cancellationToken);
        if (filteredUserIds is { Count: 0 })
        {
            // Có lọc username nhưng không tìm thấy user phù hợp thì trả kết quả rỗng ngay.
            return BuildEmptyResponse(normalizedRequest);
        }

        var (items, totalCount) = await LoadReadingsAsync(normalizedRequest, filteredUserIds, cancellationToken);
        var dtos = await BuildDtosAsync(items, cancellationToken);
        return BuildResponse(normalizedRequest, totalCount, dtos);
    }
}
