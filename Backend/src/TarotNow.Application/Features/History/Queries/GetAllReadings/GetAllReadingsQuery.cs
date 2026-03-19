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

public class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, GetAllReadingsResponse>
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
        List<Guid>? filteredUserIds = null;

        // -----------------------------------------------------------------
        // BƯỚC 1: XỬ LÝ LỌC THEO TÊN (Cross-Database Search)
        // Nếu Admin cố tình gõ Tên để tìm, ta phải rẽ vào SQL Server tìm Tên trước, 
        // Lấy ra đóng Danh sách UUID của những người trùng tên đó.
        // -----------------------------------------------------------------
        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            var usernameLower = request.Username.Trim().ToLowerInvariant();
            var users = await _userRepo.SearchUsersByUsernameAsync(usernameLower, cancellationToken);
            filteredUserIds = users.Select(u => u.Id).ToList();
            
            // Nếu không có mống nào tên này -> Khỏi cần chui vào MongoDB mất công. Trả về mảng rỗng.
            if (filteredUserIds.Count == 0)
            {
                return new GetAllReadingsResponse { Page = request.Page, PageSize = request.PageSize, TotalCount = 0, Items = new List<AdminReadingDto>() };
            }
        }

        // -----------------------------------------------------------------
        // BƯỚC 2: CÀO DB MÔNG CỔ (MongoDB)
        // Nhồi đống UUID vừa tìm được (hoặc Null) vào DB NoSQL để lấy Lịch Sử.
        // -----------------------------------------------------------------
        var (items, totalCount) = await _readingRepo.GetAllSessionsAsync(
            request.Page, 
            request.PageSize, 
            filteredUserIds?.Select(id => id.ToString()).ToList(),
            request.SpreadType,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        // -----------------------------------------------------------------
        // BƯỚC 3: KẾT HỢP DỮ LIỆU ĐỈNH CAO (BATCH MAPPING - GIẢ LẬP JOIN)
        // Đây là cách tối ưu nhất để tránh lỗi N+1 Query.
        // Thay vì Vòng lặp For -> 100 Câu SQL tìm Tên.
        // Ta gom 100 ID lại -> Gọi 1 Câu SQL duy nhất tìm 100 Tên!
        // -----------------------------------------------------------------
        var parsedItems = items
            .Select(s => new
            {
                Session = s, // Lịch sử thô nhặt từ Mongo
                // An toàn: Ép kiểu Chuỗi (Mongo) thành Guid (SQL). Thất bại thì bỏ Null.
                ParsedUserId = Guid.TryParse(s.UserId, out var guid) ? guid : (Guid?)null
            })
            .ToList();

        // Rút trích Tập Vị Tự (Distinct) - Ví dụ 10 dòng đều của User A -> Chỉ lấy cái tên User A đi hỏi DB 1 lần.
        var userGuids = parsedItems
            .Where(x => x.ParsedUserId.HasValue)
            .Select(x => x.ParsedUserId!.Value)
            .Distinct()
            .ToList();
            
        // Gõ cửa SQL 1 lần duy nhất: Lấy về cuốn Từ Điển [ID: TÊN].
        var userMap = await _userRepo.GetUsernameMapAsync(userGuids, cancellationToken);

        // Đúc Frame (DTO) để bắn về Frontend. Ráp tên tương ứng theo mã ID.
        var dtos = parsedItems.Select(x => {
            var foundUsername = x.ParsedUserId.HasValue && userMap.TryGetValue(x.ParsedUserId.Value, out var uname)
                ? uname
                : "Unknown"; // Không có tên thì cho Vô danh.
            
            return new AdminReadingDto
            {
                Id = x.Session.Id,
                UserId = x.Session.UserId,
                Username = foundUsername, // <--- Đây là thành quả.
                SpreadType = x.Session.SpreadType,
                Question = x.Session.Question,
                IsCompleted = x.Session.IsCompleted,
                CreatedAt = x.Session.CreatedAt
            };
        });

        return new GetAllReadingsResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = dtos
        };
    }
}
