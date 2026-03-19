/*
 * ===================================================================
 * FILE: MfaVerifyCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Mfa.Commands.MfaVerify
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bước Khóa Chốt (MFA Step 2).
 *   Sau khi Frontend hiển thị ảnh QR, khách quét rồi ra 6 con số.
 *   API này bắt User gõ Thử 6 Số đó vào để "Kiểm định" App hoạt động đồng bộ với DB chưa.
 *
 * NOTE DO OR DIE:
 *   Nếu Nhập đúng -> Cập nhật Cờ MfaEnabled = TRUE => Chúc mừng, Két sắt đã chốt Khóa Tử.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

/// <summary>
/// Gói Lệnh Thử Chìa Khóa Mới Sinh Ra Của App Điện Thoại.
/// </summary>
public class MfaVerifyCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
    /// <summary>6 Số Đồng Hồ chớp chớp trên Google Authenticator.</summary>
    public string Code { get; set; } = string.Empty;
}

public class MfaVerifyCommandHandler : IRequestHandler<MfaVerifyCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaVerifyCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<bool> Handle(MfaVerifyCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        // Bật rồi thì miễn test lại (Mắc công Lag API Cập nhật bậy bị dính lỗi Timeout).
        if (user.MfaEnabled)
            throw new BadRequestException("MFA đã được bật rồi.");

        // Đi tắt đón đầu? Bắt phạt phải Sinh Phôi MFA (Setup) trước.
        if (string.IsNullOrEmpty(user.MfaSecretEncrypted))
            throw new BadRequestException("Vui lòng thực hiện bước Setup trước khi Verify.");

        // ==========================================
        //  KIỂM ĐỊNH KẾT QUẢ VÀ NGHIỆM THU
        // ==========================================
        // 1. Phá Lớp Vỏ AES Lấy Lại Phôi Ký Tự.
        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        
        // 2. Chấm Điểm 6 Số xem điện thoại khách và Server Tarot đồng bộ múi giờ không.
        var isValid = _mfaService.VerifyCode(plainSecret, req.Code);

        // 3. Toang. Trượt bài test. Nhập sai hoặc để lâu quá 30s.
        if (!isValid)
            throw new BadRequestException("Mã xác thực không hợp lệ hoặc đã hết hạn.");

        // 4. Tuyệt Cú Mèo. Thẩm định xong => Khóa cửa Bật Công Tắc Chính (MfaEnabled = true).
        user.MfaEnabled = true;
        await _userRepo.UpdateAsync(user, ct);

        return true;
    }
}
