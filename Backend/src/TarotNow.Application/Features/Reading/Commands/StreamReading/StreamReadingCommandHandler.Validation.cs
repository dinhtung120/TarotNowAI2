using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandExecutor
{
    /// <summary>
    /// Kiểm tra session hợp lệ trước khi stream.
    /// Luồng xử lý: tải session theo id, xác thực quyền sở hữu user và bảo đảm session đã reveal bài.
    /// </summary>
    private async Task<ReadingSession> ValidateSessionAsync(
        StreamReadingCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetByIdAsync(request.ReadingSessionId, cancellationToken);
        if (session is null)
        {
            // Edge case: session không tồn tại thì không thể stream.
            throw new NotFoundException("Reading session not found");
        }

        if (session.UserId != request.UserId.ToString())
        {
            // Chặn truy cập stream chéo session của user khác.
            throw new UnauthorizedAccessException("Session not found or access denied");
        }

        if (!session.IsCompleted)
        {
            // Business rule: phải reveal cards trước mới được gọi AI diễn giải.
            throw new BadRequestException("Cannot stream AI interpretation before revealing cards");
        }

        return session;
    }

    /// <summary>
    /// Kiểm tra quota AI theo ngày và số request đang chạy.
    /// Luồng xử lý: chặn khi vượt quota ngày hoặc vượt ngưỡng in-flight để bảo vệ tài nguyên hệ thống.
    /// </summary>
    private async Task EnsureQuotaAsync(Guid userId, CancellationToken cancellationToken)
    {
        var dailyCount = await _aiRequestRepo.GetDailyAiRequestCountAsync(userId, cancellationToken);
        if (dailyCount >= _dailyAiQuota)
        {
            // Chặn vượt quota ngày để kiểm soát chi phí và công bằng tài nguyên.
            throw new BadRequestException("Daily AI request quota exceeded");
        }

        var activeCount = await _aiRequestRepo.GetActiveAiRequestCountAsync(userId, cancellationToken);
        if (activeCount >= _inFlightAiCap)
        {
            // Chặn quá nhiều request đồng thời để tránh quá tải và race completion khó kiểm soát.
            throw new BadRequestException("Too many in-flight AI requests");
        }
    }

    /// <summary>
    /// Kiểm tra rate limit giữa các lần yêu cầu stream.
    /// Luồng xử lý: dùng cache key theo user để áp cửa sổ thời gian; không đạt điều kiện thì trả BadRequest.
    /// </summary>
    private async Task EnsureRateLimitAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rateLimitKey = $"ratelimit:{userId}:ai_interpret";
        var rateLimit = TimeSpan.FromSeconds(_readingRateLimitSeconds);
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, rateLimit, cancellationToken);
        if (isAllowed)
        {
            // Trong giới hạn cho phép thì tiếp tục xử lý stream.
            return;
        }

        // Vượt ngưỡng rate-limit thì buộc đợi để giảm spam request AI.
        throw new BadRequestException($"Vui lòng đợi {_readingRateLimitSeconds} giây giữa các lần yêu cầu AI giải bài.");
    }

    /// <summary>
    /// Parse session id về Guid để đồng nhất kiểu lưu trữ trên PostgreSQL.
    /// Luồng xử lý: validate định dạng Guid và chặn Guid.Empty để tránh truy vấn lệch kiểu.
    /// </summary>
    private static Guid ParseReadingSessionRefOrThrow(string sessionId)
    {
        if (Guid.TryParse(sessionId, out var parsed) && parsed != Guid.Empty)
        {
            return parsed;
        }

        // Session id không phải Guid thì không thể đối chiếu bảng ai_requests kiểu uuid.
        throw new BadRequestException("Reading session id không hợp lệ.");
    }

    /// <summary>
    /// Chuẩn hóa idempotency key cho request stream.
    /// Luồng xử lý: với request có charge thì key là bắt buộc; request miễn phí có thể bỏ trống.
    /// </summary>
    private static string? ResolveIdempotencyKeyForRequest(StreamReadingCommand request, long calculatedCost)
    {
        var normalized = request.IdempotencyKey?.Trim();
        if (calculatedCost > 0 && string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("Idempotency-Key is required for paid AI stream requests.");
        }

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
