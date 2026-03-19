/*
 * ===================================================================
 * FILE: TransactionType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cái Đống Nùi Bát Quái Enum Chứa Đủ Tả Pí Lù Loại Chữ Đẩy Database Wallet Transaction (Lý Do Cộng / Trừ Gì Rất Dài).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Đại Lộ Định Biến Dòng Lý Do Chảy Của Tiền Kim Cương / Gold Vào Ra (Đập Lỗ Nào Ghi Bút Nấy Để Không Bao Giờ Thất Lạc Tiền Khách).
/// Này Cũng Dùng UI Làm Icon Hay Nhóm Lọc Lại Giao Diện Filter Admin Report Chống Hacker Trộm Tiền Đảo Lệnh.
/// </summary>
public static class TransactionType
{
    public const string DailyCheckin = "daily_checkin";           // Điểm Danh Chạp Được Lương Gold Gốc.
    public const string RegisterBonus = "register_bonus";         // Chúc Mừng Lỗ Mới Khai Trương Tặng Gold Free.
    public const string ReferralInvited = "referral_invited";     // Xòe Chó Nời Bạn Tham Gia Tới Cả Group Hưởng Gốc.
    public const string ReferralSuccess = "referral_success";     // Móc Thằng Kia Nạp Thẻ Bố Đc Thưởng Khuyên Hoa Hồng.
    public const string AchievementReward = "achievement_reward"; // Đập Đồ Đạt Được Danh Hiệu Game "Bốc 10 Lá Lần Đầu Tiên".
    public const string QuestReward = "quest_reward";             // Rút Cuốc Lấy Tí Nhiệm Vụ Hoàn Thành Đốt Bầu Tro.
    public const string ShareReward = "share_reward";             // Phím Về Nắm Fb Tường Bắn Share Nhận Tiền Dẻ Quạt.
    public const string FriendChainReward = "friend_chain_reward";// Chuỗi Bạn Gù Kéo Cả Làng Vô Bào Gold Theo Lọc Liên Hoàn Băng.
    public const string ReadingRefund = "reading_refund";         // Bom Lỗi Mỏ Server Nổ Nghẽn Phải Trả Thạch Cũ AI Cho Nó Đỡ Kiện.
    public const string ReadingCostGold = "reading_cost_gold";    // Thụp Trừ Lãi Trải Bói Đồng Nghèo Thẻ AI. Bói Bảng Thường Xài 5 Gold Lá.
    public const string Deposit = "deposit";                      // Phun Ngon Nhất: Nạp VND Qua Stripe/Paypal Trả Lại Kim Cương Gốc Vô.
    public const string DepositBonus = "deposit_bonus";           // Bồi Đầu Quà Lãi Nâng Nạp Gắn Hũ First Nạp.
    public const string ReferralReward = "referral_reward";       // Ngâm Kiếm Reff Tụ Quần Đáo Góp Hốc. 
    public const string SubscriptionRefund = "subscription_refund";// Cục Hoàn Sub Quá Tháng Cãi Cọ Với Paypal Cắt Thẻ Trả (Trừ Đi Hoàn).
    public const string AdminTopup = "admin_topup";               // Đại Lãnh Đạo Nhỏ Mũ Trắng Khui Nạp Phạt (Cộng Thưởng Hack Của User Bug Lỗi Hoặc Trả Giải Quà Sự Kiện Livestream Tặng Mõm Ko Cần Nạp Tiền Túi Bịp).
    public const string EscrowRelease = "escrow_release";         // Nhả Đập Băng Túi Thầy Ăn Máu Nhận Diamond Chói Chế Chát.
    public const string ReadingCostDiamond = "reading_cost_diamond";// Thụt Nẹt Kim Cương Đi Trải Gốc Lõi Gắt AI. Bọn Sộp Xài Đắt Hỏi Nhìu Dữ.
    public const string ChatCostEscrow = "chat_cost_escrow";      // Ngừng Chat Cắt Diamond Túi Cầm Cố Sang Chờ.
    public const string Withdrawal = "withdrawal";                // Rút Thụt Về Dòng Lệnh Chuyển Ngân Hàng Kêu Kêu Thầy Đòi Lương Cuối Tháng Vnd.
    public const string Subscription = "subscription";            // Quẹt Cố Thẻ Mua Lệnh VIP Xài Cố Tháng Auto Kéo Bank Lặp.
    public const string FollowupCost = "followup_cost";           // Quất Câu Ép Thầy Giá Phạt Cửa (Câu Tiếp Cùng Lệnh).
    public const string GachaCost = "gacha_cost";                 // Nướng Trọc Tiền Cho Card Pack Ép Đồ Game Xoay May Rủi Bóc Lá Đẹp Trang Trí Avatar Thích Lè Thức Đốt Máu Nhanh Nhất (Gacha Sưu Tầm).
    public const string EscrowFreeze = "escrow_freeze";           // Níu Cục Đá Vô Ngăn Lạnh Từ Accept Báo. Giữ Đá Lát Không Phát Khác Nhả Tui Lại Phát Tiêm Code Thật Refund Cáo Từ (Đống Chat Trực Tiếp).
    public const string EscrowRefund = "escrow_refund";           // Oái Nghén Bom Nhảm Cháy Không Trả Lời Trả Cục Đá Quăng Về (Refnd Dải).
    public const string StreakFreezeCost = "streak_freeze_cost";  // Ngồi Phủ Khóa Nhựa Nuốt Cốt Đồ Đóng Lấy Phí Bọc Kim Để Níu Điểm Đăng Nhập Gọi (Tiền Xứ Cụ Án Điểm Đóng Băng Nào Của Game Duolingo Đang Code).
}
