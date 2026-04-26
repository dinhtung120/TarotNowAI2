using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandExecutor
{
    /// <summary>
    /// Reserve slot theo user rồi tạo AI request trong critical section để tránh race quota/in-flight.
    /// </summary>
    private async Task<AiRequest> ReserveAndCreateAiRequestAsync(
        StreamReadingCommand request,
        Guid readingSessionRef,
        long calculatedCost,
        string? normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        var lockKey = $"ai-quota-reservation:{request.UserId:N}";
        var lockOwner = Guid.NewGuid().ToString("N");
        var acquired = await _cacheService.AcquireLockAsync(
            lockKey,
            lockOwner,
            leaseTime: TimeSpan.FromSeconds(_aiQuotaReservationLeaseSeconds),
            cancellationToken);
        if (!acquired)
        {
            throw new BadRequestException("Too many concurrent AI requests. Please retry shortly.");
        }

        try
        {
            await EnsureQuotaAsync(request.UserId, cancellationToken);

            AiRequest? persistedRequest = null;
            await _transactionCoordinator.ExecuteAsync(
                async transactionCt =>
                {
                    var aiRequest = await CreateAiRequestAsync(
                        request,
                        readingSessionRef,
                        calculatedCost,
                        normalizedIdempotencyKey,
                        transactionCt);
                    await FreezeEscrowAsync(request, aiRequest, calculatedCost, transactionCt);
                    persistedRequest = aiRequest;
                },
                cancellationToken);

            return persistedRequest
                   ?? throw new InvalidOperationException("Unable to persist AI request for streaming.");
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockOwner, cancellationToken);
        }
    }

    /// <summary>
    /// Tạo bản ghi AI request cho lần stream hiện tại.
    /// Luồng xử lý: lấy số follow-up hiện tại, dựng aiRequest với metadata tương ứng và lưu persistence.
    /// </summary>
    private async Task<AiRequest> CreateAiRequestAsync(
        StreamReadingCommand request,
        Guid readingSessionRef,
        long calculatedCost,
        string? normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(normalizedIdempotencyKey) == false)
        {
            var existing = await _aiRequestRepo.GetByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken);
            if (existing is not null)
            {
                if (existing.Status == AiRequestStatus.Requested)
                {
                    throw new BadRequestException("A request with this Idempotency-Key is still processing.");
                }

                throw new BadRequestException("This Idempotency-Key has already been used.");
            }
        }

        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(
            readingSessionRef,
            cancellationToken);

        var aiRequest = new AiRequest
        {
            UserId = request.UserId,
            ReadingSessionRef = readingSessionRef,
            FollowupSequence = ResolveFollowupSequence(request.FollowupQuestion, followUpCount),
            Status = AiRequestStatus.Requested,
            IdempotencyKey = normalizedIdempotencyKey ?? $"ai_stream_{readingSessionRef:D}_{Guid.CreateVersion7():N}",
            PromptVersion = _aiPromptVersion,
            ChargeDiamond = calculatedCost
        };
        // Gắn idempotency key duy nhất cho mỗi request để hỗ trợ truy vết và xử lý retry an toàn.

        await _aiRequestRepo.AddAsync(aiRequest, cancellationToken);
        // Persist request trong cùng transaction với freeze escrow để giữ nhất quán billing.
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
