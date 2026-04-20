using System.Text.Json;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Errors;
using Net.payOS.Types;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Adapter gọi SDK chính thức của PayOS.
/// </summary>
public sealed partial class PayOsGateway : IPayOsGateway
{
    private const string GatewaySuccessCode = "00";
    private const string DefaultFailureReason = "PAYOS_PAYMENT_FAILED";

    private readonly PayOS _payOs;
    private readonly DepositOptions _depositOptions;

    /// <summary>
    /// Khởi tạo gateway PayOS với thông tin xác thực từ options.
    /// </summary>
    public PayOsGateway(
        IOptions<PayOsOptions> payOsOptions,
        IOptions<DepositOptions> depositOptions)
    {
        var options = payOsOptions.Value;
        _depositOptions = depositOptions.Value;

        _payOs = new PayOS(
            options.ClientId,
            options.ApiKey,
            options.ChecksumKey,
            options.PartnerCode ?? string.Empty);
    }

    /// <inheritdoc />
    public async Task<PayOsCreatePaymentLinkResult> CreatePaymentLinkAsync(
        PayOsCreatePaymentLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var amount = ToIntAmount(request.Amount, nameof(request.Amount));
        var items = BuildPaymentItems(request.Items);
        var paymentData = new PaymentData(
            request.OrderCode,
            amount,
            request.Description,
            items,
            ResolveCancelUrl(request.CancelUrl),
            ResolveReturnUrl(request.ReturnUrl),
            expiredAt: request.ExpiredAtUnix);

        try
        {
            var result = await _payOs.createPaymentLink(paymentData);
            return new PayOsCreatePaymentLinkResult
            {
                PaymentLinkId = result.paymentLinkId,
                CheckoutUrl = result.checkoutUrl,
                QrCode = result.qrCode,
                Status = result.status ?? string.Empty,
                ExpiresAtUtc = ToUtcDateTime(result.expiredAt)
            };
        }
        catch (PayOSError ex)
        {
            throw new BadRequestException($"PayOS create payment link failed: {ex.Message}");
        }
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

        WebhookType webhookBody;
        try
        {
            webhookBody = JsonSerializer.Deserialize<WebhookType>(
                              rawPayload,
                              new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                          ?? throw new BadRequestException("Invalid PayOS webhook payload.");
        }
        catch (JsonException ex)
        {
            throw new BadRequestException($"Invalid PayOS webhook JSON: {ex.Message}");
        }

        WebhookData verifiedData;
        try
        {
            verifiedData = _payOs.verifyPaymentWebhookData(webhookBody);
        }
        catch (PayOSError ex)
        {
            throw new UnauthorizedAccessException($"Invalid PayOS webhook signature: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException($"Invalid PayOS webhook signature: {ex.Message}");
        }

        return Task.FromResult(new PayOsVerifiedWebhookData
        {
            OrderCode = verifiedData.orderCode,
            Amount = verifiedData.amount,
            IsSuccess = ResolveWebhookSuccess(webhookBody, verifiedData),
            Reference = verifiedData.reference ?? string.Empty,
            PaymentLinkId = verifiedData.paymentLinkId ?? string.Empty,
            GatewayCode = verifiedData.code ?? string.Empty,
            FailureReason = ResolveFailureReason(webhookBody, verifiedData),
            TransactionAtUtc = ParsePayOsDateTime(verifiedData.transactionDateTime)
        });
    }

}
