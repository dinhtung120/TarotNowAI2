namespace TarotNow.Domain.Entities;

public partial class User
{
    /// <summary>
    /// Hàm này được gọi khi User vừa hoàn tất một phiên rút bài hợp lệ (vd: AI đã giải nghĩa xong).
    /// Đây là hành động cốt lõi để nuôi dưỡng Daily Streak (chuỗi ngày truy cập).
    /// </summary>
    public void IncrementStreak(DateOnly businessDate)
    {
        // 1. Nếu hệ thống nhận thấy ngày rút bài trùng với ngày streak đã tính rồi
        // thì không cộng nữa (chống spam cộng nhiều streak trong 1 ngày).
        if (LastStreakDate.HasValue && LastStreakDate.Value == businessDate)
        {
            return;
        }

        // 2. Tăng streak lên mức mới.
        CurrentStreak++;
        LastStreakDate = businessDate;
        
        // Cập nhật mốc thời gian object
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Hàm này được hệ thống gọi khi phát hiện User đã quá 1 ngày Business không có hành động rút bài (bỏ lỡ streak).
    /// Chuỗi streak hiện tại (CurrentStreak) sẽ bị cắt ngang, chuyển thành PreBreakStreak để tạo cơ hội
    /// cứu vớt (mua Streak Freeze), đồng thời reset current streak.
    /// </summary>
    public void BreakStreak()
    {
        // Nếu đã 0 rồi thì bỏ qua luôn, khỏi mất công gán tới lui
        if (CurrentStreak == 0) return;

        // Lưu giữ streak trước khi đứt vào một "ngăn tủ" để nếu user chịu chi tiền thì ta cứu lại cho họ.
        PreBreakStreak = CurrentStreak;
        
        // Tuyệt tình đưa streak hiện tại về mo (0).
        CurrentStreak = 0;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Tính toán EXP Bonus (hệ số kinh nghiệm) dựa trên chuỗi ngày liên tục của người chơi.
    /// Người chơi càng kiên trì (streak cao), cấp độ nhanh thăng tiến.
    /// Công thức gốc: 1% kinh nghiệm cộng thêm cho mỗi ngày streak (VD 10 ngày -> +10% -> hệ số 1.1)
    /// </summary>
    public double GetStreakExpMultiplier()
    {
        // Hệ số = 1.0 (gốc) + % bonus (CurrentStreak / 100)
        return 1.0 + (CurrentStreak / 100.0);
    }

    /// <summary>
    /// Phục hồi lại streak sau khi người chơi vung tiền Kim Cương ra mua đặc quyền Freeze.
    /// Tích hợp cơ chế chuộc lại chuỗi ngày cày cuốc đã lỡ đánh rơi hôm qua.
    /// </summary>
    public void RestoreStreak()
    {
        if (PreBreakStreak <= 0)
        {
            throw new InvalidOperationException("Không có Streak cũ để đóng băng/phục hồi.");
        }

        // Đổ lại giá trị đã cất giấu vào CurrentStreak.
        CurrentStreak = PreBreakStreak;
        
        // Reset tủ chứa vì đã cứu rồi, không cho cứu chồng chéo.
        PreBreakStreak = 0;

        // Nếu họ vừa mới gãy streak hôm qua thì mua phục hồi xong, ta phải giả lập
        // như họ đã điểm danh hôm qua, tức là LastStreakDate cần lùi lại 1 ngày từ hôm nay.
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        LastStreakDate = today.AddDays(-1);

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Tính giá kim cương để mua chiếc phao cứu sinh (Streak Freeze) dựa trên Streak cũ đã làm mất.
    /// Càng gãy chuỗi lâu năm, giá càng chát búa.
    /// Công thức gốc: trần(PreBreakStreak / 10). Tức là 1-10 ngày = 1 Diamond, 11-20 = 2, v.v.
    /// </summary>
    public long CalculateFreezePrice()
    {
        if (PreBreakStreak == 0) return 0;
        
        // Hàm trần (Ceiling) tự nâng số thập phân lên bậc số nguyên cao nhất.
        return (long)Math.Ceiling(PreBreakStreak / 10.0);
    }
}
