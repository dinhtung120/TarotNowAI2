using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandler
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
    /// Ghi telemetry completion theo cơ chế best-effort.
    /// Luồng xử lý: gửi log request sang provider log; nếu lỗi thì nuốt ngoại lệ để không ảnh hưởng luồng nghiệp vụ chính.
    /// </summary>
    private async Task LogTelemetrySafeAsync(CompleteAiStreamCommand request, CompletionContext context)
    {
        try
        {
            await _aiProvider.LogRequestAsync(new AiProviderRequestLog
            {
                UserId = request.UserId,
                SessionId = context.SessionRef,
                RequestId = context.RequestId,
                InputTokens = 0,
                OutputTokens = request.OutputTokens,
                LatencyMs = request.LatencyMs,
                Status = context.TelemetryStatus,
                ErrorCode = context.TelemetryErrorCode
            });
        }
        catch
        {
            // Telemetry lỗi không được làm fail completion vì đây là luồng quan sát, không phải nghiệp vụ cốt lõi.
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
                ReadingSessionRef = record.ReadingSessionRef ?? string.Empty,
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
