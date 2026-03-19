/*
 * FILE: AiRequestRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng ai_requests (PostgreSQL).
 *   Ghi nhận mỗi lần hệ thống gọi AI: ai dùng, tốn bao nhiêu, trạng thái gì.
 *
 *   CÁC CHỨC NĂNG CHÍNH:
 *   → CRUD cơ bản: GetById, Add, Update
 *   → GetDailyAiRequestCountAsync: đếm số lần gọi AI hôm nay (giới hạn quota/ngày)
 *   → GetActiveAiRequestCountAsync: đếm request đang pending (chặn gọi trùng)
 *   → GetFollowupCountBySessionAsync: đếm câu follow-up trong 1 phiên (giới hạn 5 câu)
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IAiRequestRepository — truy cập bảng ai_requests (PostgreSQL).
/// </summary>
public class AiRequestRepository : IAiRequestRepository
{
    private readonly ApplicationDbContext _context;

    public AiRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>Tìm AiRequest theo ID. Trả null nếu không tồn tại.</summary>
    public async Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>Thêm mới AiRequest vào DB và lưu ngay (SaveChanges).</summary>
    public async Task AddAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        await _context.AiRequests.AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Cập nhật AiRequest (ví dụ: đổi status từ Requested → Completed).</summary>
    public async Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        _context.AiRequests.Update(request);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Đếm số lần gọi AI thành công TRONG NGÀY HÔM NAY (UTC) của 1 User.
    /// Dùng để kiểm tra: User đã dùng hết quota hàng ngày chưa?
    /// Chỉ đếm status = Completed hoặc FailedAfterFirstToken (đã consume token thật).
    /// Không đếm status = Requested (đang chờ) hoặc Failed (lỗi trước khi consume).
    /// </summary>
    public async Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Lấy đầu ngày UTC hôm nay (00:00:00)
        var todayOffset = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero);
        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId && 
                 x.CreatedAt >= todayOffset && 
                 (x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed
                     || x.Status == TarotNow.Domain.Enums.AiRequestStatus.FailedAfterFirstToken), 
            cancellationToken);
    }

    /// <summary>
    /// Đếm số request đang ở trạng thái "Requested" (chưa xử lý xong) của 1 User.
    /// Dùng để chặn: User không được gửi request mới khi còn request cũ đang pending.
    /// Tránh: User spam bấm nút "Bói bài" liên tục → tạo hàng loạt request tốn tiền.
    /// </summary>
    public async Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId && 
                 x.Status == TarotNow.Domain.Enums.AiRequestStatus.Requested, 
            cancellationToken);
    }

    /// <summary>
    /// Đếm số câu hỏi follow-up đã hoàn thành trong 1 phiên đọc bài (reading session).
    /// Cách tính: tổng request Completed cho session này - 1 (trừ request chính ban đầu).
    /// Ví dụ: 1 request gốc + 3 follow-up = totalRequests = 4 → followup count = 3.
    /// Dùng để kiểm tra: User đã hỏi hết 5 câu follow-up cho phiên này chưa?
    /// </summary>
    public async Task<int> GetFollowupCountBySessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var totalRequests = await _context.AiRequests.CountAsync(
            x => x.ReadingSessionRef == sessionId && 
                 x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed, 
            cancellationToken);

        // Trừ 1 vì request đầu tiên là request gốc (không phải follow-up)
        // Math.Max(0, ...) đảm bảo không trả về số âm nếu chưa có request nào
        return Math.Max(0, totalRequests - 1);
    }
}
