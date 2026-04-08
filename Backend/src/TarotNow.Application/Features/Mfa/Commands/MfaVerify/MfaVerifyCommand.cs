using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

// Command xác nhận mã MFA để hoàn tất bật MFA cho user.
public class MfaVerifyCommand : IRequest<bool>
{
    // Định danh user thực hiện bước verify MFA.
    public Guid UserId { get; set; }

    // Mã TOTP user nhập từ ứng dụng authenticator.
    public string Code { get; set; } = string.Empty;
}

// Handler verify mã MFA và bật cờ MFA cho user khi hợp lệ.
public class MfaVerifyCommandHandler : IRequestHandler<MfaVerifyCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    /// <summary>
    /// Khởi tạo handler verify MFA.
    /// Luồng xử lý: nhận user repository để truy cập tài khoản và MFA service để giải mã/đối chiếu mã xác thực.
    /// </summary>
    public MfaVerifyCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    /// <summary>
    /// Xử lý verify MFA cho user.
    /// Luồng xử lý: kiểm tra điều kiện tiền đề setup MFA, verify mã TOTP, sau đó bật trạng thái MFA khi hợp lệ.
    /// </summary>
    public async Task<bool> Handle(MfaVerifyCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        if (user.MfaEnabled)
        {
            // Business rule: MFA đã bật thì không cho verify lặp để tránh trạng thái nghiệp vụ mơ hồ.
            throw new BadRequestException("MFA đã được bật rồi.");
        }

        if (string.IsNullOrEmpty(user.MfaSecretEncrypted))
        {
            // Edge case: chưa qua bước setup thì không có secret để verify.
            throw new BadRequestException("Vui lòng thực hiện bước Setup trước khi Verify.");
        }

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        var isValid = _mfaService.VerifyCode(plainSecret, request.Code);
        // Verify bằng secret đã setup để xác nhận user thật sự có quyền kiểm soát ứng dụng MFA.

        if (!isValid)
        {
            // Mã không hợp lệ/hết hạn thì chặn bật MFA để tránh mở khóa sai.
            throw new BadRequestException("Mã xác thực không hợp lệ hoặc đã hết hạn.");
        }

        user.MfaEnabled = true;
        // Đổi state chính thức: tài khoản đã bật MFA sau khi verify thành công.

        await _userRepo.UpdateAsync(user, cancellationToken);
        // Persist trạng thái mới để các luồng đăng nhập/ủy quyền áp dụng MFA ngay.

        return true;
    }
}
