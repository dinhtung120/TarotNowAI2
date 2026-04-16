
namespace TarotNow.Domain.Enums;

// Tập hằng loại bút toán ví phục vụ phân loại giao dịch và đối soát ledger.
public static class TransactionType
{
    // Thưởng điểm danh hằng ngày.
    public const string DailyCheckin = "daily_checkin";

    // Thưởng chào mừng đăng ký mới.
    public const string RegisterBonus = "register_bonus";

    // Thưởng khi mời bạn bè thành công bước đầu.
    public const string ReferralInvited = "referral_invited";

    // Thưởng khi referral đạt điều kiện hoàn tất.
    public const string ReferralSuccess = "referral_success";

    // Thưởng từ thành tựu.
    public const string AchievementReward = "achievement_reward";

    // Thưởng từ nhiệm vụ.
    public const string QuestReward = "quest_reward";

    // Thưởng từ hành vi chia sẻ.
    public const string ShareReward = "share_reward";

    // Thưởng chuỗi referral bạn bè.
    public const string FriendChainReward = "friend_chain_reward";

    // Hoàn tiền lượt đọc bài.
    public const string ReadingRefund = "reading_refund";

    // Trừ Gold cho lượt đọc bài.
    public const string ReadingCostGold = "reading_cost_gold";

    // Nạp Diamond.
    public const string Deposit = "deposit";

    // Thưởng bonus khi nạp.
    public const string DepositBonus = "deposit_bonus";

    // Thưởng referral chung.
    public const string ReferralReward = "referral_reward";

    // Admin nạp thủ công cho tài khoản.
    public const string AdminTopup = "admin_topup";

    // Giải ngân tiền escrow.
    public const string EscrowRelease = "escrow_release";

    // Trừ Diamond cho lượt đọc bài.
    public const string ReadingCostDiamond = "reading_cost_diamond";

    // Đóng băng phí chat vào escrow.
    public const string ChatCostEscrow = "chat_cost_escrow";

    // Rút tiền từ ví.
    public const string Withdrawal = "withdrawal";

    // Hoàn tiền yêu cầu rút bị từ chối.
    public const string WithdrawalRefund = "withdrawal_refund";

    // Trừ phí câu hỏi follow-up.
    public const string FollowupCost = "followup_cost";

    // Trừ chi phí quay gacha.
    public const string GachaCost = "gacha_cost";

    // Nhận thưởng từ gacha.
    public const string GachaReward = "gacha_reward";

    // Thưởng vàng từ sử dụng item inventory.
    public const string InventoryReward = "inventory_reward";

    // Đóng băng tiền vào escrow.
    public const string EscrowFreeze = "escrow_freeze";

    // Hoàn tiền từ escrow.
    public const string EscrowRefund = "escrow_refund";

    // Trừ phí đóng băng streak.
    public const string StreakFreezeCost = "streak_freeze_cost";
}
