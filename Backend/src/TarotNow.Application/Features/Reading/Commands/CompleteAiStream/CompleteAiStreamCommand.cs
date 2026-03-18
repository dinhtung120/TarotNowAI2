using MediatR;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

/// <summary>
/// Command để xử lý hoàn tất / thất bại của AI Stream.
/// Mục đích: Chuyển logic nghiệp vụ từ AiController sang Application layer,
/// đảm bảo Controller chỉ đóng vai trò "thin" theo Clean Architecture.
/// 
/// Command này xử lý 3 tình huống:
/// 1. Stream thành công (Completed) → Consume escrow + update state
/// 2. Stream thất bại trước token đầu → Refund + quota rollback + update state
/// 3. Stream thất bại sau token đầu:
///    - Client disconnect → Consume escrow
///    - Lỗi server/provider → Refund escrow
/// </summary>
public class CompleteAiStreamCommand : IRequest<bool>
{
    /// <summary>
    /// ID của AiRequest record cần cập nhật
    /// </summary>
    public Guid AiRequestId { get; set; }

    /// <summary>
    /// ID của user sở hữu request
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Trạng thái cuối cùng: completed, failed_before_first_token, failed_after_first_token
    /// </summary>
    public string FinalStatus { get; set; } = null!;

    /// <summary>
    /// Lý do kết thúc (null nếu thành công)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Đánh dấu liệu client tự ngắt kết nối (disconnect).
    /// Nếu true + có token → KHÔNG auto-refund (spec requirement).
    /// </summary>
    public bool IsClientDisconnect { get; set; }

    /// <summary>
    /// Timestamp nhận token đầu tiên từ AI Provider (để track latency)
    /// </summary>
    public DateTimeOffset? FirstTokenAt { get; set; }
}

/// <summary>
/// Handler xử lý hoàn tất AI Stream — nơi tập trung toàn bộ logic:
/// - State machine transitions (status, timestamps)
/// - Escrow consume (khi thành công)
/// - Escrow refund (khi thất bại)
/// - AI quota rollback (khi thất bại)
/// 
/// Trước đây logic này nằm trong AiController (vi phạm CA), nay chuyển về đúng tầng Application.
/// </summary>
public class CompleteAiStreamCommandHandler : IRequestHandler<CompleteAiStreamCommand, bool>
{
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public CompleteAiStreamCommandHandler(
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        var processed = false;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var record = await _aiRequestRepo.GetByIdAsync(request.AiRequestId, transactionCt);
            if (record == null) return;

            var now = DateTimeOffset.UtcNow;
            record.Status = request.FinalStatus;
            record.FinishReason = NormalizeFinishReason(request.ErrorMessage);
            record.UpdatedAt = now;

            if (request.FinalStatus == AiRequestStatus.Completed)
            {
                record.CompletionMarkerAt = now;
            }

            if (request.FirstTokenAt.HasValue)
            {
                record.FirstTokenAt = request.FirstTokenAt;
            }

            switch (request.FinalStatus)
            {
                case AiRequestStatus.Completed:
                    if (record.ChargeDiamond > 0)
                    {
                        await _walletRepo.ConsumeAsync(
                            userId: request.UserId,
                            amount: record.ChargeDiamond,
                            referenceSource: "AiRequestCompletedConsume",
                            referenceId: record.Id.ToString(),
                            description: "Diamond consumed for completed AI Stream",
                            idempotencyKey: $"consume_{record.Id}",
                            cancellationToken: transactionCt
                        );
                    }
                    break;

                case AiRequestStatus.FailedBeforeFirstToken:
                    if (record.ChargeDiamond > 0)
                    {
                        await _walletRepo.RefundAsync(
                            userId: request.UserId,
                            amount: record.ChargeDiamond,
                            referenceSource: "AiRequestAutoRefund",
                            referenceId: record.Id.ToString(),
                            description: "Auto refund for AI stream failure before first token",
                            idempotencyKey: $"refund_{record.Id}",
                            cancellationToken: transactionCt
                        );
                    }
                    break;

                case AiRequestStatus.FailedAfterFirstToken:
                    if (record.ChargeDiamond > 0)
                    {
                        if (request.IsClientDisconnect)
                        {
                            await _walletRepo.ConsumeAsync(
                                userId: request.UserId,
                                amount: record.ChargeDiamond,
                                referenceSource: "AiRequestDisconnectConsume",
                                referenceId: record.Id.ToString(),
                                description: "Client disconnected after first token, consume escrow",
                                idempotencyKey: $"consume_{record.Id}",
                                cancellationToken: transactionCt
                            );
                        }
                        else
                        {
                            await _walletRepo.RefundAsync(
                                userId: request.UserId,
                                amount: record.ChargeDiamond,
                                referenceSource: "AiRequestAutoRefund",
                                referenceId: record.Id.ToString(),
                                description: "Auto refund for AI stream failure after first token",
                                idempotencyKey: $"refund_{record.Id}",
                                cancellationToken: transactionCt
                            );
                        }
                    }
                    break;

                default:
                    return;
            }

            await _aiRequestRepo.UpdateAsync(record, transactionCt);
            processed = true;
        }, cancellationToken);

        return processed;
    }

    private static string? NormalizeFinishReason(string? finishReason)
    {
        if (string.IsNullOrWhiteSpace(finishReason))
        {
            return null;
        }

        var normalized = finishReason.Trim();
        return normalized.Length <= 50 ? normalized : normalized[..50];
    }
}
