using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public partial class UpdateUserCommandHandler
{
    /// <summary>
    /// Cập nhật vai trò và trạng thái khóa của user theo command.
    /// Luồng xử lý: chuẩn hóa role/status, cập nhật role nếu thay đổi, xử lý nhánh lock/unlock tương ứng.
    /// </summary>
    private static void UpdateRoleAndStatus(User user, UpdateUserCommand request)
    {
        var normalizedRole = request.Role?.Trim().ToLowerInvariant() ?? UserRole.User;
        if (user.Role != normalizedRole)
        {
            // Chỉ cập nhật role khi có thay đổi thực sự để giảm ghi đè không cần thiết.
            user.UpdateRole(normalizedRole);
        }

        var normalizedStatus = request.Status?.Trim().ToLowerInvariant() ?? UserStatus.Active;
        var isLocked = user.Status == UserStatus.Locked;

        if (normalizedStatus == UserStatus.Locked && !isLocked)
        {
            // Nhánh chuyển sang locked: khóa tài khoản ngay.
            user.Lock();
            return;
        }

        if (normalizedStatus != UserStatus.Locked && isLocked)
        {
            // Nhánh mở khóa: kích hoạt lại tài khoản từ trạng thái locked.
            user.Activate();
        }
    }

    /// <summary>
    /// Điều chỉnh số dư kim cương và vàng về mức mục tiêu.
    /// Luồng xử lý: tính delta từng currency rồi gọi hàm xử lý credit/debit tương ứng.
    /// </summary>
    private async Task AdjustBalancesAsync(User user, UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var deltaDiamond = request.DiamondBalance - user.DiamondBalance;
        await AdjustCurrencyAsync(
            user.Id,
            CurrencyType.Diamond,
            deltaDiamond,
            request.IdempotencyKey,
            cancellationToken);

        var deltaGold = request.GoldBalance - user.GoldBalance;
        await AdjustCurrencyAsync(
            user.Id,
            CurrencyType.Gold,
            deltaGold,
            request.IdempotencyKey,
            cancellationToken);
    }

    /// <summary>
    /// Điều chỉnh một currency theo delta (dương thì credit, âm thì debit).
    /// Luồng xử lý: bỏ qua delta 0, tính direction/amount/operation key, rẽ nhánh gọi credit hoặc debit.
    /// </summary>
    private async Task AdjustCurrencyAsync(
        Guid userId,
        string currency,
        long delta,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (delta == 0)
        {
            // Không đổi số dư thì không tạo giao dịch để tránh nhiễu lịch sử ví.
            return;
        }

        var isCredit = delta > 0;
        var amount = Math.Abs(delta);
        var direction = isCredit ? "credit" : "debit";
        // Tạo idempotency key theo currency + direction để chống ghi trùng cho từng nhánh điều chỉnh.
        var operationKey = $"admin_update_{currency[..1].ToLowerInvariant()}_{direction}_{idempotencyKey}";
        var description = $"Admin adjusted {currency.ToLowerInvariant()} balance ({delta:+#;-#;0})";

        if (isCredit)
        {
            // Nhánh tăng số dư: ghi giao dịch credit từ nguồn admin update.
            await _walletRepository.CreditAsync(
                userId: userId,
                currency: currency,
                type: TransactionType.AdminTopup,
                amount: amount,
                referenceSource: "Admin_Update_User",
                referenceId: idempotencyKey,
                description: description,
                idempotencyKey: operationKey,
                cancellationToken: cancellationToken);
            return;
        }

        // Nhánh giảm số dư: ghi giao dịch debit với cùng quy ước metadata/idempotency.
        await _walletRepository.DebitAsync(
            userId: userId,
            currency: currency,
            type: TransactionType.AdminTopup,
            amount: amount,
            referenceSource: "Admin_Update_User",
            referenceId: idempotencyKey,
            description: description,
            idempotencyKey: operationKey,
            cancellationToken: cancellationToken);
    }
}
