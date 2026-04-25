using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler
{
    /// <summary>
    /// Tạo bản ghi AI request cho lần stream hiện tại.
    /// Luồng xử lý: lấy số follow-up hiện tại, dựng aiRequest với metadata tương ứng và lưu persistence.
    /// </summary>
    private async Task<AiRequest> CreateAiRequestAsync(
        StreamReadingCommand request,
        Guid readingSessionRef,
        long calculatedCost,
        CancellationToken cancellationToken)
    {
        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(
            readingSessionRef,
            cancellationToken);

        var aiRequest = new AiRequest
        {
            UserId = request.UserId,
            ReadingSessionRef = readingSessionRef,
            FollowupSequence = ResolveFollowupSequence(request.FollowupQuestion, followUpCount),
            Status = AiRequestStatus.Requested,
            IdempotencyKey = $"ai_stream_{readingSessionRef:D}_{Guid.CreateVersion7():N}",
            PromptVersion = "v1.5",
            ChargeDiamond = calculatedCost
        };
        // Gắn idempotency key duy nhất cho mỗi request để hỗ trợ truy vết và xử lý retry an toàn.

        await _aiRequestRepo.AddAsync(aiRequest, cancellationToken);
        // Persist request trước khi mở stream để completion callback có bản ghi đối chiếu.

        return aiRequest;
    }

    /// <summary>
    /// Resolve chỉ số follow-up cho AI request.
    /// Luồng xử lý: request ban đầu trả null, follow-up trả số thứ tự tăng dần theo lịch sử session.
    /// </summary>
    private static short? ResolveFollowupSequence(string? followupQuestion, int followUpCount)
    {
        if (string.IsNullOrWhiteSpace(followupQuestion))
        {
            // Luồng initial reading không mang chỉ số follow-up.
            return null;
        }

        return (short)(followUpCount + 1);
    }
}
