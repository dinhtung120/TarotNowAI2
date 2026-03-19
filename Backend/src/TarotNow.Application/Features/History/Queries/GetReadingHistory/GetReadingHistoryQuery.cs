/*
 * ===================================================================
 * FILE: GetReadingHistoryQuery.cs
 * NAMESPACE: TarotNow.Application.Features.History.Queries.GetReadingHistory
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh dùng riêng cho Trang Cá Nhân (Profile) của User.
 *   API này cho phép User tự kéo danh sách Tất cả những bộ bài bốc tay (Readings) 
 *   mà họ đã mua/chơi từ lúc tham gia App tới giờ.
 *
 * NOTE:
 *   Đây là bản thu gọn của Admin (Chỉ lấy của Chính Mình), có Phân trang rõ ràng.
 * ===================================================================
 */

using MediatR;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingHistory;

/// <summary>
/// Gói Lệnh: Đòi hõi Lịch sử Bốc Bài của CHÍNH MÌNH.
/// </summary>
public class GetReadingHistoryQuery : IRequest<GetReadingHistoryResponse>
{
    public Guid UserId { get; set; }
    
    // Cuộn vút trang Load More (Phân trang mượt)
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Phơi dữ liệu lịch sử đóng gói trả về Mobile/Web Client.
/// </summary>
public class GetReadingHistoryResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    
    // Tự tính tổng số trang để Frontend biết bao giờ thì Lấp Đáy (Hết Dữ liệu).
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    
    // Danh sách Gọn Gàng
    public IEnumerable<ReadingSessionDto> Items { get; set; } = new List<ReadingSessionDto>();
}

/// <summary>
/// DTO Siêu Tối Giản: 
/// Chỉ trả về những thứ người dùng quan tâm ở Màn Hình Danh Sách (Danh sách nhỏ).
/// Không nêm nếm Trọn Bộ Lịch sử Chat AI vào đây vì sẽ gây Nặng App.
/// </summary>
public class ReadingSessionDto
{
    public string Id { get; set; } = string.Empty;
    public string SpreadType { get; set; } = string.Empty; // Bói tình yêu (Celtic) hay Sự nghiệp (3 Cards)?
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetReadingHistoryQueryHandler : IRequestHandler<GetReadingHistoryQuery, GetReadingHistoryResponse>
{
    private readonly IReadingSessionRepository _readingRepo;

    public GetReadingHistoryQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    public async Task<GetReadingHistoryResponse> Handle(GetReadingHistoryQuery request, CancellationToken cancellationToken)
    {
        // 1. Phóng thẳng xuống MongoDB quét Lịch Sử Cá Nhân (Đã Sort DESC từ Repo)
        var (items, totalCount) = await _readingRepo.GetSessionsByUserIdAsync(request.UserId, request.Page, request.PageSize, cancellationToken);

        // 2. Gọt đẽo DTO loại bỏ mỡ thừa. (Chỉ lấy ID, LoạiTrảiBài, TìnhTrạng, NgàyGiờ)
        var dtos = items.Select(s => new ReadingSessionDto
        {
            Id = s.Id,
            SpreadType = s.SpreadType,
            IsCompleted = s.IsCompleted,
            CreatedAt = s.CreatedAt
        });

        return new GetReadingHistoryResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize), // Toán học: Làm tròn Lên (11 bài / 10 = 1.1 -> 2 Trang)
            Items = dtos
        };
    }
}
