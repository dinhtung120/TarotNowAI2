/*
 * ===================================================================
 * FILE: IChatMessageRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Đổ Nội Dung Chat Giữa Reader x Khách Lên Giao Diện Máy Chủ Nhận Nạp Tin MongoDB.
 *   Xử Lý Vụ Phân Trang Kéo Lên Trượt Đọc Tin Cũ Kèm Đánh Dấu Cảnh Chữ Đã Đọc.
 * ===================================================================
 */

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Cái Ống Cuộn Chứa Cuộc Trò Chuyện Lịch Sử Kín Đáo (Chat MongoDb).
/// </summary>
public interface IChatMessageRepository
{
    /// <summary>Vứt 1 Dòng Chữ Vào Đám Đông (Insert Tin Mới).</summary>
    Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu Thuật Mồi Câu "Infinity Scroll": Khách Buốt Trượt Lên Xem Những Tin Bắn Cũ Trứ Dữ Dội Không Thể Dồn Đủ Nhẹ Lần 1 Được,
    /// Cắt Từng Đoạn Chạy Ra (Paginate Desc). Lấy Thể Thức Mới Cổ Điển Nhất (Mới Nhất Mãi).
    /// </summary>
    Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Con Dấu Tẩy "Đã Quỷ Ám 1 Chữ Rùi": Sập Lệ Quyền Cả Cuộn Unread Chớp Nháy Cứ Cuộn Tới Chỗ Nào Lì Xóa Kêu Đi.
    /// Trả Lại Tổng Thiệt Hại Bao Nhiêu Chữ Vừa Mất Nhãn Chưa Đọc Bảng Đã Fix Trở Lại Đếm Trừ Trừ Bỏ Trống Nhỏ Giọt (Red Dot Bubble UI).
    /// </summary>
    Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default);
}
