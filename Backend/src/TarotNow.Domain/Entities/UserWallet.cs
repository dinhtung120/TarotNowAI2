
using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

// Value object ví người dùng, chứa toàn bộ quy tắc cập nhật số dư Gold/Diamond và phần đóng băng.
public class UserWallet
{
    // Số dư Gold khả dụng.
    public long GoldBalance { get; private set; } = 0;

    // Số dư Diamond khả dụng.
    public long DiamondBalance { get; private set; } = 0;

    // Số Diamond đang đóng băng chờ xử lý.
    public long FrozenDiamondBalance { get; private set; } = 0;

    // Tổng Diamond tích lũy từ giao dịch nạp.
    public long TotalDiamondsPurchased { get; private set; } = 0;

    /// <summary>
    /// Constructor rỗng cho ORM hoặc factory nội bộ.
    /// Luồng xử lý: khởi tạo ví với toàn bộ số dư bằng 0.
    /// </summary>
    protected UserWallet() { }

    /// <summary>
    /// Tạo ví mặc định cho tài khoản mới.
    /// Luồng xử lý: trả về instance UserWallet sạch với số dư khởi tạo.
    /// </summary>
    public static UserWallet CreateDefault() => new UserWallet();

    /// <summary>
    /// Cộng tiền vào ví theo loại tiền và loại giao dịch.
    /// Luồng xử lý: validate amount, phân nhánh currency, cộng số dư tương ứng và cập nhật thống kê nạp Diamond.
    /// </summary>
    public void Credit(string currency, long amount, string type)
    {
        if (amount <= 0)
        {
            // Business rule: chặn credit giá trị không dương để tránh dữ liệu số dư sai.
            throw new ArgumentException("Số tiền cộng vào phải lớn hơn 0.", nameof(amount));
        }

        if (currency == CurrencyType.Gold)
        {
            GoldBalance += amount;
            // Nhánh Gold chỉ cập nhật GoldBalance.
        }
        else if (currency == CurrencyType.Diamond)
        {
            DiamondBalance += amount;

            if (type == TransactionType.Deposit)
            {
                TotalDiamondsPurchased += amount;
                // Chỉ giao dịch nạp mới tăng thống kê tổng Diamond đã mua.
            }
        }
        else
        {
            // Edge case: currency ngoài danh sách hỗ trợ phải dừng xử lý ngay.
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

    /// <summary>
    /// Trừ tiền khỏi ví theo loại tiền.
    /// Luồng xử lý: validate amount, kiểm tra đủ số dư theo currency, rồi cập nhật balance tương ứng.
    /// </summary>
    public void Debit(string currency, long amount)
    {
        if (amount <= 0)
        {
            // Chặn giá trị trừ không hợp lệ để bảo toàn tính đúng đắn của số dư.
            throw new ArgumentException("Số tiền trừ đi phải lớn hơn 0.", nameof(amount));
        }

        if (currency == CurrencyType.Gold)
        {
            if (GoldBalance < amount)
            {
                // Business rule: không cho phép âm số dư Gold.
                throw new InvalidOperationException("Số dư Gold không đủ.");
            }

            GoldBalance -= amount;
            // Cập nhật số dư Gold sau khi trừ thành công.
        }
        else if (currency == CurrencyType.Diamond)
        {
            if (DiamondBalance < amount)
            {
                // Business rule: không cho phép âm số dư Diamond.
                throw new InvalidOperationException("Số dư Diamond không đủ.");
            }

            DiamondBalance -= amount;
            // Cập nhật số dư Diamond sau khi trừ thành công.
        }
        else
        {
            // Currency không hợp lệ, dừng xử lý để tránh ghi sai ledger.
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

    /// <summary>
    /// Đóng băng Diamond khả dụng để giữ tiền chờ kết quả nghiệp vụ.
    /// Luồng xử lý: validate amount, kiểm tra số dư khả dụng, rồi chuyển từ DiamondBalance sang FrozenDiamondBalance.
    /// </summary>
    public void FreezeDiamond(long amount)
    {
        if (amount <= 0)
        {
            // Chặn freeze amount không dương vì không mang ý nghĩa nghiệp vụ.
            throw new ArgumentException("Số tiền đóng băng phải lớn hơn 0.", nameof(amount));
        }

        if (DiamondBalance < amount)
        {
            // Không đủ số dư khả dụng thì không được phép đưa vào trạng thái đóng băng.
            throw new InvalidOperationException("Số dư Diamond không đủ để đóng băng.");
        }

        DiamondBalance -= amount;
        FrozenDiamondBalance += amount;
        // Hoán chuyển tiền từ available sang frozen để bảo toàn tổng tài sản.
    }

    /// <summary>
    /// Giải phóng Diamond đóng băng khi release cho bên nhận.
    /// Luồng xử lý: validate amount, kiểm tra frozen đủ, rồi giảm FrozenDiamondBalance.
    /// </summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        if (amount <= 0)
        {
            // Chặn release amount không dương.
            throw new ArgumentException("Số tiền giải phóng phải lớn hơn 0.", nameof(amount));
        }

        if (FrozenDiamondBalance < amount)
        {
            // Edge case: frozen không đủ thì không thể release.
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để giải phóng.");
        }

        FrozenDiamondBalance -= amount;
        // Giảm frozen vì khoản này đã được chuyển ra ngoài ví người dùng.
    }

    /// <summary>
    /// Hoàn lại Diamond đóng băng về số dư khả dụng.
    /// Luồng xử lý: validate amount, kiểm tra frozen đủ, trừ frozen và cộng lại available.
    /// </summary>
    public void RefundFrozenDiamond(long amount)
    {
        if (amount <= 0)
        {
            // Chặn refund amount không hợp lệ.
            throw new ArgumentException("Số tiền hoàn trả phải lớn hơn 0.", nameof(amount));
        }

        if (FrozenDiamondBalance < amount)
        {
            // Không thể hoàn tiền vượt quá phần đang frozen.
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để hoàn trả.");
        }

        FrozenDiamondBalance -= amount;
        DiamondBalance += amount;
        // Trả lại số dư khả dụng do giao dịch bị hủy/không hoàn tất.
    }

    /// <summary>
    /// Tiêu thụ Diamond đang đóng băng khi giao dịch được xác nhận thành công.
    /// Luồng xử lý: validate amount, kiểm tra frozen đủ, rồi giảm FrozenDiamondBalance.
    /// </summary>
    public void ConsumeFrozenDiamond(long amount)
    {
        if (amount <= 0)
        {
            // Chặn consume amount không dương để bảo toàn dữ liệu.
            throw new ArgumentException("Số tiền tiêu thụ phải lớn hơn 0.", nameof(amount));
        }

        if (FrozenDiamondBalance < amount)
        {
            // Business rule: chỉ được consume trong phạm vi số dư frozen hiện có.
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để tiêu thụ.");
        }

        FrozenDiamondBalance -= amount;
        // Khoản frozen được chuyển thành chi phí thực tế nên chỉ cần giảm frozen balance.
    }
}
