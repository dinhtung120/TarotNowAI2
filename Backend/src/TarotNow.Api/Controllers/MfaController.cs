/*
 * ===================================================================
 * FILE: MfaController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý MFA (Multi-Factor Authentication) - Xác thực đa yếu tố.
 *
 * MFA LÀ GÌ? (Giải thích đơn giản)
 *   Bình thường đăng nhập chỉ cần mật khẩu (1 yếu tố).
 *   MFA thêm yếu tố thứ 2: mã số 6 chữ số thay đổi mỗi 30 giây,
 *   lấy từ app Google Authenticator / Authy trên điện thoại.
 *   
 *   Dù hacker biết mật khẩu, vẫn KHÔNG đăng nhập được nếu không có điện thoại.
 *   
 * CÔNG NGHỆ: TOTP (Time-based One-Time Password)
 *   - Server và app chia sẻ một "secret key" bí mật
 *   - Dựa trên thời gian hiện tại + secret key → tạo mã 6 số
 *   - Mã chỉ hợp lệ trong 30 giây, sau đó tự đổi
 *
 * LUỒNG SETUP MFA:
 *   Bước 1: POST /setup    → Server tạo secret key, trả về URI (dạng QR code)
 *   Bước 2: User quét QR bằng Google Authenticator
 *   Bước 3: POST /verify   → User nhập mã 6 số để xác nhận → MFA được bật
 *
 * LUỒNG SỬ DỤNG MFA:
 *   POST /challenge → Kiểm tra mã 6 số trước khi thực hiện hành động nhạy cảm
 *   (ví dụ: rút tiền, đổi mật khẩu, xóa tài khoản)
 * ===================================================================
 */

using MediatR;                 // MediatR trung gian
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền
using Microsoft.AspNetCore.Mvc; // API controller
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

// Import các MFA Command/Query
using TarotNow.Application.Features.Mfa.Commands.MfaChallenge; // Thử thách MFA
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;     // Thiết lập MFA
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;    // Xác nhận MFA
using TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;  // Trạng thái MFA

namespace TarotNow.Api.Controllers;

/*
 * [Authorize]: Tất cả endpoint đều yêu cầu đăng nhập.
 *   Vì MFA gắn liền với tài khoản user → phải biết ai đang gọi.
 */
[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
public class MfaController : ControllerBase
{
    private readonly IMediator _mediator;

    // Constructor dạng expression body (cú pháp ngắn gọn)
    public MfaController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Helper method: Lấy UserId từ JWT token.
    /// Trả về null nếu token không hợp lệ.
    /// </summary>
    private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Mfa/setup
    /// MỤC ĐÍCH: Bước 1 - Thiết lập MFA cho tài khoản.
    ///
    /// CÁCH HOẠT ĐỘNG:
    ///   1. Server tạo secret key ngẫu nhiên (base32 string)
    ///   2. Tạo "OTP Auth URI" (đường dẫn đặc biệt chứa secret key)
    ///      Format: otpauth://totp/TarotNow:user@email.com?secret=JBSWY3...&amp;issuer=TarotNow
    ///   3. Trả về URI cho client → Client tạo QR code từ URI
    ///   4. User quét QR bằng Google Authenticator
    ///      → App lưu secret key và bắt đầu sinh mã 6 số
    ///
    /// LƯU Ý: MFA chưa được BẬT ở bước này.
    ///   User phải gọi /verify để xác nhận đã setup đúng.
    /// </summary>
    [HttpPost("setup")]
    public async Task<IActionResult> Setup()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Gửi command → handler tạo secret, trả về URI + setup info
        var result = await _mediator.Send(new MfaSetupCommand { UserId = userId.Value });
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Mfa/verify
    /// MỤC ĐÍCH: Bước 2 - Xác nhận setup MFA bằng mã TOTP.
    ///
    /// SAU KHI USER QUÉT QR:
    ///   1. App Authenticator hiển thị mã 6 số (thay đổi mỗi 30 giây)
    ///   2. User nhập mã 6 số vào ứng dụng TarotNow
    ///   3. Server kiểm tra mã đúng không
    ///   4. Nếu đúng → BẬT MFA cho tài khoản (mfaEnabled = true)
    ///   5. Từ giờ mọi hành động nhạy cảm đều cần mã MFA
    ///
    /// NẾU MÃ SAI: trả lỗi, user phải nhập lại.
    /// </summary>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] MfaVerifyBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Kiểm tra mã TOTP và bật MFA nếu đúng
        await _mediator.Send(new MfaVerifyCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "MFA đã được bật thành công." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Mfa/challenge
    /// MỤC ĐÍCH: Xác thực MFA cho hành động nhạy cảm.
    ///
    /// KHI NÀO DÙNG?
    ///   Trước khi thực hiện các hành động quan trọng:
    ///   - Rút tiền (withdrawal)
    ///   - Đổi mật khẩu
    ///   - Thay đổi email
    ///   - Admin duyệt giao dịch lớn
    ///   
    ///   Client gọi /challenge trước → nếu thành công → cho phép hành động.
    ///   
    /// KHÁC GÌ VỚI /verify?
    ///   - /verify: dùng MỘT LẦN khi setup MFA
    ///   - /challenge: dùng NHIỀU LẦN mỗi khi cần xác thực hành động nhạy cảm
    /// </summary>
    [HttpPost("challenge")]
    public async Task<IActionResult> Challenge([FromBody] MfaChallengeBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _mediator.Send(new MfaChallengeCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "Xác thực MFA thành công." });
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Mfa/status
    /// MỤC ĐÍCH: Kiểm tra MFA đã được bật cho tài khoản chưa.
    ///
    /// TRẢ VỀ: { mfaEnabled: true/false }
    ///   - true: MFA đã bật → client sẽ yêu cầu mã MFA khi cần
    ///   - false: MFA chưa bật → client hiển thị nút "Bật MFA" trong cài đặt
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _mediator.Send(new GetMfaStatusQuery { UserId = userId.Value });
        return Ok(new { mfaEnabled = result.MfaEnabled });
    }
}
