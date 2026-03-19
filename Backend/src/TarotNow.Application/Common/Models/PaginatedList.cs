/*
 * ===================================================================
 * FILE: PaginatedList.cs
 * NAMESPACE: TarotNow.Application.Common.Models
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Model GENERIC (tái sử dụng được) để đóng gói dữ liệu PHÂN TRANG.
 *
 * PHÂN TRANG LÀ GÌ?
 *   Thay vì trả về 10.000 kết quả cùng lúc (chậm, tốn bộ nhớ),
 *   chia thành nhiều trang: trang 1 = 20 kết quả, trang 2 = 20 kết quả,...
 *   
 *   Giống Google: tìm kiếm cho 1 triệu kết quả nhưng chỉ hiện 10/trang.
 *
 * GENERIC TYPE <T>:
 *   T là kiểu dữ liệu của mỗi item trong danh sách.
 *   Ví dụ:
 *   - PaginatedList<UserDto>: danh sách user phân trang
 *   - PaginatedList<ReadingSessionDto>: danh sách phiên đọc bài phân trang
 *   - PaginatedList<ChatMessageDto>: danh sách tin nhắn phân trang
 *   Một class dùng cho TẤT CẢ → không cần viết riêng cho từng loại.
 *
 * JSON TRẢ VỀ CHO CLIENT:
 *   {
 *     "items": [...],           ← Danh sách dữ liệu trang hiện tại
 *     "pageIndex": 1,           ← Trang hiện tại (1-indexed)
 *     "totalPages": 5,          ← Tổng số trang
 *     "totalCount": 100,        ← Tổng số bản ghi
 *     "hasPreviousPage": false,  ← Có trang trước không
 *     "hasNextPage": true        ← Có trang sau không
 *   }
 * ===================================================================
 */

namespace TarotNow.Application.Common.Models;

/// <summary>
/// Model chung để đóng gói dữ liệu phân trang trả về cho API.
/// </summary>
public class PaginatedList<T>
{
    /// <summary>
    /// Danh sách items trong trang hiện tại.
    /// "get;" (không có set): chỉ đọc (read-only) sau khi tạo.
    /// Đảm bảo immutability (không thể thay đổi từ bên ngoài).
    /// </summary>
    public List<T> Items { get; }

    /// <summary>Số thứ tự trang hiện tại (1-indexed: bắt đầu từ 1).</summary>
    public int PageIndex { get; }

    /// <summary>Tổng số trang (tính từ totalCount và pageSize).</summary>
    public int TotalPages { get; }

    /// <summary>Tổng số bản ghi trong toàn bộ dataset (không chỉ trang này).</summary>
    public int TotalCount { get; }

    /// <summary>
    /// Constructor: tạo PaginatedList từ dữ liệu thô.
    ///
    /// Tham số:
    ///   items: danh sách items cho trang hiện tại (đã slice/skip xong)
    ///   count: TỔNG SỐ bản ghi trong database (trước khi phân trang)
    ///   pageIndex: trang hiện tại (1-indexed)
    ///   pageSize: số item mỗi trang
    /// </summary>
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;

        /*
         * Tính tổng số trang:
         * Math.Ceiling: làm tròn LÊN.
         * count / pageSize: chia tổng số cho kích thước trang.
         * 
         * Ví dụ: count=25, pageSize=10
         *   25 / 10.0 = 2.5
         *   Ceiling(2.5) = 3 trang (trang 3 chỉ có 5 items)
         * 
         * (double)pageSize: ép sang double để phép chia trả số thực (2.5)
         * thay vì số nguyên (2) — nếu chia 2 int: 25/10 = 2 (sai).
         */
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        TotalCount = count;
        Items = items;
    }

    /// <summary>
    /// CÓ TRANG TRƯỚC không? (để hiện nút "Previous" trên UI)
    /// True nếu đang ở trang 2 trở lên.
    /// 
    /// "=>" (expression body): cú pháp ngắn gọn cho property chỉ đọc.
    /// Tương đương: get { return PageIndex > 1; }
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// CÓ TRANG SAU không? (để hiện nút "Next" trên UI)
    /// True nếu chưa phải trang cuối.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;
}
