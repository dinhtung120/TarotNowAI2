/*
 * ===================================================================
 * FILE: IAiRequestRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Hợp Đồng Lữu Trữ (Repository) dành riêng cho các Giao Dịch Chat AI.
 *   Xoay quanh việc Hỏi Hệ Thống Xem Lịch Sử Chat 1 Ngày, Giới Hạn Hạn Mức Tồn Đọng...
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Thỏa Thuận Lưu Trữ Vết Tích Giao Dịch Khi Xem Bói Bằng AI (Chat Log).
/// Bám sát vào Việc Giải Quyết Đồng Tiền (Refunds, Idempotent).
/// </summary>
public interface IAiRequestRepository
{
    /// <summary>Móc Dòng Giao Dịch Chứa Quá Khứ Lịch Sử Ra.</summary>
    Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>Ký Giấy Nộp Tiền Tạm Cất (Thêm Mới Giao Dịch).</summary>
    Task AddAsync(AiRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>Sửa Cập Nhật Hoàn/Trừ Biên Lai Hoàn Tất.</summary>
    Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>Đếm Số Dòng Lịch Sử (Quá tam Ba bận, xem trong ngày thằng này xài AI Lố Không).</summary>
    Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>Theo Dõi Còn Bao Nhiêu Lệnh Trả Lời Văng Vẳng Chưa Thoát Ra Ngoài Tắt Cửa Sổ.</summary>
    Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>Kiểm Tra Xem Khách Hàng Này Được Tính Giá Ở Thang Chat Phụ Nào Cho Phòng Hỏi Bói Cụ Thể Nào Đó (Slot Free Hay Tốn Tiền Thất Ngôn).</summary>
    Task<int> GetFollowupCountBySessionAsync(string sessionId, CancellationToken cancellationToken = default);
}
