/*
 * ===================================================================
 * FILE: GetAllReadingsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.History.Queries.GetAllReadings
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh chuyên biệt dành cho Ban Quản Trị (Admin) hoặc Hệ Thống thống kê.
 *   API này cho phép cào (Fetch) toàn bộ Lịch Sử Khách Hàng xem bói trên Platform.
 *
 * SỨC MẠNH:
 *   1. Cơ chế Lọc (Filter) mạnh mẽ: Tìm theo tên User, Loại Trải bài, Thời gian.
 *   2. Khắc phục Điểm yếu của MongoDB (NoSQL không Join được bảng SQL):
 *      -> Handler này dùng kĩ thuật Batching Query (gom lô) để bắn SQL lấy 
 *      Username của hàng chục khách hàng cùng lúc, sau đó đắp (Mapping) 
 *      ngược vào dữ liệu bốc từ MongoDB. Rất tối ưu!
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

/// <summary>
/// Gói Lệnh: Đòi hõi Mảng Lịch Sử (Danh Sách Các Lần Bói Bài).
/// </summary>
public class GetAllReadingsQuery : IRequest<GetAllReadingsResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    // ==========================================
    // BỘ LỌC (FILTERS) CHO MÀN HÌNH DASHBOARD
    // ==========================================
    
    /// <summary>Muốn tìm lịch sử của riêng MỘT Khách hàng nào đó?</summary>
    public string? Username { get; set; }
    
    /// <summary>Muốn lọc xem dạo này dân tình hay coi Trải bài Loại Gì? (Tình yêu, Sự nghiệp).</summary>
    public string? SpreadType { get; set; }
    
    /// <summary>Lọc Từ Ngày...</summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>Lọc Đến Ngày...</summary>
    public DateTime? EndDate { get; set; }
}

public class GetAllReadingsResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<AdminReadingDto> Items { get; set; } = new List<AdminReadingDto>();
}

/// <summary>
/// Thẻ Dữ Liệu Tóm tắt của 1 Quẻ Bói (Đem lên màn hình cho Quản Trị Viên xem).
/// Thay vì chỉ ném ID thô kệch, thẻ này đã được "Tiêm" Username vào cho thân thiện.
/// </summary>
public class AdminReadingDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>Được đắp thêm bằng tay nhờ truy vấn SQL phụ trợ.</summary>
    public string Username { get; set; } = string.Empty; 
    
    public string SpreadType { get; set; } = string.Empty;
    public string? Question { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, GetAllReadingsResponse>
{
    private readonly IReadingSessionRepository _readingRepo;
    
    // Cần gọi SQL để truy vấn thông tin User (Vì MongoDB chỉ lưu mã UserId).
    private readonly IUserRepository _userRepo; 

    public GetAllReadingsQueryHandler(IReadingSessionRepository readingRepo, IUserRepository userRepo)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
    }

    public async Task<GetAllReadingsResponse> Handle(GetAllReadingsQuery request, CancellationToken cancellationToken)
    {
        var filteredUserIds = await ResolveUserFilterAsync(request.Username, cancellationToken);
        if (filteredUserIds is { Count: 0 })
        {
            return BuildEmptyResponse(request);
        }

        var (items, totalCount) = await LoadReadingsAsync(request, filteredUserIds, cancellationToken);
        var dtos = await BuildDtosAsync(items, cancellationToken);
        return BuildResponse(request, totalCount, dtos);
    }
}
