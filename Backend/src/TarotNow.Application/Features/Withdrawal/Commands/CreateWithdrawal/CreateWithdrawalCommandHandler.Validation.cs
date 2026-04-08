using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public partial class CreateWithdrawalCommandHandler
{
    /// <summary>
    /// Lấy user theo id hoặc ném NotFound.
    /// Luồng xử lý: truy vấn user repository và chặn tiếp tục nếu không tồn tại user.
    /// </summary>
    private async Task<User> GetUserOrThrowAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepo.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
    }

    /// <summary>
    /// Chạy toàn bộ bước validation trước khi tạo withdrawal.
    /// Luồng xử lý: xác thực MFA, kiểm tra business rule số dư/quyền rút, kiểm tra giới hạn ngày và validate thông tin ngân hàng.
    /// </summary>
    private async Task ValidateRequestAsync(
        CreateWithdrawalCommand request,
        User user,
        DateOnly today,
        CancellationToken cancellationToken)
    {
        ValidateMfa(request, user);
        ValidateBusinessRules(request, user);
        await ValidateDailyLimitAsync(request.UserId, today, cancellationToken);
        ValidateBankInfo(request);
    }

    /// <summary>
    /// Xác thực MFA của user cho thao tác rút tiền.
    /// Luồng xử lý: bảo đảm user đã bật MFA và verify mã hiện tại trước khi cho phép tạo yêu cầu rút.
    /// </summary>
    private void ValidateMfa(CreateWithdrawalCommand request, User user)
    {
        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
        {
            // Business rule: rút tiền bắt buộc có MFA để giảm rủi ro chiếm đoạt tài khoản.
            throw new BadRequestException("Bạn phải bật bảo mật 2 lớp (MFA) trước khi rút tiền.");
        }

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(plainSecret, request.MfaCode))
        {
            // Mã MFA sai/hết hạn thì chặn tạo yêu cầu rút.
            throw new BadRequestException("Mã xác thực MFA không hợp lệ hoặc đã hết hạn.");
        }
    }

    /// <summary>
    /// Kiểm tra các business rule của yêu cầu rút.
    /// Luồng xử lý: kiểm tra mức rút tối thiểu, trạng thái reader đã duyệt và số dư diamond đủ để rút.
    /// </summary>
    private static void ValidateBusinessRules(CreateWithdrawalCommand request, User user)
    {
        if (request.AmountDiamond < 50)
        {
            // Mức rút tối thiểu nhằm tránh tạo quá nhiều request giá trị nhỏ.
            throw new BadRequestException("Số lượng rút tối thiểu là 50 Diamond.");
        }

        if (user.ReaderStatus != "approved")
        {
            // Business rule KYC: chỉ reader đã duyệt mới được rút tiền.
            throw new BadRequestException("Bạn cần hoàn tất xác minh Reader (KYC) trước khi rút tiền.");
        }

        if (user.DiamondBalance < request.AmountDiamond)
        {
            // Số dư không đủ thì chặn để tránh phát sinh trạng thái âm ví.
            throw new BadRequestException($"Số dư không đủ. Hiện có {user.DiamondBalance}💎, cần {request.AmountDiamond}💎.");
        }
    }

    /// <summary>
    /// Kiểm tra giới hạn số lần rút trong ngày.
    /// Luồng xử lý: chặn tạo thêm request khi user đã có yêu cầu pending trong cùng business date.
    /// </summary>
    private async Task ValidateDailyLimitAsync(Guid userId, DateOnly today, CancellationToken cancellationToken)
    {
        var hasPending = await _withdrawalRepo.HasPendingRequestTodayAsync(userId, today, cancellationToken);
        if (hasPending)
        {
            // Edge case: đã có request pending hôm nay thì buộc đợi sang ngày kế tiếp.
            throw new BadRequestException("Bạn đã có yêu cầu rút tiền hôm nay. Vui lòng thử lại ngày mai.");
        }
    }

    /// <summary>
    /// Kiểm tra thông tin ngân hàng đầu vào.
    /// Luồng xử lý: yêu cầu đầy đủ tên ngân hàng, tên chủ tài khoản và số tài khoản.
    /// </summary>
    private static void ValidateBankInfo(CreateWithdrawalCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.BankName) ||
            string.IsNullOrWhiteSpace(request.BankAccountName) ||
            string.IsNullOrWhiteSpace(request.BankAccountNumber))
        {
            // Thông tin nhận tiền không đầy đủ thì không thể tạo yêu cầu chi trả.
            throw new BadRequestException("Thông tin ngân hàng không hợp lệ.");
        }
    }
}
