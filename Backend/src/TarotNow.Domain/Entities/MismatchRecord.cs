/*
 * ===================================================================
 * FILE: MismatchRecord.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Giám Sát Kế Toán Vi Phạm Tới Lầm Lộn Số Ví.
 *   Xác Mệnh So Đi Số Thật Ở Cache Balance Tới Sổ Cái Cộng Gộp Trừ Sai 1 Ly Đi Cả Phượng Hoàng.
 * ===================================================================
 */

namespace TarotNow.Domain.Entities;

/// <summary>
/// Mẫu Trạm Pháo Hiệu Soi Sai Phạm Ví Lủng Túi Lên Tiếng (Snapshot Báo Cáo Không Lưu Rẽ Gì Ngoài Cặp Mắc Lỗi Sai Sống Sực Giữa Túi Và Sự Thật).
/// Trực Giác Lôi Thằng Ledger (Trừ Tiền Khóa Chấn Thật SQL) Kéo Bám So Với Ví (Ví Đang Hiện UI) Thấy Lệch Cạch Nhạc Này Nhét Vô List Report Chém Admin Mò Lỗ Hổng Nâng Bù.
/// </summary>
public class MismatchRecord
{
    // Bắt Cổ Khách Lủng:
    public Guid UserId { get; set; }
    
    // UI App Của Cậu Hiện Rằng Cậu Vẫn To:
    public long UserGoldBalance { get; set; }
    // Kế Toán Ledger Lịch Sử Chốt Bảo Mày Vô Sản Lâu Rồi Rõ Ghi Cộng Lại Rút Sạch Mà:
    public long LedgerGoldBalance { get; set; }
    
    // Kim Cương Trong Đống Túi Chìa Ra Của Wallet Là:
    public long UserDiamondBalance { get; set; }
    // Kho Lịch Sử Rút Quẫy Rõ Ràng Cho Từng Lần Sổ Sách Kim Cương Nằm Ở Cấu:
    public long LedgerDiamondBalance { get; set; }
}
