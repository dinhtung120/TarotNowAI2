namespace TarotNow.Application.Common.Models;

// Wrapper kết quả phân trang chuẩn cho các truy vấn danh sách.
public class PaginatedList<T>
{
    // Danh sách item của trang hiện tại.
    public List<T> Items { get; }

    // Chỉ số trang hiện tại (1-based).
    public int PageIndex { get; }

    // Tổng số trang có thể có theo tổng bản ghi và page size.
    public int TotalPages { get; }

    // Tổng số bản ghi thỏa điều kiện truy vấn.
    public int TotalCount { get; }

    /// <summary>
    /// Khởi tạo kết quả phân trang từ dữ liệu đã truy vấn.
    /// Luồng xử lý: lưu page index, tính tổng trang bằng ceiling, rồi gán tổng bản ghi và danh sách item.
    /// </summary>
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;

        // Dùng ceiling để không mất trang lẻ khi tổng bản ghi không chia hết page size.
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        TotalCount = count;
        Items = items;
    }

    // Cờ cho biết có trang trước trang hiện tại hay không.
    public bool HasPreviousPage => PageIndex > 1;

    // Cờ cho biết có trang tiếp theo hay không.
    public bool HasNextPage => PageIndex < TotalPages;
}
