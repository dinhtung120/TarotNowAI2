/*
 * ===================================================================
 * FILE: AuthController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller xử lý toàn bộ luồng XÁC THỰC (Authentication) của ứng dụng:
 *   1. Đăng ký tài khoản mới (Register)
 *   2. Đăng nhập (Login) → nhận JWT Access Token + Refresh Token
 *   3. Làm mới token (Refresh) → xoay vòng Refresh Token
 *   4. Đăng xuất (Logout) → thu hồi Refresh Token
 *   5. Gửi OTP xác thực email
 *   6. Xác thực email bằng OTP
 *   7. Quên mật khẩu → gửi OTP reset
 *   8. Đặt lại mật khẩu mới
 *
 * CƠ CHẾ BẢO MẬT TOKEN:
 *   - Access Token (JWT): Lưu trong memory/localStorage ở client, thời hạn ngắn (~15 phút).
 *     Dùng để gửi kèm mỗi request API (header Authorization: Bearer {token}).
 *   - Refresh Token: Lưu trong HttpOnly Cookie, thời hạn dài (~7 ngày).
 *     Dùng để xin Access Token mới khi hết hạn.
 *     HttpOnly Cookie KHÔNG thể bị JavaScript đọc → chống tấn công XSS.
 *   - Refresh Token Rotation: Mỗi lần refresh, token cũ bị hủy và sinh token mới.
 *     Nếu token cũ bị dùng lại → phát hiện bị đánh cắp → hủy tất cả session.
 * ===================================================================
 */

// Import các thư viện cần thiết
using MediatR;                 // MediatR: gửi Command/Query đến Handler xử lý
using Microsoft.AspNetCore.Mvc; // Nền tảng xây dựng API controller
using Microsoft.Extensions.Configuration; // Đọc cấu hình từ appsettings.json

// Import các Command cho từng use-case xác thực
using TarotNow.Application.Features.Auth.Commands.Login;        // Command đăng nhập
using TarotNow.Application.Features.Auth.Commands.Register;     // Command đăng ký
using TarotNow.Application.Features.Auth.Commands.RefreshToken; // Command làm mới token
using TarotNow.Application.Features.Auth.Commands.RevokeToken;  // Command thu hồi token

namespace TarotNow.Api.Controllers;

/*
 * [ApiController]: Bật tính năng API mặc định (auto-validation, auto-binding)
 * [Route("api/v1/[controller]")]: URL gốc = /api/v1/Auth
 * 
 * KHÔNG có [Authorize] ở cấp class vì:
 * - Đăng ký, đăng nhập, quên mật khẩu PHẢI cho phép truy cập mà không cần đăng nhập.
 * - Chỉ một số endpoint cần Authorize sẽ đánh dấu riêng.
 */
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    /*
     * _mediator: Trung gian MediatR để gửi commands.
     * _environment: Thông tin môi trường chạy (Development/Production/Staging).
     *   Dùng để quyết định cookie có cần "Secure" flag không.
     * _configuration: Đọc cấu hình từ file appsettings.json.
     *   Ví dụ: đọc thời hạn Refresh Token, secret key JWT, v.v.
     */
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    /*
     * Constructor - nhận 3 dependency qua Dependency Injection:
     * 1. mediator: gửi commands/queries
     * 2. environment: biết đang chạy ở môi trường nào
     * 3. configuration: đọc cấu hình
     */
    public AuthController(IMediator mediator, IWebHostEnvironment environment, IConfiguration configuration)
    {
        _mediator = mediator;
        _environment = environment;
        _configuration = configuration;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/register
    /// MỤC ĐÍCH: Đăng ký tài khoản người dùng mới.
    ///
    /// LUỒNG:
    ///   1. Client gửi: { email, password, displayName }
    ///   2. Handler kiểm tra email chưa tồn tại, validate password
    ///   3. Tạo user mới trong database (trạng thái chưa kích hoạt)
    ///   4. Trả về userId để client chuyển đến trang verify email
    ///
    /// [ProducesResponseType]: Metadata cho Swagger/OpenAPI.
    ///   Cho biết endpoint này CÓ THỂ trả về những mã HTTP nào:
    ///   - 201 Created: đăng ký thành công
    ///   - 400 Bad Request: dữ liệu không hợp lệ (password quá ngắn, v.v.)
    ///   - 422 Unprocessable Entity: vi phạm business rule (email đã tồn tại)
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Domain Rule violation (email exist)
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        // Gửi command đăng ký → handler trả về userId (Guid) của user vừa tạo
        var userId = await _mediator.Send(command);

        // Tạo response object để trả về cho client
        var response = new RegisterResponse
        {
            UserId = userId,
            Message = "Registration successful. Please verify your email to activate your account."
        };

        /*
         * CreatedAtAction: Trả về HTTP 201 Created (thành công, đã tạo resource mới).
         * - "GetProfile": tên action để client biết URL lấy thông tin user vừa tạo
         * - "Profile": tên controller chứa action đó
         * - null: route values (không cần ở đây)
         * - response: dữ liệu trả về cho client
         * 
         * HTTP 201 thay vì 200 vì theo chuẩn REST:
         * - 200 = "OK, đã xử lý xong"
         * - 201 = "OK, đã TẠO MỚI resource" (chính xác hơn cho register)
         */
        return CreatedAtAction("GetProfile", "Profile", null, response);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/login
    /// MỤC ĐÍCH: Đăng nhập để nhận JWT Access Token và Refresh Token.
    ///
    /// LUỒNG:
    ///   1. Client gửi: { email, password }
    ///   2. Handler kiểm tra email/password, tạo Access Token + Refresh Token
    ///   3. Refresh Token được đặt vào HttpOnly Cookie (chống XSS)
    ///   4. Access Token được trả về trong response body (client lưu trong memory)
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Domain Rule violation (invalid criteria)
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        /*
         * Lấy địa chỉ IP của client gửi request.
         * Dùng để ghi log bảo mật: biết login từ đâu.
         * Nếu đăng nhập từ IP bất thường → có thể cảnh báo user.
         * "??" là null-coalescing: nếu IP null → dùng "unknown".
         */
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        // Gửi command đăng nhập qua MediatR
        var result = await _mediator.Send(command);

        /*
         * Thiết lập Refresh Token thành HttpOnly Cookie.
         * 
         * TẠI SAO DÙNG HTTPONLY COOKIE?
         *   - HttpOnly: JavaScript KHÔNG THỂ đọc cookie này → chống XSS attack.
         *     (XSS = hacker chèn code JavaScript vào web để đánh cắp token)
         *   - Secure: Cookie chỉ gửi qua HTTPS → chống man-in-the-middle attack.
         *   - SameSite=Strict: Cookie không gửi theo cross-site request → chống CSRF.
         *     (CSRF = hacker lừa user click link từ web khác để thực hiện giao dịch)
         */
        var cookieOptions = BuildRefreshCookieOptions();
        
        // Đặt Refresh Token vào cookie với tên "refreshToken"
        Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

        // Trả về Access Token và thông tin user trong response body
        // (Refresh Token KHÔNG trả trong body vì đã có trong cookie)
        return Ok(result.Response);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/refresh
    /// MỤC ĐÍCH: Xoay vòng Refresh Token (Rotation) để cấp JWT mới.
    ///
    /// REFRESH TOKEN ROTATION LÀ GÌ?
    ///   Khi Access Token hết hạn (thường sau 15 phút), client gọi endpoint này
    ///   với Refresh Token (trong cookie) để xin Access Token mới.
    ///   Server sẽ:
    ///   1. Kiểm tra Refresh Token hợp lệ
    ///   2. HỦY token cũ (đánh dấu "revoked" trong database)
    ///   3. Tạo Refresh Token MỚI và Access Token MỚI
    ///   4. Trả về Access Token mới, đặt Refresh Token mới vào cookie
    ///
    ///   Nếu token cũ đã bị revoked mà vẫn được dùng → phát hiện đánh cắp
    ///   → hủy TẤT CẢ session của user → bắt buộc đăng nhập lại.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Cookie không hợp lệ hoặc Token compromised
    public async Task<IActionResult> RefreshTokens()
    {
        // Đọc Refresh Token từ HttpOnly Cookie
        // (Client KHÔNG gửi trong body/header vì cookie tự động đính kèm)
        var refreshToken = Request.Cookies["refreshToken"];

        // Nếu không có cookie → user chưa đăng nhập hoặc cookie hết hạn
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is missing." });
        }

        // Tạo command refresh với token và IP client
        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        };

        // Gửi command → handler xác thực, hủy token cũ, sinh token mới
        var result = await _mediator.Send(command);

        // Đặt Refresh Token MỚI vào cookie (thay thế token cũ)
        var cookieOptions = BuildRefreshCookieOptions();
        Response.Cookies.Append("refreshToken", result.NewRefreshToken, cookieOptions);

        // Trả về Access Token mới trong response body
        return Ok(result.Response);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/logout?revokeAll=false
    /// MỤC ĐÍCH: Đăng xuất - thu hồi Refresh Token.
    ///
    /// HAI CHẾ ĐỘ:
    ///   1. revokeAll=false (mặc định): Chỉ hủy session hiện tại (thiết bị này).
    ///      Các thiết bị khác vẫn đăng nhập bình thường.
    ///   2. revokeAll=true: Hủy TẤT CẢ session trên mọi thiết bị.
    ///      Dùng khi nghi ngờ bị hack → đuổi hết mọi thiết bị ra.
    ///      Cần phải đang đăng nhập (có JWT) để xác nhận danh tính.
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromQuery] bool revokeAll = false)
    {
        // Đọc Refresh Token từ cookie
        var refreshToken = Request.Cookies["refreshToken"];
        
        // Tạo command thu hồi token
        var command = new RevokeTokenCommand
        {
            Token = refreshToken ?? string.Empty, // Nếu null → gán chuỗi rỗng
            RevokeAll = revokeAll                  // true = hủy tất cả session
        };

        if (revokeAll)
        {
            /*
             * Nếu revokeAll = true: cần biết UserId để hủy tất cả token của user đó.
             * UserId lấy từ JWT claims (user phải đang đăng nhập).
             * Nếu không có JWT hợp lệ → từ chối (401 Unauthorized).
             */
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var parsedUserId))
            {
                command.UserId = parsedUserId;
            }
            else
            {
                // Chưa đăng nhập mà muốn hủy tất cả → không cho phép
                return Unauthorized(new { message = "Must be authenticated to revoke all sessions." });
            }
        }
        else if (string.IsNullOrEmpty(command.Token))
        {
            // Không revokeAll và cũng không có token → không biết hủy cái gì
            return BadRequest(new { message = "No refresh token provided." });
        }

        // Gửi command thu hồi token qua MediatR
        await _mediator.Send(command);

        /*
         * Xóa cookie "refreshToken" ở phía client.
         * Dù server đã đánh dấu token là "revoked" trong database,
         * vẫn cần xóa cookie để trình duyệt không gửi token cũ nữa.
         * BuildRefreshCookieOptions() đảm bảo xóa đúng cookie (cùng path, domain).
         */
        Response.Cookies.Delete("refreshToken", BuildRefreshCookieOptions());

        return Ok(new { message = "Logged out successfully." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/send-verification-email
    /// MỤC ĐÍCH: Gửi mã OTP qua email để xác thực tài khoản sau đăng ký.
    ///
    /// BẢO MẬT QUAN TRỌNG:
    ///   Luôn trả về 200 OK dù email có tồn tại hay không.
    ///   Nếu trả 404 khi email không tồn tại → hacker có thể dò xem
    ///   email nào đã đăng ký trên hệ thống (email enumeration attack).
    /// </summary>
    [HttpPost("send-verification-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendVerificationEmail([FromBody] TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp.SendEmailVerificationOtpCommand command)
    {
        // Luôn trả về OK để chống dò email (email enumeration prevention)
        await _mediator.Send(command);
        return Ok(new { message = "If the email is valid and unverified, an OTP has been sent." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/verify-email
    /// MỤC ĐÍCH: Xác thực email bằng mã OTP đã gửi.
    ///
    /// SAU KHI XÁC THỰC THÀNH CÔNG:
    ///   - Trạng thái tài khoản chuyển từ "Inactive" → "Active"
    ///   - User có thể đăng nhập và sử dụng đầy đủ tính năng
    /// </summary>
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromBody] TarotNow.Application.Features.Auth.Commands.VerifyEmail.VerifyEmailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = "Email verified successfully. Account is now active." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/forgot-password
    /// MỤC ĐÍCH: Gửi OTP đặt lại mật khẩu qua email.
    ///
    /// Cùng nguyên tắc bảo mật với send-verification-email:
    /// luôn trả OK dù email tồn tại hay không → chống dò email.
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] TarotNow.Application.Features.Auth.Commands.ForgotPassword.ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email exists, a password reset OTP has been sent." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Auth/reset-password
    /// MỤC ĐÍCH: Đặt lại mật khẩu mới bằng OTP.
    ///
    /// SAU KHI RESET THÀNH CÔNG:
    ///   1. Mật khẩu cũ bị thay thế bằng hash của mật khẩu mới
    ///   2. TẤT CẢ Refresh Token bị thu hồi (revoke)
    ///      → Tất cả thiết bị bị đăng xuất → bắt đăng nhập lại bằng mật khẩu mới
    ///      → Nếu hacker đang giữ session cũ → cũng bị đuổi ra
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] TarotNow.Application.Features.Auth.Commands.ResetPassword.ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = "Password has been successfully reset. All existing devices have been logged out." });
    }

    /*
     * ===================================================================
     * PRIVATE HELPER METHODS (Các hàm hỗ trợ nội bộ)
     * ===================================================================
     * Các hàm bên dưới KHÔNG phải endpoint API.
     * Chúng là "công cụ nội bộ" được các endpoint ở trên gọi.
     * "private" nghĩa là chỉ class này mới dùng được, bên ngoài không truy cập.
     * ===================================================================
     */

    /// <summary>
    /// Tạo CookieOptions cho Refresh Token.
    /// Tập trung logic cookie vào một chỗ để đảm bảo nhất quán.
    /// </summary>
    private CookieOptions BuildRefreshCookieOptions()
    {
        // Lấy thời hạn hiệu lực của Refresh Token từ cấu hình
        var expiryDays = ResolveRefreshTokenExpiryDays();

        /*
         * shouldUseSecureCookie: quyết định có bật flag "Secure" không.
         * - Production: LUÔN bật (chỉ gửi cookie qua HTTPS)
         * - Development: chỉ bật nếu đang dùng HTTPS (dev có thể chạy HTTP)
         * 
         * flag "Secure" bật → cookie KHÔNG được gửi qua HTTP thường
         * → chống man-in-the-middle attack (hacker nghe lén mạng)
         */
        var shouldUseSecureCookie = !_environment.IsDevelopment() || Request.IsHttps;

        return new CookieOptions
        {
            /*
             * HttpOnly = true:
             *   JavaScript KHÔNG THỂ đọc cookie này (document.cookie không thấy).
             *   Chỉ trình duyệt mới gửi kèm HTTP request.
             *   → Chống XSS attack: dù hacker chèn được JS, không đọc được token.
             */
            HttpOnly = true,

            /*
             * Secure: Cookie chỉ gửi qua kết nối HTTPS (mã hóa).
             *   → Chống bị nghe lén trên mạng không an toàn (Wi-Fi công cộng).
             */
            Secure = shouldUseSecureCookie,

            /*
             * SameSite = Strict:
             *   Cookie CHỈ gửi khi request đến từ CÙNG domain.
             *   Nếu user click link trên web khác dẫn đến API → cookie KHÔNG gửi.
             *   → Chống CSRF attack: hacker không thể lợi dụng cookie của user.
             */
            SameSite = SameSiteMode.Strict,

            /*
             * Expires: Thời điểm cookie hết hạn.
             *   Sau thời gian này, trình duyệt tự xóa cookie.
             *   Phải khớp với thời hạn Refresh Token trong database.
             */
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };
    }

    /// <summary>
    /// Đọc thời hạn Refresh Token từ file cấu hình (appsettings.json).
    /// Hỗ trợ 2 tên key để tương thích ngược:
    ///   - "Jwt:RefreshExpiryDays" (tên mới)
    ///   - "Jwt:RefreshTokenExpirationDays" (tên cũ)
    /// Mặc định 7 ngày nếu không tìm thấy cấu hình.
    /// </summary>
    private int ResolveRefreshTokenExpiryDays()
    {
        /*
         * _configuration["Jwt:RefreshExpiryDays"]: đọc giá trị từ appsettings.json
         * Ví dụ file appsettings.json:
         * {
         *   "Jwt": {
         *     "RefreshExpiryDays": "14"
         *   }
         * }
         * 
         * "??" (null-coalescing): nếu key đầu không tìm thấy → thử key thứ 2.
         * Đây là cách hỗ trợ backward compatibility (tương thích ngược).
         */
        var configured = _configuration["Jwt:RefreshExpiryDays"]
                         ?? _configuration["Jwt:RefreshTokenExpirationDays"];

        /*
         * int.TryParse: thử chuyển string → int.
         *   - Nếu thành công VÀ giá trị > 0 → dùng giá trị đó
         *   - Nếu thất bại (không phải số, hoặc ≤ 0) → dùng mặc định 7 ngày
         * 
         * TẠI SAO MẶC ĐỊNH 7 NGÀY?
         *   - Đủ dài để user không phải đăng nhập liên tục (UX tốt)
         *   - Đủ ngắn để giảm rủi ro nếu token bị đánh cắp (bảo mật)
         */
        return int.TryParse(configured, out var value) && value > 0
            ? value
            : 7;
    }
}
