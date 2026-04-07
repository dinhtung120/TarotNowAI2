using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Helpers;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    private async Task<bool> TryHandleIdempotentAsync(string? idempotencyKey, CancellationToken cancellationToken)
        => await ExistsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);

    private async Task<User> GetUserForUpdateAsync(Guid userId, string role, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Set<User>()
            .FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId)
            .ToListAsync(cancellationToken);

        return users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy {role} {userId}");
    }

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

        private async Task TrackSpendingToLeaderboardAsync(
        Guid userId, 
        string currency, 
        long amount, 
        CancellationToken ct)
    {
        if (amount <= 0) return;

        var normalizedCurrency = currency?.Trim().ToLowerInvariant();
        if (normalizedCurrency != CurrencyType.Gold && normalizedCurrency != CurrencyType.Diamond)
        {
            return;
        }

        try
        {
            var track = normalizedCurrency == CurrencyType.Gold ? "spent_gold" : "spent_diamond";
            var dailyKey = PeriodKeyHelper.GetPeriodKey("daily");
            var monthlyKey = PeriodKeyHelper.GetPeriodKey("monthly");

            
            _logger.LogInformation("[Gamification] Gửi {Amount} {Currency} lên BXH (User: {UserId}, Track: {Track}, Daily: {DailyKey})", 
                amount, normalizedCurrency, userId, track, dailyKey);

            await _lbRepo.IncrementScoreAsync(userId, track, dailyKey, amount, ct);
            await _lbRepo.IncrementScoreAsync(userId, track, monthlyKey, amount, ct);
            await _lbRepo.IncrementScoreAsync(userId, track, "all", amount, ct);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex, "[Gamification] Lỗi khi cập nhật bảng xếp hạng cho User {UserId}", userId);
        }
    }
}
