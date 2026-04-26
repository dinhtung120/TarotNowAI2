using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using System.Threading;
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
    private const string StatusPending = "PENDING";
    private const string StatusPaid = "PAID";
    private const string StatusCancelled = "CANCELLED";

    private readonly PayOS _payOs;
    private readonly ConcurrentDictionary<long, TestPaymentLinkState> _paymentLinks = new();
    private readonly ConcurrentDictionary<long, int> _createPaymentLinkCallCounts = new();
    private readonly ConcurrentDictionary<long, int> _perOrderFailureBudgets = new();
    private readonly ConcurrentDictionary<long, byte> _failedFirstAttemptOrders = new();
    private int _globalFailureBudget;
    private volatile bool _failFirstCreateAttemptPerOrderEnabled;

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
        _createPaymentLinkCallCounts.AddOrUpdate(request.OrderCode, 1, (_, current) => current + 1);
        if (ShouldInjectCreatePaymentLinkFailure(request.OrderCode))
        {
            throw new InvalidOperationException($"Injected transient provisioning failure for order {request.OrderCode}.");
        }

        var expiresAt = request.ExpiredAtUnix.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(request.ExpiredAtUnix.Value).UtcDateTime
            : (DateTime?)null;

        UpsertPaymentLinkState(
            orderCode: request.OrderCode,
            amount: request.Amount,
            paymentStatus: StatusPending,
            reference: null,
            transactionAtUtc: null,
            failureReason: null);

        return Task.FromResult(new PayOsCreatePaymentLinkResult
        {
            PaymentLinkId = $"plink_{request.OrderCode}",
            CheckoutUrl = $"https://payos.test/checkout/{request.OrderCode}",
            QrCode = $"PAYOS_QR_{request.OrderCode}",
            Status = "PENDING",
            ExpiresAtUtc = expiresAt
        });
    }

    /// <summary>
    /// Ép số lần thất bại kế tiếp cho mọi order (dùng cho fault-injection integration tests).
    /// </summary>
    public void FailNextCreatePaymentLinkAttempts(int attempts)
    {
        Interlocked.Exchange(ref _globalFailureBudget, Math.Max(0, attempts));
    }

    /// <summary>
    /// Ép số lần thất bại kế tiếp cho một order cụ thể.
    /// </summary>
    public void FailNextCreatePaymentLinkAttemptsForOrder(long orderCode, int attempts)
    {
        if (attempts <= 0)
        {
            _perOrderFailureBudgets.TryRemove(orderCode, out _);
            return;
        }

        _perOrderFailureBudgets[orderCode] = attempts;
    }

    /// <summary>
    /// Lấy số lần CreatePaymentLink đã được gọi cho order.
    /// </summary>
    public int GetCreatePaymentLinkCallCount(long orderCode)
    {
        return _createPaymentLinkCallCounts.TryGetValue(orderCode, out var callCount)
            ? callCount
            : 0;
    }

    /// <summary>
    /// Bật/tắt chế độ fail lần create đầu tiên cho mỗi order code.
    /// </summary>
    public void SetFailFirstCreateAttemptPerOrder(bool enabled)
    {
        _failFirstCreateAttemptPerOrderEnabled = enabled;
        if (!enabled)
        {
            _failedFirstAttemptOrders.Clear();
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

        var transactionAtUtc = ParseTransactionTime(verified.transactionDateTime);
        var isSuccess = webhook.success
                        && string.Equals(verified.code, SuccessCode, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(webhook.code, SuccessCode, StringComparison.OrdinalIgnoreCase);
        var failureReason = isSuccess || string.IsNullOrWhiteSpace(verified.desc)
            ? null
            : verified.desc.Trim();

        UpsertPaymentLinkState(
            orderCode: verified.orderCode,
            amount: verified.amount,
            paymentStatus: isSuccess ? StatusPaid : StatusCancelled,
            reference: verified.reference,
            transactionAtUtc: transactionAtUtc,
            failureReason: failureReason);

        return Task.FromResult(new PayOsVerifiedWebhookData
        {
            OrderCode = verified.orderCode,
            Amount = verified.amount,
            IsSuccess = isSuccess,
            Reference = verified.reference ?? string.Empty,
            PaymentLinkId = verified.paymentLinkId ?? string.Empty,
            GatewayCode = verified.code ?? string.Empty,
            FailureReason = failureReason,
            TransactionAtUtc = transactionAtUtc
        });
    }

    /// <inheritdoc />
    public Task<PayOsPaymentLinkInformation> GetPaymentLinkInformationAsync(
        long orderCode,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (orderCode <= 0)
        {
            throw new BadRequestException("Invalid PayOS order code.");
        }

        if (!_paymentLinks.TryGetValue(orderCode, out var state))
        {
            throw new BadRequestException("PayOS get payment link information failed: order not found in test gateway.");
        }

        return Task.FromResult(new PayOsPaymentLinkInformation
        {
            OrderCode = state.OrderCode,
            Amount = state.Amount,
            AmountPaid = string.Equals(state.PaymentStatus, StatusPaid, StringComparison.OrdinalIgnoreCase)
                ? state.Amount
                : 0,
            PaymentStatus = state.PaymentStatus,
            LatestReference = state.Reference,
            LatestTransactionAtUtc = state.TransactionAtUtc,
            FailureReason = state.FailureReason
        });
    }

    private void UpsertPaymentLinkState(
        long orderCode,
        long amount,
        string paymentStatus,
        string? reference,
        DateTime? transactionAtUtc,
        string? failureReason)
    {
        _paymentLinks.AddOrUpdate(
            orderCode,
            _ => new TestPaymentLinkState
            {
                OrderCode = orderCode,
                Amount = amount,
                PaymentStatus = paymentStatus,
                Reference = NormalizeOptional(reference),
                TransactionAtUtc = transactionAtUtc,
                FailureReason = NormalizeOptional(failureReason)
            },
            (_, existing) =>
            {
                existing.OrderCode = orderCode;
                existing.Amount = amount;
                existing.PaymentStatus = paymentStatus;
                existing.Reference = NormalizeOptional(reference);
                existing.TransactionAtUtc = transactionAtUtc;
                existing.FailureReason = NormalizeOptional(failureReason);
                return existing;
            });
    }

    private bool ShouldInjectCreatePaymentLinkFailure(long orderCode)
    {
        if (_failFirstCreateAttemptPerOrderEnabled && _failedFirstAttemptOrders.TryAdd(orderCode, 0))
        {
            return true;
        }

        if (_perOrderFailureBudgets.TryGetValue(orderCode, out var perOrderBudget)
            && perOrderBudget > 0)
        {
            if (_perOrderFailureBudgets.TryUpdate(orderCode, perOrderBudget - 1, perOrderBudget))
            {
                return true;
            }
        }

        while (true)
        {
            var current = Volatile.Read(ref _globalFailureBudget);
            if (current <= 0)
            {
                return false;
            }

            if (Interlocked.CompareExchange(ref _globalFailureBudget, current - 1, current) == current)
            {
                return true;
            }
        }
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
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

    private sealed class TestPaymentLinkState
    {
        public long OrderCode { get; set; }

        public long Amount { get; set; }

        public string PaymentStatus { get; set; } = StatusPending;

        public string? Reference { get; set; }

        public DateTime? TransactionAtUtc { get; set; }

        public string? FailureReason { get; set; }
    }
}
