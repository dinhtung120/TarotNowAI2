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
/// 3. Stream thất bại sau token đầu → Refund + quota rollback + update state
/// 4. Client disconnect sau token đầu → Chỉ update state (KHÔNG auto-refund)
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

    public CompleteAiStreamCommandHandler(
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo)
    {
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
    }

    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy AI Request record từ DB
        var record = await _aiRequestRepo.GetByIdAsync(request.AiRequestId, cancellationToken);
        if (record == null) return false;

        // 2. Cập nhật state machine
        record.Status = request.FinalStatus;
        record.FinishReason = request.ErrorMessage;
        record.UpdatedAt = DateTimeOffset.UtcNow;

        // Ghi timestamp completion nếu thành công
        if (request.FinalStatus == AiRequestStatus.Completed)
        {
            record.CompletionMarkerAt = DateTimeOffset.UtcNow;
        }

        // Ghi timestamp FirstToken nếu có
        if (request.FirstTokenAt.HasValue)
        {
            record.FirstTokenAt = request.FirstTokenAt;
        }

        await _aiRequestRepo.UpdateAsync(record, cancellationToken);

        // 3. Xử lý tài chính tùy theo trạng thái cuối
        switch (request.FinalStatus)
        {
            case AiRequestStatus.Completed:
                // Thành công → Consume (tiêu thụ) Diamond đã đóng băng
                // Dùng ConsumeAsync thay cho ReleaseAsync(Guid.Empty) cũ (fix BL-03)
                if (record.ChargeDiamond > 0)
                {
                    await _walletRepo.ConsumeAsync(
                        userId: request.UserId,
                        amount: record.ChargeDiamond,
                        referenceSource: "AiRequestCompletedConsume",
                        referenceId: record.Id.ToString(),
                        description: "Diamond consumed for completed AI Stream",
                        idempotencyKey: $"consume_{record.Id}",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case AiRequestStatus.FailedBeforeFirstToken:
            case AiRequestStatus.FailedAfterFirstToken:
                // Thất bại chính thức (không phải client disconnect sau token) → Refund + Quota Rollback
                if (!request.IsClientDisconnect)
                {
                    // Refund Diamond đã đóng băng
                    if (record.ChargeDiamond > 0)
                    {
                        await _walletRepo.RefundAsync(
                            userId: request.UserId,
                            amount: record.ChargeDiamond,
                            referenceSource: "AiRequestAutoRefund",
                            referenceId: record.Id.ToString(),
                            description: $"Auto Refund for aborting AI Streaming ({request.FinalStatus})",
                            idempotencyKey: $"refund_{record.Id}",
                            cancellationToken: cancellationToken
                        );
                    }

                    // BL-04 FIX: Rollback AI quota — giảm daily count vì request này không thành công.
                    // Spec yêu cầu: "failed_before_first_token → quota rollback 1" 
                    //                "failed_after_first_token → quota rollback 1"
                    // Cách rollback: Đánh dấu AiRequest này là "không tính vào quota" 
                    // bằng cách set RetryCount = -1 (sentinel value) để GetDailyAiRequestCountAsync exclude nó.
                    record.RetryCount = -1; // Sentinel: đánh dấu request đã bị rollback quota
                    await _aiRequestRepo.UpdateAsync(record, cancellationToken);
                }
                // Trường hợp client disconnect SAU token đầu → backend vẫn track, KHÔNG auto-refund (spec)
                break;
        }

        return true;
    }
}
