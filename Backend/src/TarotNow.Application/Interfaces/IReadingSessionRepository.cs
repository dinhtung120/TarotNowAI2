/*
 * ===================================================================
 * FILE: IReadingSessionRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Với Kho Xoáy Chứa Các Giao Dịch Chơi Bốc Bài Trực Tiếp Bằng Trí Khôn Của Máy AI.
 *   Là Bệ Phóng Tổ Chức Sessions - Phiên Đọc Rút Bao Nhiêu Quẻ, Lá Nào Gì, Thanh Toán Atomically Ra Sao Đều Nắm Ở Thư Ký Này.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Đại Sảnh Lưu Trữ Trạng Thái Đang Xay Dòng Thời Gian Xóc Bộ Bài Rút Sẵn Và Giữ Trữ Lịch Sử Gọi Máy Thông Suốt.
/// </summary>
public interface IReadingSessionRepository
{
    /// <summary>Đập Bàn Gọi Thầy Bắt Đầu Quăng Bộ Bài Tạo Session Bàn Trống (Mở Phiên).</summary>
    Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    
    /// <summary>Liếc Trộm Mật Mã Căn Phòng Xem Bộ Bài Đang Đóng Hay Đang Rút Cấp Nào Trên Bàn Của Session Rõ Ràng.</summary>
    Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    /// <summary>Ghi Thêm Thông Tin Nếu Lá Bài Được Mở Chồng Rút Đã Xong Xuôi Tắt Máy Đi Lại Kết Quả.</summary>
    Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    
    /// <summary>Theo Dõi Giới Hạn Phúc Lợi Daily Lệnh Không Tốn Tiền Không Bốc Được Nữa Quá Nữa Đêm Khung Mới Mở Reset Lại Cầu Thủ Free (Quản Lý Daily Rút Miễn Phí).</summary>
    Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);

    /// <summary>Người Khách Mở Hồ Sơ Đọc Tưởng Thấy Mảnh Tình Xưa Hiện Về Xem Lại Lá Nào Ngàn Năm Trứ Cũ Nương (History Tụ Của Mình).</summary>
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>Mở Tung Choác Nét Chỉ Tiết Gương Mặt Con Session Kèm Theo Văng Toàn Bộ Đoạn Văn AI Tâm Sự Chung Của 1 Căn Sổ Chấn Động Thật Bự Lên Web Khách Xem Nội Lực Đi Xem Bói.</summary>
    Task<(ReadingSession ReadingSession, IEnumerable<TarotNow.Domain.Entities.AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thẩm Quyền Chúa Trời (Admin) Chiếu Soi Lọc Cửa Tìm Lịch Sử Tràn Của Người Dùng Lọc Cất Theo Thời Gian Lá Đặc Dị Các Phòng Tự Tạo Kiểm Tra Rác Lậu.
    /// </summary>
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page, 
        int pageSize, 
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}
