namespace TarotNow.Api.Services;

// Trừu tượng hóa cách dựng cookie refresh token để controller không phụ thuộc chi tiết môi trường.
public interface IRefreshTokenCookieService
{
    /// <summary>
    /// Tạo cấu hình cookie refresh token phù hợp theo môi trường chạy và trạng thái HTTPS hiện tại.
    /// Luồng xử lý: đọc request context, áp dụng rule bảo mật cookie, trả về CookieOptions hoàn chỉnh.
    /// </summary>
    CookieOptions BuildOptions(HttpRequest request);
}
