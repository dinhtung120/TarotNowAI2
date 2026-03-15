using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Value Object / Owned Entity quản lý tài chính của người dùng.
/// 
/// Tại sao tách riêng khỏi User?
/// - Nguyên tắc SRP (Single Responsibility Principle): User entity chỉ nên quản lý 
///   thông tin cá nhân + trạng thái tài khoản. Logic tài chính (credit, debit, freeze, 
///   refund, consume) là một "nhóm trách nhiệm" hoàn toàn khác.
/// - Dễ test: Có thể viết unit test cho logic tài chính mà không cần User context.
/// - Dễ mở rộng: Nếu thêm tiền tệ mới (VD: Star, Coin), chỉ sửa class này.
///
/// Cách hoạt động trong EF Core?
/// - Dùng Owned Entity pattern: `builder.OwnsOne(u => u.Wallet, ...)`
/// - Các cột wallet (gold_balance, diamond_balance, ...) vẫn nằm trong bảng `users`,
///   nhưng logic code được tách biệt rõ ràng.
/// </summary>
public class UserWallet
{
    /// <summary>
    /// Gold: tiền miễn phí — nhận qua điểm danh, hoạt động, đăng ký mới.
    /// Không thể nạp bằng tiền thật.
    /// </summary>
    public long GoldBalance { get; private set; } = 0;

    /// <summary>
    /// Diamond: tiền nạp — dùng để trả phí AI Reading, Follow-up.
    /// Tỷ giá mặc định: 1 Diamond = 1.000 VND.
    /// </summary>
    public long DiamondBalance { get; private set; } = 0;

    /// <summary>
    /// Diamond đang bị đóng băng trong Escrow.
    /// Khi user gọi AI stream → Diamond chuyển từ DiamondBalance sang FrozenDiamondBalance.
    /// Stream thành công → Consume (trừ hẳn). Stream thất bại → Refund (trả lại).
    /// </summary>
    public long FrozenDiamondBalance { get; private set; } = 0;

    /// <summary>
    /// Tổng Diamond đã nạp bằng tiền thật (chỉ tăng, không giảm).
    /// Dùng cho VIP tier calculation, analytics.
    /// </summary>
    public long TotalDiamondsPurchased { get; private set; } = 0;

    /// <summary>
    /// EF Core cần parameterless constructor cho Owned Entity.
    /// </summary>
    protected UserWallet() { }

    /// <summary>
    /// Constructor khởi tạo ví mới với số dư mặc định = 0.
    /// Được gọi khi tạo User mới.
    /// </summary>
    public static UserWallet CreateDefault() => new UserWallet();

    // ======================================================================
    // METHODS: Các thao tác tài chính
    // Mỗi method enforce business invariants TRƯỚC KHI thay đổi state,
    // đảm bảo ví luôn ở trạng thái hợp lệ dù xảy ra exception.
    // ======================================================================

    /// <summary>
    /// Cộng tiền vào ví (Gold hoặc Diamond).
    /// Nếu type là Deposit (nạp tiền thật) → tính thêm vào TotalDiamondsPurchased.
    /// 
    /// Tại sao cần tham số `type`?
    /// → Vì chỉ Deposit mới tăng TotalDiamondsPurchased (dùng cho VIP tier).
    ///   Các loại credit khác (bonus, refund, reward) không tính vào tổng nạp.
    /// </summary>
    public void Credit(string currency, long amount, string type)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền cộng vào phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            GoldBalance += amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            DiamondBalance += amount;
            // Chỉ Deposit (nạp tiền thật) mới tăng TotalDiamondsPurchased
            if (type == TransactionType.Deposit)
            {
                TotalDiamondsPurchased += amount;
            }
        }
    }

    /// <summary>
    /// Trừ tiền từ ví.
    /// 
    /// Tại sao throw exception thay vì return false?
    /// → Domain-Driven Design: exception thể hiện domain violation rõ ràng hơn,
    ///   Application layer sẽ catch và chuyển thành ProblemDetails response.
    /// </summary>
    public void Debit(string currency, long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền trừ đi phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            if (GoldBalance < amount)
                throw new InvalidOperationException("Số dư Gold không đủ.");
            GoldBalance -= amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            if (DiamondBalance < amount)
                throw new InvalidOperationException("Số dư Diamond không đủ.");
            DiamondBalance -= amount;
        }
    }

    /// <summary>
    /// Đóng băng Diamond — chuyển từ khả dụng sang Escrow.
    /// 
    /// Flow Escrow Pattern:
    /// 1. FreezeDiamond(amount) → Diamond chờ xử lý
    /// 2a. ConsumeFrozenDiamond(amount) → Dịch vụ thành công, Diamond bị "đốt"
    /// 2b. RefundFrozenDiamond(amount) → Dịch vụ thất bại, Diamond trả lại
    /// </summary>
    public void FreezeDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền đóng băng phải lớn hơn 0.", nameof(amount));

        if (DiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond không đủ để đóng băng.");

        DiamondBalance -= amount;
        FrozenDiamondBalance += amount;
    }

    /// <summary>
    /// Giải phóng Diamond đã đóng băng — trừ khỏi frozen balance (dùng trong Release 2-party).
    /// Lưu ý: Method này KHÔNG cộng vào DiamondBalance — receiver sẽ nhận qua Credit riêng.
    /// </summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền giải phóng phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để giải phóng.");

        FrozenDiamondBalance -= amount;
    }

    /// <summary>
    /// Hoàn trả Diamond — chuyển từ đóng băng về lại khả dụng.
    /// Dùng khi AI stream thất bại → user nhận lại Diamond.
    /// </summary>
    public void RefundFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền hoàn trả phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để hoàn trả.");

        FrozenDiamondBalance -= amount;
        DiamondBalance += amount;
    }

    /// <summary>
    /// Tiêu thụ Diamond đã đóng băng — trừ hẳn, không cộng cho ai.
    /// Dùng khi AI stream thành công → Diamond bị "đốt" (consumed).
    /// Thay thế cho Release pattern cần System Master Account.
    /// </summary>
    public void ConsumeFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền tiêu thụ phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để tiêu thụ.");

        FrozenDiamondBalance -= amount;
    }
}
