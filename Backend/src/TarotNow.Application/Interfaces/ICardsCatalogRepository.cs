
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract truy vấn danh mục lá bài để thống nhất dữ liệu hiển thị và nghiệp vụ đọc bài.
public interface ICardsCatalogRepository
{
    /// <summary>
    /// Lấy thông tin lá bài theo id để phục vụ các luồng cần tham chiếu định danh số.
    /// Luồng xử lý: truy vấn theo khóa id và trả null nếu không tồn tại.
    /// </summary>
    Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin lá bài theo mã code để hỗ trợ tra cứu theo chuẩn domain.
    /// Luồng xử lý: tìm bản ghi theo code và trả null khi không khớp dữ liệu.
    /// </summary>
    Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ danh mục lá bài để cache hoặc render catalog đầy đủ.
    /// Luồng xử lý: đọc toàn bộ bản ghi hiện có và trả danh sách kết quả.
    /// </summary>
    Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

// DTO mô tả metadata chuẩn của một lá bài trong catalog.
public class CardCatalogDto
{
    // Định danh nội bộ của lá bài.
    public int Id { get; set; }

    // Mã code duy nhất dùng cho mapping nghiệp vụ.
    public string Code { get; set; } = string.Empty;

    // Tên lá bài tiếng Việt.
    public string NameVi { get; set; } = string.Empty;

    // Tên lá bài tiếng Anh.
    public string NameEn { get; set; } = string.Empty;

    // Tên lá bài tiếng Trung.
    public string NameZh { get; set; } = string.Empty;

    // Nhóm Arcana (Major/Minor) để phân loại bộ bài.
    public string Arcana { get; set; } = string.Empty;

    // Chất bài của Minor Arcana; null khi là Major Arcana.
    public string? Suit { get; set; }

    // Nguyên tố biểu trưng của lá bài.
    public string Element { get; set; } = string.Empty;

    // Số thứ tự lá bài trong bộ.
    public int Number { get; set; }

    // Đường dẫn ảnh minh họa của lá bài.
    public string? ImageUrl { get; set; }

    // Từ khóa xuôi để phục vụ diễn giải nhanh.
    public List<string> UprightKeywords { get; set; } = new();

    // Mô tả nghĩa xuôi chi tiết.
    public string UprightDescription { get; set; } = string.Empty;

    // Từ khóa ngược để phục vụ diễn giải nhanh.
    public List<string> ReversedKeywords { get; set; } = new();

    // Mô tả nghĩa ngược chi tiết.
    public string ReversedDescription { get; set; } = string.Empty;
}
