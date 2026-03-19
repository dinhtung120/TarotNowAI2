/*
 * FILE: WalletRepository.cs
 * MỤC ĐÍCH: Repository quản lý ví tiền (PostgreSQL).
 *   ĐÂY LÀ FILE QUAN TRỌNG NHẤT — xử lý tất cả giao dịch tài chính.
 *
 *   CÁC LOẠI GIAO DỊCH:
 *   → CreditAsync: CỘNG tiền (nạp tiền, hoàn tiền, thưởng)
 *   → DebitAsync: TRỪ tiền (mua dịch vụ, phí đọc bài)
 *   → FreezeAsync: ĐÓNG BĂNG Diamond (escrow — giữ tiền trước khi dịch vụ hoàn tất)
 *   → ReleaseAsync: CHUYỂN Diamond đã đóng băng cho Reader (dịch vụ hoàn tất)
 *   → RefundAsync: HOÀN TRẢ Diamond đã đóng băng (dịch vụ thất bại)
 *   → ConsumeAsync: TIÊU HỦY Diamond đã đóng băng (dịch vụ hoàn tất, không cần receiver)
 *
 *   NGUYÊN TẮC TÀI CHÍNH:
 *   1. MỌI giao dịch đều ghi vào sổ cái (wallet_transactions) — audit trail
 *   2. FOR UPDATE lock: tránh race condition khi 2 request cùng lúc thay đổi số dư
 *   3. Idempotency key: chặn double-charge, nếu key trùng → skip (không thực hiện lại)
 *   4. Double-check idempotency: kiểm tra 2 lần (trước và sau FOR UPDATE) vì concurrent requests
 *   5. ACID transaction: Credit/Debit/Freeze/Release/Refund đều trong transaction
 *
 *   ESCROW FLOW (luồng giữ tiền):
 *   → Freeze: User → DiamondBalance giảm, FrozenBalance tăng (tiền bị "khóa")
 *   → Release: FrozenBalance giảm (payer), DiamondBalance tăng (receiver) — chuyển cho Reader
 *   → Refund: FrozenBalance giảm, DiamondBalance tăng (User) — hoàn trả lại cho User
 *   → Consume: FrozenBalance giảm (tiêu hủy, không ai nhận) — dịch vụ AI tự động
 */

using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IWalletRepository — giao dịch tài chính (PostgreSQL).
/// Mọi method đều theo pattern: FOR UPDATE → idempotency check → Domain method → ghi ledger.
/// </summary>
public class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WalletRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Helper: thực thi action trong transaction.
    /// Nếu đã có transaction từ trước (nested call) → tái sử dụng, không tạo mới.
    /// Nếu chưa có → tạo transaction mới với isolation level ReadCommitted.
    /// CreateExecutionStrategy: hỗ trợ retry khi connection tạm mất (EF Core resilience).
    /// </summary>
    private async Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            // Tái sử dụng transaction có sẵn (ví dụ: gọi từ ReadingSession handler)
            await action();
            return;
        }

        // Tạo transaction mới
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            await action();
            await transaction.CommitAsync(cancellationToken);
        });
    }

    /// <summary>Normalize idempotency key (trim whitespace, null nếu rỗng).</summary>
    private static string? NormalizeIdempotencyKey(string? idempotencyKey)
        => string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey.Trim();

    /// <summary>
    /// Kiểm tra idempotency key đã tồn tại chưa.
    /// Nếu đã có → giao dịch này đã xử lý → skip.
    /// </summary>
    private async Task<bool> ExistsByIdempotencyKeyAsync(string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (idempotencyKey == null) return false;
        return await _dbContext.Set<WalletTransaction>().AnyAsync(t => t.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra lỗi DbUpdateException có phải là unique violation trên idempotency key không.
    /// Khi 2 request CÙNG LÚC gửi cùng idempotency key → 1 thành công, 1 lỗi unique index.
    /// Lỗi này = "đã xử lý" → catch và bỏ qua (không phải lỗi thật).
    /// </summary>
    private static bool IsIdempotencyUniqueViolation(DbUpdateException exception, string? idempotencyKey)
    {
        if (idempotencyKey == null) return false;

        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(postgresException.ConstraintName, "ix_wallet_transactions_idempotency_key", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// CỘNG tiền vào ví User (nạp tiền, hoàn tiền, thưởng).
    ///
    /// PATTERN CHUNG cho mọi giao dịch:
    /// 1. Normalize idempotency key
    /// 2. Kiểm tra idempotency (lần 1 — trước FOR UPDATE)
    /// 3. SELECT FOR UPDATE: khóa hàng User tránh race condition
    /// 4. Kiểm tra idempotency (lần 2 — sau FOR UPDATE, double-check)
    /// 5. Ghi nhận balanceBefore
    /// 6. Gọi Domain method (user.Credit/Debit/Freeze)
    /// 7. Ghi nhận balanceAfter
    /// 8. Tạo WalletTransaction (ledger entry) với balanceBefore/After
    /// 9. SaveChangesAsync trong transaction
    ///
    /// Tại sao kiểm tra idempotency 2 lần?
    /// → Lần 1: trước lock → nếu đã tồn tại → skip nhanh (tránh tốn lock)
    /// → Lần 2: sau lock → chắc chắn không bị race condition giữa 2 concurrent requests
    /// </summary>
    public async Task CreditAsync(Guid userId, string currency, string type, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                // Idempotency check lần 1 (trước lock — fast path)
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // FOR UPDATE: khóa hàng User → tránh race condition
                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                // Idempotency check lần 2 (sau lock — chắc chắn)
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // Ghi nhận số dư TRƯỚC khi thay đổi
                long balanceBefore = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                // Gọi Domain method: tăng số dư + validate business rules
                user.Credit(currency, amount, type);

                // Ghi nhận số dư SAU khi thay đổi
                long balanceAfter = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                // Tạo ledger entry — sổ cái ghi nhận giao dịch (audit trail)
                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: currency,
                    type: type,
                    amount: amount, // Số dương = cộng tiền
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate: unique index đã chặn → coi như đã xử lý, bỏ qua
        }
    }

    /// <summary>
    /// TRỪ tiền từ ví User (mua dịch vụ, phí đọc bài).
    /// Pattern giống CreditAsync nhưng:
    /// → amount lưu SỐ ÂM trong ledger (-amount) để phân biệt debit vs credit
    /// → Domain method user.Debit() kiểm tra: balance ≥ amount (không cho âm)
    /// </summary>
    public async Task DebitAsync(Guid userId, string currency, string type, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                // Debit: giảm số dư (throw nếu balance < amount)
                user.Debit(currency, amount);

                long balanceAfter = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: currency,
                    type: type,
                    amount: -amount, // SỐ ÂM = trừ tiền
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate → bỏ qua
        }
    }

    /// <summary>
    /// ĐÓNG BĂNG Diamond (escrow) — giữ tiền trước khi dịch vụ hoàn tất.
    /// DiamondBalance giảm, FrozenBalance tăng (tiền bị "khóa", không thể dùng).
    /// Khi nào: User hỏi Reader, AI streaming chưa xong → freeze tiền trước.
    /// Nếu dịch vụ OK → Release/Consume. Nếu fail → Refund.
    /// </summary>
    public async Task FreezeAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                // FreezeDiamond: DiamondBalance -= amount, FrozenBalance += amount
                user.FreezeDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowFreeze,
                    amount: -amount, // Âm = tiền bị "khóa" khỏi balance
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate → bỏ qua
        }
    }

    /// <summary>
    /// CHUYỂN TIỀN đã đóng băng cho Reader (dịch vụ hoàn tất thành công).
    /// Payer: FrozenBalance giảm (tiền ra khỏi escrow).
    /// Receiver: DiamondBalance tăng (Reader nhận tiền).
    /// Tạo 2 ledger entries (1 cho payer, 1 cho receiver) trong CÙNG 1 transaction.
    /// Receiver có idempotency key riêng (thêm suffix "_receiver") → chặn double-pay cả 2 chiều.
    /// </summary>
    public async Task ReleaseAsync(Guid payerId, Guid receiverId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);
        // Tạo idempotency key riêng cho receiver → tránh conflict nếu payer và receiver trùng ID
        var receiverIdempotencyKey = normalizedIdempotencyKey == null ? null : $"{normalizedIdempotencyKey}_receiver";

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // Lock CẢ 2 user (payer + receiver) cùng lúc trong 1 transaction
                var payers = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", payerId).ToListAsync(cancellationToken);
                var payer = payers.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy payer {payerId}");

                var receivers = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", receiverId).ToListAsync(cancellationToken);
                var receiver = receivers.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy receiver {receiverId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // Payer: giải phóng tiền đã đóng băng
                long payerBalanceBefore = payer.DiamondBalance;
                payer.ReleaseFrozenDiamond(amount);
                long payerBalanceAfter = payer.DiamondBalance;

                // Receiver: nhận tiền vào balance
                long receiverBalanceBefore = receiver.DiamondBalance;
                receiver.Credit(CurrencyType.Diamond, amount, TransactionType.EscrowRelease);
                long receiverBalanceAfter = receiver.DiamondBalance;

                // Ledger entry cho Payer (tiền RA)
                var ledgerEntryPayer = WalletTransaction.Create(
                    userId: payerId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease,
                    amount: -amount, // Âm = tiền RA khỏi escrow
                    balanceBefore: payerBalanceBefore,
                    balanceAfter: payerBalanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                // Ledger entry cho Receiver (tiền VÀO)
                var ledgerEntryReceiver = WalletTransaction.Create(
                    userId: receiverId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease,
                    amount: amount, // Dương = tiền VÀO
                    balanceBefore: receiverBalanceBefore,
                    balanceAfter: receiverBalanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: receiverIdempotencyKey
                );

                // AddRange: thêm cả 2 entries trong 1 batch
                _dbContext.Set<WalletTransaction>().AddRange(ledgerEntryPayer, ledgerEntryReceiver);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate → bỏ qua
        }
    }

    /// <summary>
    /// HOÀN TRẢ Diamond đã đóng băng cho User (escrow refund).
    /// FrozenBalance giảm, DiamondBalance tăng → tiền quay lại ví User.
    /// Khi nào: AI stream timeout/fail, Reader không phản hồi, v.v.
    /// </summary>
    public async Task RefundAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                // RefundFrozenDiamond: FrozenBalance -= amount, DiamondBalance += amount
                user.RefundFrozenDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRefund,
                    amount: amount, // Dương = tiền quay lại (cộng vào balance)
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate → bỏ qua
        }
    }

    /// <summary>
    /// TIÊU HỦY Diamond đã đóng băng — trừ khỏi frozen balance.
    /// Khác với Release: KHÔNG CÓ RECEIVER (tiền "biến mất" = chi phí dịch vụ).
    /// Dùng khi: AI streaming hoàn tất thành công → "đốt" Diamond đã freeze.
    /// Tại sao không dùng Release? → Chưa có System Master Account → dùng Consume thay thế.
    /// Transaction: SELECT FOR UPDATE → ConsumeFrozenDiamond → ghi ledger.
    /// </summary>
    public async Task ConsumeAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // FOR UPDATE: khóa hàng User
                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                // ConsumeFrozenDiamond: FrozenBalance -= amount (tiền "bốc hơi")
                user.ConsumeFrozenDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease, // Dùng EscrowRelease cho audit trail
                    amount: -amount, // Âm = tiền RA (tiêu hủy)
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate → bỏ qua
        }
    }
}
