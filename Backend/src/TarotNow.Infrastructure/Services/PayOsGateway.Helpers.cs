using System.Globalization;
using Net.payOS.Types;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public sealed partial class PayOsGateway
{
    private const string PayOsStatusCancelled = "CANCELLED";
    private const string PayOsStatusExpired = "EXPIRED";

    private List<ItemData> BuildPaymentItems(IReadOnlyList<PayOsPaymentItem> requestItems)
    {
        if (requestItems.Count == 0)
        {
            throw new BadRequestException("At least one payment item is required.");
        }

        return requestItems.Select(item =>
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new BadRequestException("Payment item name is required.");
            }

            if (item.Quantity <= 0)
            {
                throw new BadRequestException("Payment item quantity must be greater than zero.");
            }

            return new ItemData(
                item.Name.Trim(),
                item.Quantity,
                ToIntAmount(item.Price, nameof(item.Price)));
        }).ToList();
    }

    private string ResolveReturnUrl(string? requestUrl)
    {
        if (string.IsNullOrWhiteSpace(requestUrl) == false)
        {
            return requestUrl.Trim();
        }

        if (string.IsNullOrWhiteSpace(_depositOptions.ReturnUrl))
        {
            throw new InvalidOperationException("Deposit:ReturnUrl is not configured.");
        }

        return _depositOptions.ReturnUrl.Trim();
    }

    private string ResolveCancelUrl(string? requestUrl)
    {
        if (string.IsNullOrWhiteSpace(requestUrl) == false)
        {
            return requestUrl.Trim();
        }

        if (string.IsNullOrWhiteSpace(_depositOptions.CancelUrl))
        {
            throw new InvalidOperationException("Deposit:CancelUrl is not configured.");
        }

        return _depositOptions.CancelUrl.Trim();
    }

    private static bool ResolveWebhookSuccess(WebhookType webhookBody, WebhookData verifiedData)
    {
        if (!webhookBody.success)
        {
            return false;
        }

        return string.Equals(verifiedData.code, GatewaySuccessCode, StringComparison.OrdinalIgnoreCase)
               && string.Equals(webhookBody.code, GatewaySuccessCode, StringComparison.OrdinalIgnoreCase);
    }

    private static string? ResolveFailureReason(WebhookType webhookBody, WebhookData verifiedData)
    {
        if (ResolveWebhookSuccess(webhookBody, verifiedData))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(verifiedData.desc) == false)
        {
            return verifiedData.desc.Trim();
        }

        if (string.IsNullOrWhiteSpace(webhookBody.desc) == false)
        {
            return webhookBody.desc.Trim();
        }

        return DefaultFailureReason;
    }

    private static int ToIntAmount(long value, string parameterName)
    {
        if (value <= 0)
        {
            throw new BadRequestException($"{parameterName} must be greater than zero.");
        }

        if (value > int.MaxValue)
        {
            throw new BadRequestException($"{parameterName} exceeds supported PayOS amount.");
        }

        return (int)value;
    }

    private static DateTime? ToUtcDateTime(long? unixTimeSeconds)
    {
        if (unixTimeSeconds == null || unixTimeSeconds <= 0)
        {
            return null;
        }

        return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds.Value).UtcDateTime;
    }

    private static DateTime? ParsePayOsDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!DateTime.TryParseExact(
                value,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var parsed))
        {
            return null;
        }

        return DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
    }

    private static Transaction? ResolveLatestTransaction(IReadOnlyCollection<Transaction>? transactions)
    {
        if (transactions == null || transactions.Count == 0)
        {
            return null;
        }

        Transaction? latest = null;
        DateTime? latestAtUtc = null;

        foreach (var transaction in transactions)
        {
            var transactionAtUtc = ParsePayOsDateTime(transaction.transactionDateTime);
            if (transactionAtUtc == null)
            {
                latest ??= transaction;
                continue;
            }

            if (latestAtUtc == null || transactionAtUtc > latestAtUtc)
            {
                latest = transaction;
                latestAtUtc = transactionAtUtc;
            }
        }

        return latest;
    }

    private static string? ResolvePaymentLinkFailureReason(PaymentLinkInformation paymentInfo)
    {
        if (string.Equals(paymentInfo.status, PayOsStatusCancelled, StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(paymentInfo.cancellationReason)
                ? PayOsStatusCancelled
                : paymentInfo.cancellationReason;
        }

        if (string.Equals(paymentInfo.status, PayOsStatusExpired, StringComparison.OrdinalIgnoreCase))
        {
            return PayOsStatusExpired;
        }

        return null;
    }
}
