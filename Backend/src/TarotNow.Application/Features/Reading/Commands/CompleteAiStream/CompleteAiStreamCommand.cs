/*
 * ===================================================================
 * FILE: CompleteAiStreamCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.CompleteAiStream
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Khép Lại (Hoàn Tất Toán) Phiên Box Chat AI.
 *   Xử lý việc phân xử Trừ Tiền (Consume Escrow) hay Hoàn Đầu Kính Tiền (Refund Escrow)
 *   dựa vào việc AI trả lời Xuyên Suốt hay Gãy Kết Nối Giữa Dòng.
 * ===================================================================
 */

using MediatR;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public static class AiStreamFinalStatuses
{
    public static readonly string Completed = AiRequestStatus.Completed;
    public static readonly string FailedBeforeFirstToken = AiRequestStatus.FailedBeforeFirstToken;
    public static readonly string FailedAfterFirstToken = AiRequestStatus.FailedAfterFirstToken;

    public static bool IsSupported(string? status)
    {
        return string.Equals(status, Completed, StringComparison.Ordinal)
               || string.Equals(status, FailedBeforeFirstToken, StringComparison.Ordinal)
               || string.Equals(status, FailedAfterFirstToken, StringComparison.Ordinal);
    }
}

/// <summary>
/// Command để xử lý hoàn tất / thất bại của AI Stream.
/// Mục đích: Chuyển logic nghiệp vụ từ AiController sang Application layer,
/// đảm bảo Controller chỉ đóng vai trò "thin" theo Clean Architecture.
/// 
/// Command này xử lý 3 tình huống:
/// 1. Stream thành công (Completed) → Tịch Thu Tiền Giam giữ (Consume escrow).
/// 2. Stream thất bại trước khi AI mở miệng ngậm chữ (token đầu) → Trả lại tiền (Refund) + Trả quota.
/// 3. Stream thất bại lúc đang nhả chữ lai rai:
///    - Client ngắt mạng/rút dây mạng (Disconnect) → Vẫn móp túi trừ tiền (Consume escrow) chống bịa cớ chạy làng.
///    - Lỗi Error 500 từ ChatGPT (Server/provider) → Tuyên Lỗi do web, Refund Tiền Cho Khách.
/// </summary>
public class CompleteAiStreamCommand : IRequest<bool>
{
    /// <summary>Căn Cước của Record dòng AI chat log.</summary>
    public Guid AiRequestId { get; set; }

    /// <summary>Căn cước Của Kẻ Hao Tiền Nạp Card (UserId).</summary>
    public Guid UserId { get; set; }

    /// <summary>Chỉ chấp nhận 3 Trạng thái Của Cái Dòng Sinh Ra Bằng Enum: completed, failed_before_first_token, failed_after_first_token.</summary>
    public string FinalStatus { get; set; } = AiStreamFinalStatuses.Completed;

    /// <summary>Lý do Màn Hình Tắt Điện (Báo lỗi 429 Too many request, hoặc Null nếu Lướt Êm Thru).</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Cờ Gạt Tố Cáo: Client bứng dây mạng tự nguyện (Disconnect) hay Server bưng bít lỗi?</summary>
    public bool IsClientDisconnect { get; set; }

    /// <summary>Camera Giám Sát Tốc Độ: Lưu lại ms Cỡ chừng nào AI nhả chữ đầu tiên (Track Latency UX).</summary>
    public DateTimeOffset? FirstTokenAt { get; set; }

    /// <summary>Số chunk/token stream server đã trả cho client.</summary>
    public int OutputTokens { get; set; }

    /// <summary>Độ trễ từ token đầu tới lúc stream kết thúc (ms).</summary>
    public int LatencyMs { get; set; }

    /// <summary>Văn bản hoàn chỉnh của AI (để lưu vào DB).</summary>
    public string? FullResponse { get; set; }

    /// <summary>Câu hỏi do user đặt ra (để lưu vào DB History).</summary>
    public string? FollowupQuestion { get; set; }
}

/// <summary>
/// Người Thực Thi Đóng Đóng Dấu Kết Án Cuối Cùng Của ChatAI.
/// Trừ Tiền (Consume), Trả Tiền (Refund) Gọi DB tại Đây.
/// </summary>
public class CompleteAiStreamCommandHandler : IRequestHandler<CompleteAiStreamCommand, bool>
{
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IAiProvider _aiProvider;
    private readonly IReadingSessionRepository _readingRepo;

    public CompleteAiStreamCommandHandler(
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator,
        IAiProvider aiProvider,
        IReadingSessionRepository readingRepo)
    {
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
        _aiProvider = aiProvider;
        _readingRepo = readingRepo;
    }

    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        if (!AiStreamFinalStatuses.IsSupported(request.FinalStatus))
        {
            return false;
        }

        var processed = false;
        string? sessionRef = null;
        string? requestId = null;
        string telemetryStatus = "failed";
        string? telemetryErrorCode = null;

        // Bắt Khung Transaction 2 Mảng: Update Bảng AI + Trừ Tiền Bảng Tài Khoản Cùng Lúc. Rớt mạng phải Hoàn nguyên Rollback cả 2.
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var record = await _aiRequestRepo.GetByIdAsync(request.AiRequestId, transactionCt);
            if (record == null) return; // Ma Trơi -> Lơ đi luôn.

            var now = DateTimeOffset.UtcNow;
            record.Status = request.FinalStatus;
            record.FinishReason = NormalizeFinishReason(request.ErrorMessage);
            record.UpdatedAt = now;

            // Chúc Mừng Sinh Nhật
            if (request.FinalStatus == AiStreamFinalStatuses.Completed)
            {
                record.CompletionMarkerAt = now;
            }

            // Đồng Hồ Đo Latency Hoạt Động Cực Nhọc.
            if (request.FirstTokenAt.HasValue)
            {
                record.FirstTokenAt = request.FirstTokenAt;
            }

            // Tòa Án Kinh Tế (Economics Escrow Logic)
            switch (request.FinalStatus)
            {
                case var status when status == AiStreamFinalStatuses.Completed: // Suôn Sẻ.
                    if (record.ChargeDiamond > 0)
                    {
                        await _walletRepo.ConsumeAsync(
                            userId: request.UserId,
                            amount: record.ChargeDiamond,
                            referenceSource: "AiRequestCompletedConsume",
                            referenceId: record.Id.ToString(),
                            description: "Diamond consumed for completed AI Stream",
                            idempotencyKey: $"consume_{record.Id}", // Khóa Đúp Idempotent (Chống Lỗi Click 2 Lần)
                            cancellationToken: transactionCt
                        );
                    }
                    break;

                case var status when status == AiStreamFinalStatuses.FailedBeforeFirstToken: // Chưa Ra Chữ Nào, Thằng ChatGPT Giãy Đành Đạch.
                    if (record.ChargeDiamond > 0)
                    {
                        // Hoàn Tiền X100%.
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

                case var status when status == AiStreamFinalStatuses.FailedAfterFirstToken: // Lạy Thánh, Ra Chữ Rồi Mà Nghẽn Cơ Mạch Đứt Chừng.
                    if (record.ChargeDiamond > 0)
                    {
                        if (request.IsClientDisconnect) // Do Khách Tắt Trình Duyệt Ngang Bất Ngờ (Anti Cheating Xù Tiền).
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
                            // Do Thằng Đô Ra Ê Mon ChatGPT Hư Thật Á -> Kính Lão Đắc Thọ, Thối Tiền Refund Cho Khách VIP.
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

            sessionRef = record.ReadingSessionRef;
            requestId = record.Id.ToString();
            telemetryStatus = request.FinalStatus == AiStreamFinalStatuses.Completed ? "completed" : "failed";
            telemetryErrorCode = telemetryStatus == "failed" ? request.ErrorMessage : null;

            // Đóng Hồ Sơ Cất Kho.
            await _aiRequestRepo.UpdateAsync(record, transactionCt);
            processed = true;
            
            // Xử lý lưu nội dung chữ của AI xuống MongoDB thông qua Cập nhật Session
            if (request.FinalStatus == AiStreamFinalStatuses.Completed && !string.IsNullOrWhiteSpace(request.FullResponse))
            {
                var session = await _readingRepo.GetByIdAsync(record.ReadingSessionRef, transactionCt);
                if (session != null)
                {
                    // Update AiSummary or Followups based on RequestType (InitialReading vs Followup)
                    // We can use the logic from record.IdempotencyKey or request type
                    var isFollowUp = record.IdempotencyKey != null && record.IdempotencyKey.Contains("ai_stream") 
                        && record.ChargeDiamond <= 5 && record.ChargeDiamond > 0;
                        
                    // If it's free, we might not differentiate easily if we don't know the exact Followup string, 
                    // but we can just append it to Followups if AiSummary is already set.
                    if (!string.IsNullOrEmpty(session.AiSummary))
                    {
                        var newFollowups = session.Followups.ToList();
                        newFollowups.Add(new ReadingFollowup 
                        { 
                            Question = string.IsNullOrWhiteSpace(request.FollowupQuestion) ? "Câu hỏi Follow-up" : request.FollowupQuestion, 
                            Answer = request.FullResponse 
                        });
                        
                        var updatedSession = TarotNow.Domain.Entities.ReadingSession.Rehydrate(
                            id: session.Id,
                            userId: session.UserId,
                            spreadType: session.SpreadType,
                            question: session.Question,
                            cardsDrawn: session.CardsDrawn,
                            currencyUsed: session.CurrencyUsed,
                            amountCharged: session.AmountCharged,
                            isCompleted: session.IsCompleted,
                            createdAt: session.CreatedAt,
                            completedAt: session.CompletedAt,
                            aiSummary: session.AiSummary,
                            followups: newFollowups
                        );
                        await _readingRepo.UpdateAsync(updatedSession, transactionCt);
                    }
                    else
                    {
                        var updatedSession = TarotNow.Domain.Entities.ReadingSession.Rehydrate(
                            id: session.Id,
                            userId: session.UserId,
                            spreadType: session.SpreadType,
                            question: session.Question,
                            cardsDrawn: session.CardsDrawn,
                            currencyUsed: session.CurrencyUsed,
                            amountCharged: session.AmountCharged,
                            isCompleted: session.IsCompleted,
                            createdAt: session.CreatedAt,
                            completedAt: session.CompletedAt,
                            aiSummary: request.FullResponse,
                            followups: session.Followups
                        );
                        await _readingRepo.UpdateAsync(updatedSession, transactionCt);
                    }
                }
            }
        }, cancellationToken);

        if (!processed || string.IsNullOrWhiteSpace(requestId))
        {
            return processed;
        }

        // Ghi telemetry theo best-effort; không rollback business transaction nếu Mongo/log provider gặp lỗi.
        try
        {
            await _aiProvider.LogRequestAsync(
                request.UserId,
                sessionRef,
                requestId,
                inputTokens: 0,
                outputTokens: request.OutputTokens,
                latencyMs: request.LatencyMs,
                status: telemetryStatus,
                errorCode: telemetryErrorCode);
        }
        catch
        {
            // no-op: telemetry errors must not alter financial/business outcomes.
        }

        return processed;
    }

    // Dao Tẩu Rác Của Thằng ChatGPT Trả Về Lấy 50 Chữ Đầu, Coi Nó Trả Dài Choán Bảng SQL Của Mình
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
