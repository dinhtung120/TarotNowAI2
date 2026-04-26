using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Tiêu thụ escrow đã giữ cho AI request.
    /// Luồng xử lý: gọi wallet consume với idempotency key cố định theo request id để tránh double-charge khi retry.
    /// </summary>
    private Task ConsumeEscrowAsync(
        Guid userId,
        AiRequest record,
        string referenceSource,
        CancellationToken cancellationToken)
    {
        return _walletRepo.ConsumeAsync(
            userId: userId,
            amount: record.ChargeDiamond,
            referenceSource: referenceSource,
            referenceId: record.Id.ToString(),
            description: "Diamond consumed for completed AI Stream",
            idempotencyKey: $"consume_{record.Id}",
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Hoàn trả escrow cho AI request.
    /// Luồng xử lý: gọi wallet refund với idempotency key cố định để đảm bảo retry không hoàn tiền trùng.
    /// </summary>
    private Task RefundEscrowAsync(
        Guid userId,
        AiRequest record,
        string description,
        CancellationToken cancellationToken)
    {
        return _walletRepo.RefundAsync(
            userId: userId,
            amount: record.ChargeDiamond,
            referenceSource: "AiRequestAutoRefund",
            referenceId: record.Id.ToString(),
            description: description,
            idempotencyKey: $"refund_{record.Id}",
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Publish domain event telemetry completion theo cơ chế best-effort.
    /// Luồng xử lý: phát event telemetry ra outbox; nếu publish lỗi thì chỉ log warning để không ảnh hưởng luồng nghiệp vụ chính.
    /// </summary>
    private async Task PublishTelemetryEventSafeAsync(
        CompleteAiStreamCommand request,
        CompletionContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await _domainEventPublisher.PublishAsync(
                new AiStreamCompletionTelemetryRequestedDomainEvent
                {
                    UserId = request.UserId,
                    AiRequestId = request.AiRequestId,
                    SessionId = context.SessionRef,
                    RequestId = context.RequestId,
                    InputTokens = request.InputTokens,
                    OutputTokens = request.OutputTokens,
                    LatencyMs = request.LatencyMs,
                    Status = context.TelemetryStatus,
                    ErrorCode = context.TelemetryErrorCode,
                    PromptVersion = context.PromptVersion
                },
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "AI completion telemetry failed for request {AiRequestId}, user {UserId}.",
                request.AiRequestId,
                request.UserId);
        }
    }

    /// <summary>
    /// Phát domain event hoàn tất billing cho reading.
    /// Luồng xử lý: chỉ phát event khi có charge thực tế để downstream xử lý analytics/reward không bị nhiễu.
    /// </summary>
    private async Task PublishReadingBillingEventAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        bool wasRefunded,
        CancellationToken cancellationToken)
    {
        if (record.ChargeDiamond <= 0)
        {
            // Không có phí thì bỏ qua event billing để tránh tạo bản ghi downstream không cần thiết.
            return;
        }

        var deltaAmount = wasRefunded ? record.ChargeDiamond : -record.ChargeDiamond;
        var changeType = wasRefunded
            ? TarotNow.Domain.Enums.TransactionType.EscrowRefund
            : TarotNow.Domain.Enums.TransactionType.EscrowRelease;
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = TarotNow.Domain.Enums.CurrencyType.Diamond,
                ChangeType = changeType,
                DeltaAmount = deltaAmount,
                ReferenceId = record.Id.ToString()
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new ReadingBillingCompletedDomainEvent
            {
                UserId = request.UserId,
                AiRequestId = record.Id,
                ReadingSessionRef = record.ReadingSessionRef.ToString("D"),
                ChargeDiamond = record.ChargeDiamond,
                FinalStatus = request.FinalStatus,
                WasRefunded = wasRefunded
            },
            cancellationToken);
    }

    /// <summary>
    /// Chuẩn hóa finish reason để lưu vào bản ghi request.
    /// Luồng xử lý: trim chuỗi lỗi và cắt ngắn tối đa 50 ký tự để tránh phình dữ liệu không kiểm soát.
    /// </summary>
    private static string? NormalizeFinishReason(string? finishReason)
    {
        if (string.IsNullOrWhiteSpace(finishReason))
        {
            // Edge case: lỗi rỗng/trắng thì chuẩn hóa về null.
            return null;
        }

        var normalized = finishReason.Trim();
        return normalized.Length <= 50 ? normalized : normalized[..50];
    }
}
