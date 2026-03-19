/*
 * ===================================================================
 * FILE: ListReaderRequestsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListReaderRequests
 * ===================================================================
 * MỤC ĐÍCH:
 *   Query + Response DTO cho Admin XEM DANH SÁCH ĐƠN XIN LÀM READER.
 *   Admin dùng trang "Approval Queue" (hàng đợi duyệt) để:
 *   - Xem các đơn đang chờ (Pending)
 *   - Phê duyệt hoặc từ chối (ApproveReaderCommand)
 *   - Xem lịch sử đã duyệt/từ chối
 *
 * PHÂN TRANG + LỌC:
 *   Page/PageSize: phân trang tiêu chuẩn
 *   StatusFilter: lọc theo trạng thái (pending/approved/rejected)
 *   Nếu StatusFilter = null → lấy tất cả trạng thái
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common; // ReaderRequestDto

namespace TarotNow.Application.Features.Admin.Queries.ListReaderRequests;

/// <summary>
/// Query phân trang danh sách đơn xin Reader — dùng cho Admin approval queue.
/// </summary>
public class ListReaderRequestsQuery : IRequest<ListReaderRequestsResult>
{
    /// <summary>Trang hiện tại (1-indexed). Mặc định trang 1.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Số item mỗi trang. Mặc định 20.</summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Lọc theo trạng thái (nullable = lấy tất cả):
    ///   "pending" → chỉ hiện đơn chờ duyệt
    ///   "approved" → chỉ hiện đơn đã duyệt
    ///   "rejected" → chỉ hiện đơn bị từ chối
    /// </summary>
    public string? StatusFilter { get; set; }
}

/// <summary>
/// Kết quả phân trang.
/// 
/// Enumerable.Empty<T>(): tạo collection rỗng (0 item).
/// Khác với null → JavaScript vẫn nhận được mảng rỗng [],
/// thay vì null (dễ gây lỗi "Cannot read property of null").
/// </summary>
public class ListReaderRequestsResult
{
    /// <summary>Danh sách đơn xin reader (đã phân trang).</summary>
    public IEnumerable<ReaderRequestDto> Requests { get; set; } = Enumerable.Empty<ReaderRequestDto>();

    /// <summary>
    /// Tổng số đơn (long thay vì int vì MongoDB count trả long).
    /// Dùng cho pagination UI tính tổng số trang.
    /// </summary>
    public long TotalCount { get; set; }
}
