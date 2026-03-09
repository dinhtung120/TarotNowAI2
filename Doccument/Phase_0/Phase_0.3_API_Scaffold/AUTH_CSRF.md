# Cấu hình Refresh Token Cookie & CSRF Protection (Phase 0.3)

Tài liệu hướng dẫn triển khai bảo mật cho Authentication Token và chống tấn công CSRF, theo chuẩn OWASP và tài liệu thiết kế TarotNow.

## 1. Refresh Token Cookie Configuration

Để bảo vệ `refresh_token` khỏi các lỗ hổng XSS (đánh cắp token), hệ thống **bắt buộc** phải chuyển/trả về `refresh_token` qua **HTTP-Only Cookies** thay vì trả về trong body của response JSON.

### Cấu hình Cookie chuẩn:
```csharp
var cookieOptions = new CookieOptions
{
    HttpOnly = true, // KHÔNG thể truy cập qua JavaScript (chống XSS)
    Secure = true, // Chỉ gửi qua kết nối HTTPS (chống MitM)
    SameSite = SameSiteMode.Strict, // Nâng cao bảo mật chống CSRF (chỉ gửi trong cùng site)
    Expires = DateTime.UtcNow.AddDays(7) // Cấu hình theo thời gian hết hạn của Refresh Token
};

// Response trả cookie cho Client
Response.Cookies.Append("refreshToken", token, cookieOptions);
```

### Flow sử dụng:
1. **Login:** Server kiểm tra credential hợp lệ -> Trả `access_token` qua JSON body, gửi `refresh_token` qua HTTP-Only Cookie.
2. **Refresh Token:** Client request endpoint `/api/v1/Token/Refresh` mà không cần đính kèm token trong body -> Cookie tự động được Browser đính kèm.
3. **Logout:** Server ghi đè cookie `refreshToken` với tuổi thọ hết hạn ngay lập tức (`Expires = DateTime.UtcNow.AddDays(-1)`).

---

## 2. CSRF Protection (Cross-Site Request Forgery)

Do chúng ta đang sử dụng Cookie cho `refresh_token` (và có thể cả authentication sau này nếu có thay đổi), có rủi ro bị tấn công **CSRF**. 

### Các lớp bảo vệ hiện tại:

1. **SameSite=Strict**
    * Thuộc tính `SameSiteMode.Strict` được sử dụng trong cookie để đảm bảo browser CHỈ gửi cookie nếu request được phát khởi từ đúng nguồn (same origin) của ứng dụng. Bất kỳ bên thứ ba nào gọi API (ví dụ: `<img>` tags ẩn hoặc hidden forms trên các trang web giả mạo) đều không được browser đính kèm cookie.

2. **Antiforgery Token Pattern (Dự phòng cho Cookie Authentication)**
    * Nếu ứng dụng FrontEnd (Next.js) chuyển hẳn sang dùng Cookie Authentication cho cả access token (để SSR tốt hơn), chúng ta cần kích hoạt **Double Submit Cookie** hoặc **Synchronizer Token Pattern**.
    * .NET Core cung cấp sẵn qua `services.AddAntiforgery(...)` và `app.UseAntiforgery()`.

3. **CORS Configuration**
    * Application sẽ chỉ tin tưởng các Domain được định nghĩa ở mục `AllowedOrigins` trong `appsettings.json` (vd: `http://localhost:3000`).
    * CORS preflight sẽ từ chối các request xuất phát từ miền khác không nằm trong Whitelist.

*Lưu ý: JWT Access Token đính kèm qua header `Authorization` (Bearer Token) KHÔNG dính lỗ hổng CSRF vì JWT Header không được browser tự động đính kèm theo request chéo trang.*
