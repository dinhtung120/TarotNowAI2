namespace TarotNow.Domain.Entities;

// Phần hành vi streak của User: tăng chuỗi, đứt chuỗi, đóng băng và phục hồi streak.
public partial class User
{
    /// <summary>
    /// Tăng streak khi người dùng có hoạt động hợp lệ trong ngày nghiệp vụ.
    /// Luồng xử lý: bỏ qua nếu đã ghi nhận cùng ngày, ngược lại tăng CurrentStreak và cập nhật LastStreakDate.
    /// </summary>
    public void IncrementStreak(DateOnly businessDate)
    {
        if (LastStreakDate.HasValue && LastStreakDate.Value == businessDate)
        {
            // Edge case: cùng ngày chỉ được tính một lần để tránh cộng streak trùng.
            return;
        }

        CurrentStreak++;
        LastStreakDate = businessDate;
        // Cập nhật state streak theo ngày nghiệp vụ mới vừa ghi nhận.
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu streak bị đứt và lưu lại giá trị trước khi đứt để hỗ trợ cơ chế đóng băng.
    /// Luồng xử lý: nếu streak đang 0 thì bỏ qua, còn lại chuyển CurrentStreak sang PreBreakStreak rồi reset.
    /// </summary>
    public void BreakStreak()
    {
        if (CurrentStreak == 0)
        {
            // Không cần xử lý khi streak đã bằng 0.
            return;
        }

        PreBreakStreak = CurrentStreak;
        CurrentStreak = 0;
        // Chuyển streak hiện tại vào vùng tạm trước khi reset để có thể phục hồi nếu được phép.

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Tính hệ số nhân EXP theo số ngày streak hiện tại.
    /// Luồng xử lý: trả hệ số cơ bản 1.0 cộng thêm tỷ lệ CurrentStreak/100.
    /// </summary>
    public double GetStreakExpMultiplier()
    {
        return 1.0 + (CurrentStreak / 100.0);
    }

    /// <summary>
    /// Phục hồi streak đã đứt từ giá trị PreBreakStreak khi người dùng dùng cơ chế đóng băng.
    /// Luồng xử lý: kiểm tra có giá trị phục hồi, khôi phục CurrentStreak, reset vùng tạm và canh lại LastStreakDate.
    /// </summary>
    public void RestoreStreak()
    {
        if (PreBreakStreak <= 0)
        {
            // Business rule: chỉ cho phục hồi khi từng có streak bị đứt được lưu trước đó.
            throw new InvalidOperationException("Không có Streak cũ để đóng băng/phục hồi.");
        }

        CurrentStreak = PreBreakStreak;
        PreBreakStreak = 0;
        // Sau khi phục hồi thì xóa vùng tạm để tránh tái sử dụng nhiều lần.

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        LastStreakDate = today.AddDays(-1);
        // Đặt LastStreakDate = hôm qua để người dùng cần hoạt động hôm nay mới tăng streak tiếp.

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Tính giá đóng băng streak dựa trên độ dài streak trước khi đứt.
    /// Luồng xử lý: trả 0 nếu không có streak đứt, ngược lại lấy trần PreBreakStreak/10.
    /// </summary>
    public long CalculateFreezePrice()
    {
        if (PreBreakStreak == 0)
        {
            // Không có streak cần đóng băng nên không phát sinh chi phí.
            return 0;
        }

        return (long)Math.Ceiling(PreBreakStreak / 10.0);
    }
}
