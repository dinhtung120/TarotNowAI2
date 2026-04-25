

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository thao tác dữ liệu AiRequest trên PostgreSQL.
public class AiRequestRepository : IAiRequestRepository
{
    // DbContext dùng để query/ghi trạng thái request AI.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository với DbContext từ DI.
    /// Luồng xử lý: giữ tham chiếu context để mọi thao tác tuân theo transaction scope hiện tại.
    /// </summary>
    public AiRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy AiRequest theo định danh.
    /// Luồng xử lý: truy vấn bản ghi đầu tiên khớp id, trả null nếu không tồn tại.
    /// </summary>
    public async Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Thêm mới request AI.
    /// Luồng xử lý: add entity vào DbSet rồi persist ngay để phát sinh id và timestamp.
    /// </summary>
    public async Task AddAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        await _context.AiRequests.AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        // Save ngay để các bước downstream có thể dùng trạng thái request vừa tạo.
    }

    /// <summary>
    /// Cập nhật trạng thái hoặc metadata của request AI.
    /// Luồng xử lý: đánh dấu entity modified và lưu thay đổi vào DB.
    /// </summary>
    public async Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        _context.AiRequests.Update(request);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Đếm số request AI đã kết thúc trong ngày UTC của một user.
    /// Luồng xử lý: lấy mốc đầu ngày UTC và chỉ tính các status đã hoàn tất để phục vụ hạn mức ngày.
    /// </summary>
    public async Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var todayOffset = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero);
        // Dùng UTC day boundary để nhất quán giữa các môi trường chạy khác múi giờ.

        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId &&
                 x.CreatedAt >= todayOffset &&
                 (x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed
                     || x.Status == TarotNow.Domain.Enums.AiRequestStatus.FailedAfterFirstToken),
            cancellationToken);
        // Không tính trạng thái Requested để tránh khóa quota do request còn treo/chưa kết thúc.
    }

    /// <summary>
    /// Đếm số request AI đang hoạt động của user.
    /// Luồng xử lý: chỉ đếm status Requested để giới hạn số phiên stream đồng thời.
    /// </summary>
    public async Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId &&
                 x.Status == TarotNow.Domain.Enums.AiRequestStatus.Requested,
            cancellationToken);
    }

    /// <summary>
    /// Đếm số follow-up đã hoàn tất trong một reading session.
    /// Luồng xử lý: đếm tổng request completed theo session rồi trừ request đầu tiên để ra số follow-up thực.
    /// </summary>
    public async Task<int> GetFollowupCountBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var totalRequests = await _context.AiRequests.CountAsync(
            x => x.ReadingSessionRef == sessionId &&
                 x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed,
            cancellationToken);

        // Edge case: nếu chưa có request completed thì trả 0 thay vì giá trị âm.
        return Math.Max(0, totalRequests - 1);
    }
}
