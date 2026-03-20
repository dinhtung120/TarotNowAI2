/*
 * ===================================================================
 * FILE: VerifyEmailCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.VerifyEmail
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thực thi luồng xác nhận Email.
 *   Từ Pending -> Active. Tặng điểm Gold nòng cốt cho người dùng mới.
 *
 * WORKFLOW:
 *   1. Tìm người dùng bằng Email.
 *   2. Kiểm tra nếu Đã Xác Thực rồi -> Báo lỗi rác (tránh spam API).
 *   3. Lấy mã OTP mới nhất sinh ra cho mục đích "Xác Nhận Email".
 *   4. Nếu đúng: Thay đổi trạng thái tài khoản.
 *   5. TRẢ THƯỞNG: Châm ngòi cơ chế Sinh lợi (Gamification). Tặng lập tức 5 Gold chào sân.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

/// <summary>
/// Trạm gác cuối cùng để mở cửa cho User vào hệ thống sử dụng Dịch Vụ.
/// </summary>
public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository, IEmailOtpRepository emailOtpRepository)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm thông tin
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // 2. Tránh làm hệ thống chạy dư thừa nếu User vô tình click 2 lần.
        if (user.Status == UserStatus.Active)
        {
            throw new BusinessRuleException("EMAIL_ALREADY_VERIFIED", "This email address is already verified.");
        }

        // 3. Đọc dữ liệu OTP thuộc thể loại VerifyEmail (Phân biệt rạch ròi với ForgotPassword OTP).
        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, cancellationToken);
        
        // Cổng chốt an ninh
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Đánh dấu để Phế bỏ OTP (Chống Dùng Lại).
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // 4. Kích hoạt tài khoản: Mở khoá đăng nhập (JWT Token).
        user.Activate();

        // -------------------------------------------------------------
        // 5. TẶNG THƯỞNG TÂN THỦ (Gamification / Onboarding Hook)
        // Khi xác thực thành công, Hệ thống tự động đẩy 5 Đồng Vàng (Gold) vào ví.
        // Đây gọi là Onboarding Hook: Khuyến khích user dùng thử ngay chức năng xem bài Tarot AI tốn phí ảo.
        // -------------------------------------------------------------
        user.Wallet.Credit(CurrencyType.Gold, 5, TransactionType.RegisterBonus);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        return true;
    }
}
