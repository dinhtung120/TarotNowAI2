using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Cập nhật nội dung ReadingSession theo kết quả AI stream.
    /// Luồng xử lý: chỉ chạy khi completed + có full response, tải session hiện tại, build snapshot mới và persist.
    /// </summary>
    private async Task UpdateReadingSessionContentAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        CancellationToken cancellationToken)
    {
        if (request.FinalStatus != AiStreamFinalStatuses.Completed || string.IsNullOrWhiteSpace(request.FullResponse))
        {
            // Chỉ cập nhật nội dung phiên đọc khi stream hoàn tất thành công và có kết quả đầy đủ.
            return;
        }

        await _domainEventPublisher.PublishAsync(
            new ReadingSessionContentSyncRequestedDomainEvent
            {
                SessionId = record.ReadingSessionRef.ToString("D"),
                AiRequestId = request.AiRequestId,
                FollowupQuestion = request.FollowupQuestion,
                FullResponse = request.FullResponse!,
                OccurredAtUtc = DateTime.UtcNow
            },
            cancellationToken);
        // Chỉ publish yêu cầu sync để tránh trộn write-model PG và read-model Mongo trong cùng transaction.
    }

    /// <summary>
    /// Dựng session mới từ session hiện tại và dữ liệu stream hoàn tất.
    /// Luồng xử lý: nếu đã có summary thì append follow-up; nếu chưa có thì set full response làm summary đầu tiên.
    /// </summary>
    private static ReadingSession BuildUpdatedSession(
        ReadingSession session,
        CompleteAiStreamCommand request)
    {
        var aiRequestId = request.AiRequestId.ToString("D");
        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            if (session.Followups.Any(f => string.Equals(f.AiRequestId, aiRequestId, StringComparison.OrdinalIgnoreCase)))
            {
                // Callback completion lặp cho cùng request thì giữ nguyên session để tránh duplicate follow-up.
                return session;
            }

            var newFollowups = session.Followups.ToList();
            newFollowups.Add(new ReadingFollowup
            {
                Question = string.IsNullOrWhiteSpace(request.FollowupQuestion)
                    ? "Câu hỏi Follow-up"
                    : request.FollowupQuestion,
                Answer = request.FullResponse!,
                AiRequestId = aiRequestId
            });
            // Session đã có summary chính thì câu trả lời mới được ghi như follow-up để giữ timeline hội thoại.

            return RehydrateSession(session, session.AiSummary, newFollowups);
        }

        // Session chưa có summary thì dùng full response hiện tại làm summary khởi tạo.
        return RehydrateSession(session, request.FullResponse!, session.Followups);
    }

    /// <summary>
    /// Rehydrate session từ snapshot mới.
    /// Luồng xử lý: sao chép toàn bộ trường bất biến và thay phần nội dung cần cập nhật (AiSummary/Followups).
    /// </summary>
    private static ReadingSession RehydrateSession(
        ReadingSession session,
        string aiSummary,
        IEnumerable<ReadingFollowup> followups)
    {
        return ReadingSession.Rehydrate(new ReadingSessionSnapshot
        {
            Id = session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            CardsDrawn = session.CardsDrawn,
            CurrencyUsed = session.CurrencyUsed,
            AmountCharged = session.AmountCharged,
            IsCompleted = session.IsCompleted,
            CreatedAt = session.CreatedAt,
            CompletedAt = session.CompletedAt,
            AiSummary = aiSummary,
            Followups = followups.ToList()
        });
    }
}
