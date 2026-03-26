using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public partial class CreateWithdrawalCommandHandler
{
    private async Task<User> GetUserOrThrowAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepo.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
    }

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

    private void ValidateMfa(CreateWithdrawalCommand request, User user)
    {
        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
        {
            throw new BadRequestException("Bạn phải bật bảo mật 2 lớp (MFA) trước khi rút tiền.");
        }

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(plainSecret, request.MfaCode))
        {
            throw new BadRequestException("Mã xác thực MFA không hợp lệ hoặc đã hết hạn.");
        }
    }

    private static void ValidateBusinessRules(CreateWithdrawalCommand request, User user)
    {
        if (request.AmountDiamond < 50)
        {
            throw new BadRequestException("Số lượng rút tối thiểu là 50 Diamond.");
        }

        if (user.ReaderStatus != "approved")
        {
            throw new BadRequestException("Bạn cần hoàn tất xác minh Reader (KYC) trước khi rút tiền.");
        }

        if (user.DiamondBalance < request.AmountDiamond)
        {
            throw new BadRequestException($"Số dư không đủ. Hiện có {user.DiamondBalance}💎, cần {request.AmountDiamond}💎.");
        }
    }

    private async Task ValidateDailyLimitAsync(Guid userId, DateOnly today, CancellationToken cancellationToken)
    {
        var hasPending = await _withdrawalRepo.HasPendingRequestTodayAsync(userId, today, cancellationToken);
        if (hasPending)
        {
            throw new BadRequestException("Bạn đã có yêu cầu rút tiền hôm nay. Vui lòng thử lại ngày mai.");
        }
    }

    private static void ValidateBankInfo(CreateWithdrawalCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.BankName)
            || string.IsNullOrWhiteSpace(request.BankAccountName)
            || string.IsNullOrWhiteSpace(request.BankAccountNumber))
        {
            throw new BadRequestException("Thông tin ngân hàng không hợp lệ.");
        }
    }
}
