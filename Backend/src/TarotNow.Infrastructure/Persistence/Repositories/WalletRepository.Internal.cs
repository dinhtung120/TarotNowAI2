using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Helpers;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial chứa helper nội bộ cho luồng wallet.
public partial class WalletRepository
{
    /// <summary>
    /// Kiểm tra request đã được xử lý theo idempotency key chưa.
    /// Luồng xử lý: tái sử dụng ExistsByIdempotencyKeyAsync.
    /// </summary>
    private async Task<bool> TryHandleIdempotentAsync(string? idempotencyKey, CancellationToken cancellationToken)
        => await ExistsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);

    /// <summary>
    /// Lấy user theo chế độ FOR UPDATE để khóa hàng.
    /// Luồng xử lý: query raw SQL FOR UPDATE và ném lỗi khi user không tồn tại.
    /// </summary>
    private async Task<User> GetUserForUpdateAsync(Guid userId, string role, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Set<User>()
            .FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId)
            .ToListAsync(cancellationToken);

        return users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy {role} {userId}");
    }

    /// <summary>
    /// Tạo ledger entry từ request chuẩn hóa.
    /// Luồng xử lý: gọi factory WalletTransaction.Create để bảo toàn invariant domain transaction.
    /// </summary>
    private static WalletTransaction CreateWalletLedgerEntry(WalletLedgerEntryRequest request)
    {
        return WalletTransaction.Create(new WalletTransactionCreateRequest
        {
            UserId = request.UserId,
            Currency = request.Currency,
            Type = request.Type,
            Amount = request.Amount,
            BalanceBefore = request.BalanceBefore,
            BalanceAfter = request.BalanceAfter,
            ReferenceSource = request.ReferenceSource,
            ReferenceId = request.ReferenceId,
            Description = request.Description,
            MetadataJson = request.MetadataJson,
            IdempotencyKey = request.IdempotencyKey
        });
    }

    /// <summary>
    /// Ghi log thông tin khi request idempotent đã được xử lý trước đó.
    /// Luồng xử lý: log mức Information kèm operation, principal và key.
    /// </summary>
    private void LogIdempotencyAlreadyHandled(
        DbUpdateException exception,
        string operationName,
        Guid principalId,
        string? idempotencyKey)
    {
        _logger.LogInformation(
            exception,
            "[WalletRepository] Idempotency key already handled for {Operation}. PrincipalId={PrincipalId}, Key={IdempotencyKey}",
            operationName,
            principalId,
            idempotencyKey);
    }

    /// <summary>
    /// Cập nhật leaderboard chi tiêu khi phát sinh debit/release hợp lệ.
    /// Luồng xử lý: validate amount/currency, xác định track và tăng điểm cho daily/monthly/all.
    /// </summary>
    private async Task TrackSpendingToLeaderboardAsync(
        Guid userId,
        string currency,
        long amount,
        CancellationToken ct)
    {
        if (amount <= 0) return;
        // Edge case: amount không dương thì không có chi tiêu thực tế để cộng điểm.

        var normalizedCurrency = currency?.Trim().ToLowerInvariant();
        if (normalizedCurrency != CurrencyType.Gold && normalizedCurrency != CurrencyType.Diamond)
        {
            return;
            // Chỉ track hai currency được định nghĩa trong leaderboard chi tiêu.
        }

        try
        {
            var track = normalizedCurrency == CurrencyType.Gold ? "spent_gold" : "spent_diamond";
            var dailyKey = PeriodKeyHelper.GetPeriodKey("daily");
            var monthlyKey = PeriodKeyHelper.GetPeriodKey("monthly");

            _logger.LogInformation("[Gamification] Gửi {Amount} {Currency} lên BXH (User: {UserId}, Track: {Track}, Daily: {DailyKey})",
                amount, normalizedCurrency, userId, track, dailyKey);
            // Log rõ track và period để hỗ trợ debug chênh lệch điểm leaderboard.

            await _lbRepo.IncrementScoreAsync(userId, track, dailyKey, amount, ct);
            await _lbRepo.IncrementScoreAsync(userId, track, monthlyKey, amount, ct);
            await _lbRepo.IncrementScoreAsync(userId, track, "all", amount, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Gamification] Lỗi khi cập nhật bảng xếp hạng cho User {UserId}", userId);
            // Không throw lại để không làm fail luồng tài chính chính vì lỗi side-effect leaderboard.
        }
    }
}
