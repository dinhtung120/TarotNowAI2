
namespace TarotNow.Application.Interfaces;

// Contract băm mật khẩu để tách nghiệp vụ xác thực khỏi thuật toán hash cụ thể.
public interface IPasswordHasher
{
    /// <summary>
    /// Băm mật khẩu thô trước khi lưu để bảo vệ thông tin đăng nhập.
    /// Luồng xử lý: nhận password plain text và trả chuỗi hash kèm thông tin thuật toán.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Xác minh mật khẩu người dùng nhập so với hash đã lưu.
    /// Luồng xử lý: dùng hash hiện có để kiểm tra providedPassword và trả kết quả đúng/sai.
    /// </summary>
    bool VerifyPassword(string hash, string providedPassword);

    /// <summary>
    /// Kiểm tra hash cũ có cần băm lại theo cấu hình bảo mật mới hay không.
    /// Luồng xử lý: phân tích metadata hash và trả true khi thuật toán/tham số đã lỗi thời.
    /// </summary>
    bool NeedsRehash(string hash);
}
