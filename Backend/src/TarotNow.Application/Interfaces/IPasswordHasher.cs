/*
 * ===================================================================
 * FILE: IPasswordHasher.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Phác Họa Giao Diện Cho Trình Băm Chặt Xào Rác (Hash Password).
 *   Đảm Bảo Lớp Application Không Bám Gắn (Coupling) Trực Tiếp Tới Lib Code (Library B Crypt Hoặc Argon2 Nằm Bên Infrastructure Tầng Dưới).
 * ===================================================================
 */

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Găng Tay Đựng Mật Khẩu (Để Thằng Nào Dùng Lib Gì Tự Thấy Náo Mà Chém Rọc). 
/// Đổi Core Thư Viện Chạy Cho Cực Nhanh Dễ Chỉ Nhờ Thỏa Ước Lời Ép Này (DI Pattern).
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Đưa Mật Khẩu Thô Xay Nuốt Ra Cục Chữ Cám Nhìn Như Bát Giới (Hash Lẫn Muối Răng Ngộ). Lưu SQL.</summary>
    string HashPassword(string password);

    /// <summary>Kiểm Tra Pass Có Vừa Rửa So Miệng Đúng Kiểu Chuỗi Chữ Xay Trước Đây Hông Mới Mở Cửa Cho Vô DB.</summary>
    bool VerifyPassword(string hash, string providedPassword);

    /// <summary>Cho biết hash hiện tại có cần băm lại theo policy mới không.</summary>
    bool NeedsRehash(string hash);
}
