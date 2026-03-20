/*
 * ===================================================================
 * FILE: ResetPasswordCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.ResetPassword
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý Xác thực OTP vòng cuối và Đặt mới Mật Khẩu.
 *
 * LUỒNG HOẠT ĐỘNG:
 *   1. Tìm người dùng bằng Email.
 *   2. Tìm mã OTP hợp lệ MỚI NHẤT (còn hiệu lực 15p) chưa được dùng.
 *   3. So khớp Mã số OTP gửi lên với Mã số OTP trên Database.
 *   4. Băm (Hash) mật khẩu mới. Update User Entity.
 *   5. Hủy hiệu lực mã OTP (ngăn tấn công dùng lại OTP - Replay Attack).
 *   6. BUỘC ĐĂNG XUẤT MỌI NƠI: Thu hồi toàn bộ Refresh Tokens của User. 
 *      Buộc tin tặc (nếu có giữ thiết bị) cũng bị đẩy ra ngoài.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Trung tâm thay lõi mật khẩu và vô hiệu hoá các phiên đăng nhập lạ.
/// </summary>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository, 
        IEmailOtpRepository emailOtpRepository, 
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Lấy mã OTP mới nhất còn hanh hiệu và thuộc về user này. Thể loại OTP: Dành cho Reset.
        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, cancellationToken);
        
        // Cổng kiểm định: Nếu mã null, quá thời hạn, hoặc nhập sai mã -> Đều báo chung "OTP không hợp lệ".
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Đổi mật khẩu: Cần Hash bảo vệ mới trước khi nhét ngược lại Domain.
        var newHash = _passwordHasher.HashPassword(request.NewPassword);
        user.UpdatePassword(newHash);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        // BẢO MẬT: Hủy giá trị mã OTP (Mark As Used = Đã sử dụng). Không cho phép ai copy dán lại nữa.
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // BẢO MẬT CHỦ ĐỘNG (Proactive Security):
        // Khi người dùng bấm quên mật khẩu, có rủi ro là họ bị lộ hoặc mất điện thoại.
        // Cần huỷ toàn bộ phiên truy cập cũ. 
        // Lần sau khi app/điện thoại xin JWT token mới sẽ bị từ chối 100%. Bắt nhập Password Mới.
        await _refreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);

        return true;
    }
}
