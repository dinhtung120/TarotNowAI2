

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract thao tác dữ liệu tài chính của luồng chat để đảm bảo tính nhất quán giữa phiên và câu hỏi.
public interface IChatFinanceRepository
{
    /// <summary>
    /// Lấy phiên tài chính theo conversationRef để tái sử dụng phiên đang hoạt động.
    /// Luồng xử lý: truy vấn theo mã cuộc hội thoại và trả null nếu chưa phát sinh phiên.
    /// </summary>
    Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default);

    /// <summary>
    /// Lấy nhiều phiên tài chính theo danh sách conversationRef để tối ưu truy vấn batch.
    /// Luồng xử lý: lọc theo tập khóa đầu vào và trả danh sách phiên tương ứng.
    /// </summary>
    Task<List<ChatFinanceSession>> GetSessionsByConversationRefsAsync(IEnumerable<string> conversationRefs, CancellationToken ct = default);

    /// <summary>
    /// Lấy phiên tài chính theo id để tiếp tục xử lý nghiệp vụ escrow.
    /// Luồng xử lý: tìm chính xác theo Guid và trả null khi không có bản ghi.
    /// </summary>
    Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Lấy phiên tài chính với lock/update intent nhằm tránh race condition khi cập nhật.
    /// Luồng xử lý: truy vấn theo id trong ngữ cảnh cập nhật và trả entity có thể chỉnh sửa.
    /// </summary>
    Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Tạo phiên tài chính mới cho cuộc hội thoại cần thanh toán.
    /// Luồng xử lý: nhận entity phiên đã hợp lệ và thêm vào kho dữ liệu.
    /// </summary>
    Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default);

    /// <summary>
    /// Cập nhật phiên tài chính hiện có để phản ánh trạng thái mới của quy trình.
    /// Luồng xử lý: ghi các thay đổi của entity session vào nguồn dữ liệu.
    /// </summary>
    Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default);

    /// <summary>
    /// Lấy một question item theo id để xử lý các bước xác nhận/thanh toán.
    /// Luồng xử lý: truy vấn theo Guid và trả null khi item không tồn tại.
    /// </summary>
    Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Lấy question item với lock/update intent để cập nhật an toàn trong giao dịch.
    /// Luồng xử lý: truy vấn item theo id trong ngữ cảnh cập nhật và trả entity tương ứng.
    /// </summary>
    Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Lấy question item theo idempotency key để chống tạo trùng yêu cầu thanh toán.
    /// Luồng xử lý: tra key duy nhất và trả item đã tồn tại nếu có.
    /// </summary>
    Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Lấy toàn bộ item thuộc một phiên tài chính để dựng timeline xử lý.
    /// Luồng xử lý: lọc theo sessionId và trả danh sách item liên quan.
    /// </summary>
    Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default);

    /// <summary>
    /// Thêm question item mới khi phát sinh yêu cầu đặt câu hỏi có thanh toán.
    /// Luồng xử lý: persist entity item mới vào kho dữ liệu.
    /// </summary>
    Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default);

    /// <summary>
    /// Cập nhật question item khi trạng thái thanh toán hoặc xử lý thay đổi.
    /// Luồng xử lý: ghi đè dữ liệu đã thay đổi của item tương ứng.
    /// </summary>
    Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default);

    /// <summary>
    /// Lấy các offer đã quá hạn để chạy luồng xử lý timeout tự động.
    /// Luồng xử lý: lọc item theo điều kiện quá hạn và trả danh sách cần xử lý.
    /// </summary>
    Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default);

    /// <summary>
    /// Lấy các item đủ điều kiện auto-refund để hoàn tiền đúng SLA.
    /// Luồng xử lý: truy vấn theo trạng thái và mốc thời gian hoàn tiền tự động.
    /// </summary>
    Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default);

    /// <summary>
    /// Lấy các item đủ điều kiện auto-release để giải ngân tự động.
    /// Luồng xử lý: lọc item theo rule release và trả danh sách cần thực thi.
    /// </summary>
    Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default);

    /// <summary>
    /// Lấy các item tranh chấp đã tới hạn tự động kết luận.
    /// Luồng xử lý: so sánh dueAtUtc với trạng thái tranh chấp và trả các item cần resolve.
    /// </summary>
    Task<List<ChatQuestionItem>> GetDisputedItemsForAutoResolveAsync(DateTime dueAtUtc, CancellationToken ct = default);

    /// <summary>
    /// Lấy danh sách item tranh chấp có phân trang để phục vụ màn hình quản trị.
    /// Luồng xử lý: truy vấn theo page/pageSize và trả tập item cùng tổng bản ghi.
    /// </summary>
    Task<(IReadOnlyList<ChatQuestionItem> Items, long TotalCount)> GetDisputedItemsPaginatedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default);

    /// <summary>
    /// Đếm số tranh chấp gần đây theo người nhận để đánh giá rủi ro vận hành.
    /// Luồng xử lý: lọc theo receiverId và fromUtc, sau đó trả về tổng số tranh chấp.
    /// </summary>
    Task<long> CountRecentDisputesByReceiverAsync(
        Guid receiverId,
        DateTime fromUtc,
        CancellationToken ct = default);

    /// <summary>
    /// Lưu toàn bộ thay đổi đang theo dõi trong unit of work hiện tại.
    /// Luồng xử lý: commit các thay đổi pending của session và item xuống dữ liệu bền vững.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
