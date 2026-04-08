namespace TarotNow.Application.Common.Helpers;

// Nhóm hàm tiện ích tính toán thông tin hồ sơ từ ngày sinh.
public static class ProfileHelper
{
    // Giá trị cung hoàng đạo mặc định khi ngày sinh nằm trước mọi mốc boundary.
    private const string Capricorn = "Capricorn (Ma Kết)";

    // Danh sách mốc chuyển cung theo định dạng MMDD, duyệt từ cuối để tối ưu so sánh.
    private static readonly (int Boundary, string Zodiac)[] ZodiacBoundaries =
    [
        (120, "Aquarius (Bảo Bình)"),
        (219, "Pisces (Song Ngư)"),
        (321, "Aries (Bạch Dương)"),
        (420, "Taurus (Kim Ngưu)"),
        (521, "Gemini (Song Tử)"),
        (622, "Cancer (Cự Giải)"),
        (723, "Leo (Sư Tử)"),
        (823, "Virgo (Xử Nữ)"),
        (923, "Libra (Thiên Bình)"),
        (1024, "Scorpio (Bọ Cạp)"),
        (1122, "Sagittarius (Nhân Mã)"),
        (1222, Capricorn)
    ];

    /// <summary>
    /// Tính cung hoàng đạo từ ngày sinh.
    /// Luồng xử lý: chuyển ngày sinh về mã MMDD, duyệt ngược mốc boundary để lấy cung đầu tiên thỏa điều kiện.
    /// </summary>
    public static string CalculateZodiac(DateTime dob)
    {
        var dayCode = dob.Month * 100 + dob.Day;

        for (var index = ZodiacBoundaries.Length - 1; index >= 0; index--)
        {
            var (boundary, zodiac) = ZodiacBoundaries[index];
            if (dayCode >= boundary)
            {
                // Nhánh match đầu tiên khi duyệt ngược sẽ là cung chính xác cho ngày sinh hiện tại.
                return zodiac;
            }
        }

        // Edge case ngày sinh nhỏ hơn mốc đầu tiên: trả Capricorn theo quy ước boundary.
        return Capricorn;
    }

    /// <summary>
    /// Tính con số thần số học từ ngày sinh theo quy tắc cộng các chữ số.
    /// Luồng xử lý: chuyển ngày sinh sang chuỗi yyyyMMdd, cộng từng chữ số, rồi rút gọn về số đích.
    /// </summary>
    public static int CalculateNumerology(DateTime dob)
    {
        var dateString = dob.ToString("yyyyMMdd");
        var sum = 0;

        foreach (var c in dateString)
        {
            if (char.IsDigit(c))
            {
                // Cộng dồn từng chữ số để tạo tổng ban đầu cho bước rút gọn numerology.
                sum += c - '0';
            }
        }

        return ReduceToSingleDigitOrMaster(sum);
    }

    /// <summary>
    /// Rút gọn số về một chữ số hoặc giữ nguyên các master number 11/22/33.
    /// Luồng xử lý: lặp cộng chữ số cho đến khi đạt điều kiện dừng theo quy tắc numerology.
    /// </summary>
    private static int ReduceToSingleDigitOrMaster(int number)
    {
        while (number > 9 && number != 11 && number != 22 && number != 33)
        {
            var tempSum = 0;
            var n = number;

            while (n > 0)
            {
                // Tách từng chữ số bằng phép chia lấy dư để cộng dồn nhanh, không cần ép sang chuỗi.
                tempSum += n % 10;
                n /= 10;
            }

            // Cập nhật state trung gian cho vòng rút gọn kế tiếp.
            number = tempSum;
        }

        return number;
    }
}
