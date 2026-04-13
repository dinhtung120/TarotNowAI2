using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Domain.Entities;

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

}
