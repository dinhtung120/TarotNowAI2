using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Thực thi phần chốt completion trong transaction.
    /// Luồng xử lý: tải record AI request, cập nhật final state, xử lý settlement ví, persist dữ liệu liên quan và cập nhật context cho hậu xử lý.
    /// </summary>
    private async Task ProcessCompletionAsync(
        CompleteAiStreamCommand request,
        CompletionContext context,
        CancellationToken cancellationToken)
    {
        var record = await _aiRequestRepo.GetByIdAsync(request.AiRequestId, cancellationToken);
        if (record is null)
        {
            // Edge case: request id không tồn tại thì bỏ qua toàn bộ completion để giữ idempotent.
            return;
        }

        ApplyRecordFinalState(record, request);
        var settlementDecision = new AiStreamSettlementPolicy().Decide(request);
        request.SettlementReason = settlementDecision.Reason;
        var wasRefunded = await ApplyWalletSettlementAsync(request, record, settlementDecision, cancellationToken);
        // Chốt trạng thái record trước, sau đó xử lý settlement theo final status để đồng bộ billing.

        await _aiRequestRepo.UpdateAsync(record, cancellationToken);
        await UpdateReadingSessionContentAsync(request, record, cancellationToken);
        await PublishReadingBillingEventAsync(request, record, wasRefunded, cancellationToken);
        // Persist record/session và phát domain event trong cùng transaction để tránh lệch trạng thái.

        context.Processed = true;
        context.RequestId = record.Id.ToString();
        context.SessionRef = record.ReadingSessionRef.ToString("D");
        context.TelemetryStatus = request.FinalStatus == AiStreamFinalStatuses.Completed ? "completed" : "failed";
        context.TelemetryErrorCode = request.SettlementReason ?? (context.TelemetryStatus == "failed" ? request.ErrorMessage : null);
        context.PromptVersion = record.PromptVersion;
        // Ghi context phục vụ log telemetry và quyết định hậu xử lý ngoài transaction.
    }

    /// <summary>
    /// Áp dụng trạng thái kết thúc cho AI request record.
    /// Luồng xử lý: cập nhật status, finish reason, timestamp và mốc completion/first-token khi có.
    /// </summary>
    private static void ApplyRecordFinalState(AiRequest record, CompleteAiStreamCommand request)
    {
        var now = DateTimeOffset.UtcNow;
        record.Status = request.FinalStatus;
        record.FinishReason = NormalizeFinishReason(request.ErrorMessage);
        record.UpdatedAt = now;
        // Đồng bộ metadata kết thúc cho bản ghi request trước khi persist.

        if (request.FinalStatus == AiStreamFinalStatuses.Completed)
        {
            record.CompletionMarkerAt = now;
            // Chỉ ghi completion marker khi request hoàn tất thành công.
        }

        if (request.FirstTokenAt.HasValue)
        {
            record.FirstTokenAt = request.FirstTokenAt;
            // Cập nhật mốc first token nếu client truyền về để phục vụ metric latency chi tiết.
        }
    }

    /// <summary>
    /// Xử lý settlement ví theo trạng thái kết thúc stream.
    /// Luồng xử lý: không charge thì bỏ qua; completed/disconnect consume escrow; lỗi tương ứng refund theo chính sách.
    /// </summary>
    private async Task<bool> ApplyWalletSettlementAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        AiStreamSettlementDecision settlementDecision,
        CancellationToken cancellationToken)
    {
        if (record.ChargeDiamond <= 0)
        {
            // Không có khoản tạm giữ thì không cần consume/refund.
            return false;
        }

        if (settlementDecision.ShouldConsumeEscrow)
        {
            var referenceSource = request.FinalStatus == AiStreamFinalStatuses.Completed
                ? "AiRequestCompletedConsume"
                : "AiRequestDisconnectConsume";
            await ConsumeEscrowAsync(request.UserId, record, referenceSource, cancellationToken);
            return false;
        }

        var refundDescription = request.FinalStatus == AiStreamFinalStatuses.FailedBeforeFirstToken
            ? "Auto refund for AI stream failure before first token"
            : "Auto refund for AI stream failure after first token";
        await RefundEscrowAsync(request.UserId, record, refundDescription, cancellationToken);
        return true;
    }
}
