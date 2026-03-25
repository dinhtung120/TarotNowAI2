/*
 * ===================================================================
 * FILE: ICardsCatalogRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Tiếp (Interface) với Thư Viện 78 Lá Bài Tarot.
 *   Nơi duy nhất cung cấp Từ Điển Dữ Liệu Tĩnh về Tên Lá Bài, Thuộc Tính.
 * ===================================================================
 */

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Giao Tiếp Đọc Cuốn Sách Bách Khoa Toàn Thư Tarot Kể Về 78 Lá Bài.
/// Database lưu trữ dạng tĩnh bên MongoDB để Tăng Tốc Độ Gọi Bằng Mã Tên (The Fool, Ace Of Wands...)
/// </summary>
public interface ICardsCatalogRepository
{
    /// <summary>Đi lùng đúng lá bài Tên Số Thứ Tự (VD 0 là The Fool).</summary>
    Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Lục Danh Pháp Bằng Mã Kiểu Code (Mã này xài trong File Hình Ảnh luôn).</summary>
    Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Bê Nguyên Một Thùng 78 Lá Lên Bàn Cho Thuật Toán RNG Trộn Xào Ra Quẻ.</summary>
    Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Đúc File Khuôn Dạng Card (DTO). Nhẹ Nhàng Bưng Bê Quăng Cho Thằng Khác Qua Lại Mà Không Rối Lớp Application.
/// Tên Múi Ngôn Ngữ: Vi, En, Zh => Làm Giao Diện Translate Tự Động Rất Tiện Lợi Bằng Code Lập Trình Frontend.
/// </summary>
public class CardCatalogDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameZh { get; set; } = string.Empty;
    public string Arcana { get; set; } = string.Empty; // Ẩn Chính (Major) hay Ẩn Phụ (Minor)
    public string? Suit { get; set; } // Chất Bài: Wands (Gậy), Cups (Ly)...
    public string Element { get; set; } = string.Empty; // Hệ Lửa, Hệ Nước
    public int Number { get; set; }
    public string? ImageUrl { get; set; }

    public List<string> UprightKeywords { get; set; } = new();
    public string UprightDescription { get; set; } = string.Empty;
    public List<string> ReversedKeywords { get; set; } = new();
    public string ReversedDescription { get; set; } = string.Empty;
}
