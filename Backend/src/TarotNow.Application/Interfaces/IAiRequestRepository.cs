

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý vòng đời yêu cầu AI để kiểm soát tải và giới hạn theo người dùng.
public interface IAiRequestRepository
{
    /// <summary>
    /// Lấy yêu cầu AI theo định danh để tiếp tục xử lý hoặc đối soát trạng thái.
    /// Luồng xử lý: truy vấn theo id và trả về null nếu không tìm thấy bản ghi.
    /// </summary>
    Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy yêu cầu AI theo idempotency key để chống tạo request trùng.
    /// Luồng xử lý: truy vấn theo key đã chuẩn hóa và trả null nếu chưa tồn tại.
    /// </summary>
    Task<AiRequest?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo yêu cầu AI mới trước khi bắt đầu luồng sinh nội dung.
    /// Luồng xử lý: nhận entity đã chuẩn hóa và persist vào kho dữ liệu.
    /// </summary>
    Task AddAsync(AiRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật yêu cầu AI hiện có để phản ánh trạng thái mới của quy trình.
    /// Luồng xử lý: ghi đè các trường đã thay đổi của entity tương ứng.
    /// </summary>
    Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số yêu cầu AI trong ngày của người dùng để áp dụng hạn mức theo ngày.
    /// Luồng xử lý: lọc theo userId và mốc ngày hiện hành, trả về tổng số request.
    /// </summary>
    Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số yêu cầu AI đang active để tránh chạy song song vượt ngưỡng cho phép.
    /// Luồng xử lý: lọc theo userId và trạng thái đang xử lý, trả về số lượng hiện tại.
    /// </summary>
    Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số lượt follow-up trong cùng phiên để giới hạn độ dài hội thoại.
    /// Luồng xử lý: truy vấn theo sessionId và trả về số request đã phát sinh.
    /// </summary>
    Task<int> GetFollowupCountBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
