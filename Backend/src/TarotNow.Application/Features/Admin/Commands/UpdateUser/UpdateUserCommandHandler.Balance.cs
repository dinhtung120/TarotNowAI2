using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public partial class UpdateUserCommandHandlerRequestedDomainEventHandler
{
    private const string AdminReferenceSource = "Admin_Update_User";

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
        var operationKey = BuildOperationKey(currency, isCredit, idempotencyKey);
        var adjustment = new BalanceAdjustmentContext(
            userId,
            currency,
            amount,
            isCredit,
            idempotencyKey,
            operationKey,
            $"Admin adjusted {currency.ToLowerInvariant()} balance ({delta:+#;-#;0})");
        await ApplyBalanceChangeAsync(adjustment, cancellationToken);
        await PublishMoneyChangedAsync(userId, currency, isCredit ? amount : -amount, operationKey, cancellationToken);
    }

    private async Task ApplyBalanceChangeAsync(
        BalanceAdjustmentContext adjustment,
        CancellationToken cancellationToken)
    {
        if (adjustment.IsCredit)
        {
            await _walletRepository.CreditAsync(
                userId: adjustment.UserId,
                currency: adjustment.Currency,
                type: TransactionType.AdminAdjustmentCredit,
                amount: adjustment.Amount,
                referenceSource: AdminReferenceSource,
                referenceId: adjustment.ReferenceId,
                description: adjustment.Description,
                idempotencyKey: adjustment.OperationKey,
                cancellationToken: cancellationToken);
            return;
        }

        await _walletRepository.DebitAsync(
            userId: adjustment.UserId,
            currency: adjustment.Currency,
            type: TransactionType.AdminAdjustmentDebit,
            amount: adjustment.Amount,
            referenceSource: AdminReferenceSource,
            referenceId: adjustment.ReferenceId,
            description: adjustment.Description,
            idempotencyKey: adjustment.OperationKey,
            cancellationToken: cancellationToken);
    }

    private Task PublishMoneyChangedAsync(
        Guid userId,
        string currency,
        long deltaAmount,
        string referenceId,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = userId,
                Currency = currency,
                ChangeType = deltaAmount >= 0
                    ? TransactionType.AdminAdjustmentCredit
                    : TransactionType.AdminAdjustmentDebit,
                DeltaAmount = deltaAmount,
                ReferenceId = referenceId
            },
            cancellationToken);
    }

    private static string BuildOperationKey(string currency, bool isCredit, string idempotencyKey)
    {
        var direction = isCredit ? "credit" : "debit";
        return $"admin_update_{currency[..1].ToLowerInvariant()}_{direction}_{idempotencyKey}";
    }

    private sealed record BalanceAdjustmentContext(
        Guid UserId,
        string Currency,
        long Amount,
        bool IsCredit,
        string ReferenceId,
        string OperationKey,
        string Description);
}
