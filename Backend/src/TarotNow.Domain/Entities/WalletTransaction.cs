
using System;

namespace TarotNow.Domain.Entities;

// Entity bút toán ví để lưu vết thay đổi số dư và metadata đối soát cho từng giao dịch.
public class WalletTransaction
{
    // Định danh bút toán.
    public Guid Id { get; private set; }

    // Người dùng sở hữu bút toán.
    public Guid UserId { get; private set; }

    // Loại tiền của bút toán.
    public string Currency { get; private set; } = string.Empty;

    // Loại nghiệp vụ giao dịch.
    public string Type { get; private set; } = string.Empty;

    // Giá trị biến động số dư.
    public long Amount { get; private set; }

    // Số dư trước khi ghi bút toán.
    public long BalanceBefore { get; private set; }

    // Số dư sau khi ghi bút toán.
    public long BalanceAfter { get; private set; }

    // Số dư khả dụng trước giao dịch.
    public long AvailableBalanceBefore { get; private set; }

    // Số dư khả dụng sau giao dịch.
    public long AvailableBalanceAfter { get; private set; }

    // Số dư frozen trước giao dịch.
    public long FrozenBalanceBefore { get; private set; }

    // Số dư frozen sau giao dịch.
    public long FrozenBalanceAfter { get; private set; }

    // Nguồn nghiệp vụ phát sinh bút toán.
    public string? ReferenceSource { get; private set; }

    // Định danh bản ghi nguồn nghiệp vụ.
    public string? ReferenceId { get; private set; }

    // Mô tả bút toán.
    public string? Description { get; private set; }

    // Metadata mở rộng ở dạng JSON.
    public string? MetadataJson { get; private set; }

    // Khóa idempotency của bút toán.
    public string? IdempotencyKey { get; private set; }

    // Thời điểm tạo bút toán.
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu đã lưu.
    /// </summary>
    protected WalletTransaction() { }

    /// <summary>
    /// Constructor nội bộ tạo bút toán với đầy đủ dữ liệu đối soát.
    /// Luồng xử lý: gán snapshot số dư trước/sau cùng metadata nguồn và mốc tạo.
    /// </summary>
    private WalletTransaction(
        Guid userId,
        string currency,
        string type,
        long amount,
        long balanceBefore,
        long balanceAfter,
        long availableBalanceBefore,
        long availableBalanceAfter,
        long frozenBalanceBefore,
        long frozenBalanceAfter,
        string? referenceSource,
        string? referenceId,
        string? description,
        string? metadataJson,
        string? idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Currency = currency;
        Type = type;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        AvailableBalanceBefore = availableBalanceBefore;
        AvailableBalanceAfter = availableBalanceAfter;
        FrozenBalanceBefore = frozenBalanceBefore;
        FrozenBalanceAfter = frozenBalanceAfter;
        ReferenceSource = referenceSource;
        ReferenceId = referenceId;
        Description = description;
        MetadataJson = metadataJson;
        IdempotencyKey = idempotencyKey;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory tạo bút toán ví từ request chuẩn hóa.
    /// Luồng xử lý: chuyển từng trường từ request vào constructor nội bộ và trả entity hoàn chỉnh.
    /// </summary>
    public static WalletTransaction Create(WalletTransactionCreateRequest request)
    {
        return new WalletTransaction(
            request.UserId,
            request.Currency,
            request.Type,
            request.Amount,
            request.BalanceBefore,
            request.BalanceAfter,
            request.AvailableBalanceBefore,
            request.AvailableBalanceAfter,
            request.FrozenBalanceBefore,
            request.FrozenBalanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            request.IdempotencyKey);
    }
}

// Request tạo bút toán ví, gom toàn bộ dữ liệu đầu vào phục vụ ghi ledger.
public sealed class WalletTransactionCreateRequest
{
    // Người dùng sở hữu bút toán.
    public Guid UserId { get; init; }

    // Loại tiền.
    public string Currency { get; init; } = string.Empty;

    // Loại giao dịch.
    public string Type { get; init; } = string.Empty;

    // Giá trị biến động.
    public long Amount { get; init; }

    // Số dư trước giao dịch.
    public long BalanceBefore { get; init; }

    // Số dư sau giao dịch.
    public long BalanceAfter { get; init; }

    // Số dư khả dụng trước giao dịch.
    public long AvailableBalanceBefore { get; init; }

    // Số dư khả dụng sau giao dịch.
    public long AvailableBalanceAfter { get; init; }

    // Số dư frozen trước giao dịch.
    public long FrozenBalanceBefore { get; init; }

    // Số dư frozen sau giao dịch.
    public long FrozenBalanceAfter { get; init; }

    // Nguồn nghiệp vụ.
    public string? ReferenceSource { get; init; }

    // Mã tham chiếu nguồn.
    public string? ReferenceId { get; init; }

    // Mô tả giao dịch.
    public string? Description { get; init; }

    // Metadata mở rộng.
    public string? MetadataJson { get; init; }

    // Khóa idempotency.
    public string? IdempotencyKey { get; init; }
}
