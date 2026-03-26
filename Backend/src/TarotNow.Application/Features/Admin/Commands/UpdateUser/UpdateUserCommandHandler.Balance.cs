using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public partial class UpdateUserCommandHandler
{
    private static void UpdateRoleAndStatus(User user, UpdateUserCommand request)
    {
        var normalizedRole = request.Role?.Trim().ToLowerInvariant() ?? UserRole.User;
        if (user.Role != normalizedRole)
        {
            user.UpdateRole(normalizedRole);
        }

        var normalizedStatus = request.Status?.Trim().ToLowerInvariant() ?? UserStatus.Active;
        var isLocked = user.Status == UserStatus.Locked;

        if (normalizedStatus == UserStatus.Locked && !isLocked)
        {
            user.Lock();
            return;
        }

        if (normalizedStatus != UserStatus.Locked && isLocked)
        {
            user.Activate();
        }
    }

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

    private async Task AdjustCurrencyAsync(
        Guid userId,
        string currency,
        long delta,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (delta == 0)
        {
            return;
        }

        var isCredit = delta > 0;
        var amount = Math.Abs(delta);
        var direction = isCredit ? "credit" : "debit";
        var operationKey = $"admin_update_{currency[..1].ToLowerInvariant()}_{direction}_{idempotencyKey}";
        var description = $"Admin adjusted {currency.ToLowerInvariant()} balance ({delta:+#;-#;0})";

        if (isCredit)
        {
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
