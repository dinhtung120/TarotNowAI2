using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Partial chứa state transitions và invariant helpers của DepositOrder.
public partial class DepositOrder
{
    /// <summary>
    /// Đánh dấu payment link đã sẵn sàng sau khi worker tạo thành công ở gateway.
    /// </summary>
    public void MarkPaymentLinkReady(
        string paymentLinkId,
        string checkoutUrl,
        string qrCode,
        DateTime? expiresAtUtc,
        DateTime? provisionedAtUtc = null)
    {
        ValidateRequiredString(paymentLinkId, nameof(paymentLinkId));
        ValidateRequiredString(checkoutUrl, nameof(checkoutUrl));
        ValidateRequiredString(qrCode, nameof(qrCode));

        if (PaymentLinkStatus == DepositPaymentLinkStatus.Ready
            && string.Equals(PayOsPaymentLinkId, paymentLinkId.Trim(), StringComparison.Ordinal))
        {
            return;
        }

        var now = provisionedAtUtc ?? DateTime.UtcNow;
        PayOsPaymentLinkId = paymentLinkId.Trim();
        CheckoutUrl = checkoutUrl.Trim();
        QrCode = qrCode.Trim();
        ExpiresAtUtc = expiresAtUtc;
        PaymentLinkStatus = DepositPaymentLinkStatus.Ready;
        PaymentLinkFailureReason = null;
        PaymentLinkAttemptCount += 1;
        PaymentLinkLastAttemptAtUtc = now;
        PaymentLinkProvisionedAtUtc = now;
        UpdatedAt = now;
    }

    /// <summary>
    /// Đánh dấu provisioning payment link thất bại để phục vụ quan sát/reconcile.
    /// </summary>
    public void MarkPaymentLinkProvisionFailed(string? reason, DateTime? attemptedAtUtc = null)
    {
        var now = attemptedAtUtc ?? DateTime.UtcNow;
        PaymentLinkStatus = DepositPaymentLinkStatus.Failed;
        PaymentLinkFailureReason = NormalizeOptional(reason);
        PaymentLinkAttemptCount += 1;
        PaymentLinkLastAttemptAtUtc = now;
        UpdatedAt = now;
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thành công sau khi webhook PayOS hợp lệ.
    /// </summary>
    public void MarkAsSuccess(string transactionId, DateTime? processedAtUtc = null)
    {
        ValidateRequiredString(transactionId, nameof(transactionId));

        if (Status == DepositOrderStatus.Success)
        {
            EnsureTransactionConsistency(transactionId);
            return;
        }

        if (Status == DepositOrderStatus.Failed && WalletGrantedAtUtc.HasValue)
        {
            throw new InvalidOperationException("Cannot recover a failed order after wallet grant.");
        }

        Status = DepositOrderStatus.Success;
        TransactionId = transactionId.Trim();
        FailureReason = null;
        ProcessedAt = processedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thất bại khi webhook trả trạng thái không thành công.
    /// </summary>
    public void MarkAsFailed(string? reason, string? transactionId = null, DateTime? processedAtUtc = null)
    {
        if (Status == DepositOrderStatus.Success)
        {
            throw new InvalidOperationException("Cannot fail a successful order.");
        }

        Status = DepositOrderStatus.Failed;
        FailureReason = NormalizeOptional(reason);
        TransactionId = NormalizeOptional(transactionId);
        ProcessedAt = processedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu lệnh đã cấp ví để chống credit lặp.
    /// </summary>
    public void MarkWalletGranted(DateTime? grantedAtUtc = null)
    {
        if (WalletGrantedAtUtc.HasValue)
        {
            return;
        }

        WalletGrantedAtUtc = grantedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateMoney(long amountVnd, long baseDiamondAmount, long bonusGoldAmount)
    {
        if (amountVnd <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountVnd), "AmountVnd must be greater than zero.");
        }

        if (baseDiamondAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseDiamondAmount), "BaseDiamondAmount must be greater than zero.");
        }

        if (bonusGoldAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bonusGoldAmount), "BonusGoldAmount cannot be negative.");
        }
    }

    private static void ValidateRequiredString(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} is required.", parameterName);
        }
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private bool HasPaymentLinkDetails()
    {
        return string.IsNullOrWhiteSpace(PayOsPaymentLinkId) == false
               && string.IsNullOrWhiteSpace(CheckoutUrl) == false
               && string.IsNullOrWhiteSpace(QrCode) == false;
    }

    private void EnsureTransactionConsistency(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(TransactionId))
        {
            return;
        }

        if (!string.Equals(TransactionId, transactionId.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Processed order transaction id mismatch.");
        }
    }
}
