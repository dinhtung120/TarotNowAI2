using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Errors;
using Net.payOS.Types;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Test double cho PayOS gateway: không gọi mạng khi tạo payment link, nhưng vẫn verify chữ ký webhook bằng SDK.
/// </summary>
public sealed class TestPayOsGateway : IPayOsGateway
{
    private const string SuccessCode = "00";
    private readonly PayOS _payOs;

    /// <summary>
    /// Khởi tạo test gateway từ cấu hình PayOS.
    /// </summary>
    public TestPayOsGateway(IOptions<PayOsOptions> payOsOptions)
    {
        var options = payOsOptions.Value;
        _payOs = new PayOS(
            options.ClientId,
            options.ApiKey,
            options.ChecksumKey,
            options.PartnerCode ?? string.Empty);
    }

    /// <inheritdoc />
    public Task<PayOsCreatePaymentLinkResult> CreatePaymentLinkAsync(
        PayOsCreatePaymentLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var expiresAt = request.ExpiredAtUnix.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(request.ExpiredAtUnix.Value).UtcDateTime
            : (DateTime?)null;

        return Task.FromResult(new PayOsCreatePaymentLinkResult
        {
            PaymentLinkId = $"plink_{request.OrderCode}",
            CheckoutUrl = $"https://payos.test/checkout/{request.OrderCode}",
            QrCode = $"PAYOS_QR_{request.OrderCode}",
            Status = "PENDING",
            ExpiresAtUtc = expiresAt
        });
    }

    /// <inheritdoc />
    public Task<PayOsVerifiedWebhookData> VerifyWebhookAsync(
        string rawPayload,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(rawPayload))
        {
            throw new BadRequestException("Webhook payload is required.");
        }

        WebhookType webhook;
        try
        {
            webhook = JsonSerializer.Deserialize<WebhookType>(
                          rawPayload,
                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                      ?? throw new BadRequestException("Invalid PayOS webhook payload.");
        }
        catch (JsonException ex)
        {
            throw new BadRequestException($"Invalid PayOS webhook JSON: {ex.Message}");
        }

        WebhookData verified;
        try
        {
            verified = _payOs.verifyPaymentWebhookData(webhook);
        }
        catch (PayOSError ex)
        {
            throw new UnauthorizedAccessException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException(ex.Message);
        }

        return Task.FromResult(new PayOsVerifiedWebhookData
        {
            OrderCode = verified.orderCode,
            Amount = verified.amount,
            IsSuccess = webhook.success
                        && string.Equals(verified.code, SuccessCode, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(webhook.code, SuccessCode, StringComparison.OrdinalIgnoreCase),
            Reference = verified.reference ?? string.Empty,
            PaymentLinkId = verified.paymentLinkId ?? string.Empty,
            GatewayCode = verified.code ?? string.Empty,
            FailureReason = string.IsNullOrWhiteSpace(verified.desc) ? null : verified.desc.Trim(),
            TransactionAtUtc = ParseTransactionTime(verified.transactionDateTime)
        });
    }

    private static DateTime? ParseTransactionTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!DateTime.TryParseExact(
                value,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out var parsed))
        {
            return null;
        }

        return parsed.ToUniversalTime();
    }
}
