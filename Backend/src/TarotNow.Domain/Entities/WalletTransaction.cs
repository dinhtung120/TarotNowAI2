/*
 * ===================================================================
 * FILE: WalletTransaction.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Biên Bản Kế Toán Lành Tít Sổ Cái Gạch Tay (Ledger History Nhất SQL Không Xóa).
 *   Sử Dụng Ghi Tay Vết Đều Đặn Chữ Dò Mỗi Nhát Thở Đồng Coin Ra Vào Tụ Ở Cái Hệ Ví Dữ Dội Nọ Để Đảm Bảo Dữ Liệu Thanh Tra Auditor.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity Sổ Cái Rạch Lỗi Kế Toán Bảng `wallet_transactions`. Đứng Sừng Sững Với Chức Năng Rất Ác: "CHỈ IN VÀ XEM, CẤM UPDATE" (Append Only Pattern Lịch Sử Thay Đổi Đồng).
/// </summary>
public class WalletTransaction
{
    // Bút Tích Số Giao Dịch
    public Guid Id { get; private set; }
    // Chủ Của Quyển Sổ Này (Thằng Tích Có Trừ Ra).
    public Guid UserId { get; private set; }
    
    // Giao Bạc Đồng Nào Ghi Đi Nhé (Gold / Diamond).
    public string Currency { get; private set; } = string.Empty;
    // Đi Về Lối Nào: Nạp (Deposit), Giải Ngân Escrow (Release), Gọi Ai (Spend), Tặng Quà Bonus Lì Xì Tiên.
    public string Type { get; private set; } = string.Empty;
    
    // Khối Tích Phát Thu Hoặc Trừ Cũ (Bảng Gốc Tuyệt Đối Number).
    public long Amount { get; private set; }
    
    // Tiền Chốt Cũ Trước Khoảnh Khắc Bị Bắn Này Tồn Bao Mật Túi Đáy DB (So Ngay Vi Áp).
    public long BalanceBefore { get; private set; }
    // Khoảnh Khắc Nhấn Trừ Xong Trả Ra Ánh Sáng Cho App Đọc Ngay Cột Đếm (Audit Track Vạch Lộ Nét Nằm).
    public long BalanceAfter { get; private set; }
    
    // Trỏ Bông Nguồn Gốc Phía Nào Đi Nẻo Kẻ Ngờ Cụ Thể "Stripe Hook Payment", "Session AI", Đăng Kí Gift.
    public string? ReferenceSource { get; private set; }
    // Trỏ Id Biên Lai Móc Xích Màng Nhọn Tương Ứng Ví Cụ Lệnh Payment, Ids Session Rút Thẻ Nào Bị Cắn Tiền Khách Ngắm Khui Phán Giải Hoàn Cáo (Thúc Data).
    public string? ReferenceId { get; private set; }
    
    // Dòng Lệnh Chữ Rõ Cố Tích Nghĩa Admin Truy Lệnh "Thưởng Tiền Đầu Nạp Kêu Quá Đã Em Trai" Cho Hiện Giao Diện Mobile Xem Kiu Lịch Sử.
    public string? Description { get; private set; }
    // Bọng Túi Json Ngoại Bao Lót Nếu Thông Tin Kẹt Nhiều Mã Gói Lỗi (Đổ Nguyên Cục Data JSON Payload Log Để Đảo Nghịch Ngắt Debug Nhanh Cho DB Lưu Tĩnh).
    public string? MetadataJson { get; private set; }
    
    // Đống Trọng Mật Mã Bom Chống Cùng 1 Vụ Bắn 2 Lần Cột Phí Đồng Mã Quét Duplicate Bắn Vết Đã Vô Rùi Khỏi Thêm (Idempotency Key Không Thể So Nét Xào Nấu).
    public string? IdempotencyKey { get; private set; }
    
    // Thời Kì Đấu Khởi Gồm In Bản Liệt (Sẽ Giống Giới Timestamp Trên Database Create).
    public DateTime CreatedAt { get; private set; }

    protected WalletTransaction() { } // EF Core Gọi.

    /// <summary>Vòi In Sách DB Thuật Che Cắn Private Thẳng Tránh Bị Ném Lung Tung Gây Lệch Kế Toán Ra Ngoài Object Factory Đẻ Biên Cương Bên Dưới.</summary>
    private WalletTransaction(Guid userId, string currency, string type, long amount, long balanceBefore, long balanceAfter, string? referenceSource, string? referenceId, string? description, string? metadataJson, string? idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Currency = currency;
        Type = type;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        ReferenceSource = referenceSource;
        ReferenceId = referenceId;
        Description = description;
        MetadataJson = metadataJson;
        IdempotencyKey = idempotencyKey;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cổng Phát Tạo Tem Trả Ledger Mẫu Sẵn Thôi Cho Ngoài Repo Không Bị Nát Cần Dựng Đóng Mã An Toàn Có Bọc Tốt Trực Tiếp Tới Constructor Đang Kẹt Trong Lệnh Giới Hạn Mở (Factory Method Pattern).
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
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            request.IdempotencyKey);
    }
}

public sealed class WalletTransactionCreateRequest
{
    public Guid UserId { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public long Amount { get; init; }
    public long BalanceBefore { get; init; }
    public long BalanceAfter { get; init; }
    public string? ReferenceSource { get; init; }
    public string? ReferenceId { get; init; }
    public string? Description { get; init; }
    public string? MetadataJson { get; init; }
    public string? IdempotencyKey { get; init; }
}
