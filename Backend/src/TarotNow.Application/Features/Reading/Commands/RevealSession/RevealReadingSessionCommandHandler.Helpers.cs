using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public partial class RevealReadingSessionCommandHandler
{
    /// <summary>
    /// Tải và kiểm tra session trước khi reveal.
    /// Luồng xử lý: lấy session theo id, xác thực quyền sở hữu user và chặn reveal lặp cho session đã completed.
    /// </summary>
    private async Task<ReadingSession> GetSessionForRevealAsync(
        RevealReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException("Session not found");

        if (session.UserId != request.UserId.ToString())
        {
            // Business rule: chỉ chủ session mới được reveal để bảo vệ dữ liệu riêng tư.
            throw new UnauthorizedAccessException("Reading session not found or access denied");
        }

        if (session.IsCompleted)
        {
            // Edge case: session đã reveal trước đó thì không cho thao tác lại.
            throw new BadRequestException("This session has already been revealed");
        }

        return session;
    }

    /// <summary>
    /// Resolve số lá bài cần rút theo spread type.
    /// Luồng xử lý: map spread đã chuẩn hóa sang số lá tương ứng; giá trị ngoài danh mục sẽ ném lỗi.
    /// </summary>
    private static int ResolveCardsToDraw(string spreadType)
    {
        return spreadType switch
        {
            SpreadType.Daily1Card => 1,
            SpreadType.Spread3Cards => 3,
            SpreadType.Spread5Cards => 5,
            SpreadType.Spread10Cards => 10,
            _ => throw new BadRequestException($"Invalid spread type: {spreadType}")
        };
    }

    /// <summary>
    /// Resolve exp theo cấu hình nghiệp vụ phiên reveal.
    /// Luồng xử lý: spread thường dùng diamond được nhân exp cao hơn; daily1card hoặc currency khác giữ exp cơ bản.
    /// </summary>
    private static long ResolveExpToGrant(ReadingSession session)
    {
        var usesDiamond = string.Equals(
            session.CurrencyUsed,
            CurrencyType.Diamond,
            StringComparison.OrdinalIgnoreCase);

        return session.SpreadType != SpreadType.Daily1Card && usesDiamond ? 2 : 1;
    }

    /// <summary>
    /// Cập nhật bộ sưu tập thẻ của user sau khi rút bài.
    /// Luồng xử lý: duyệt từng card đã rút và upsert điểm exp tương ứng vào collection.
    /// </summary>
    private async Task UpdateCollectionAsync(
        Guid userId,
        IEnumerable<int> drawnCards,
        long expToGrant,
        CancellationToken cancellationToken)
    {
        foreach (var cardId in drawnCards)
        {
            await _collectionRepo.UpsertCardAsync(userId, cardId, expToGrant, cancellationToken);
            // Ghi nhận từng lá vào collection để đảm bảo không mất dữ liệu khi user rút nhiều thẻ cùng phiên.
        }
        // Kết thúc vòng lặp, toàn bộ thẻ đã được đồng bộ vào collection.
    }

    /// <summary>
    /// Cộng exp tổng cho user sau khi reveal.
    /// Luồng xử lý: tải user theo id, bỏ qua nếu không tồn tại, cộng exp và persist.
    /// </summary>
    private async Task ApplyUserExpAsync(
        Guid userId,
        long expAmount,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            // Edge case: user không tồn tại tại thời điểm cộng exp thì bỏ qua an toàn.
            return;
        }

        user.AddExp(expAmount);
        // Đổi state level/exp của user theo số exp nhận được từ lần reveal.

        await _userRepository.UpdateAsync(user, cancellationToken);
        // Persist thay đổi exp để các truy vấn profile/gamification phản ánh dữ liệu mới.
    }
}
