/*
 * ===================================================================
 * FILE: ProfileHelper.cs
 * NAMESPACE: TarotNow.Domain.Helpers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Công Cụ Phụ Trợ Nhỏ Tính Móng Cung Hoàng Đạo (Zodiac) Hoặc Số Mệnh Chủ (Numerology) Bằng Cách Tính Ngày Tháng DB Nhập Ở Profile.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Helpers;

/// <summary>
/// Các Hàm Phạt Giải Cắm Static Không Sở Hữu Database.
/// Dùng Đứt Nẹt Ngay Các Dòng Chạy Đoán Ngày Tháng Ra Mệnh Cung Giúp Bọn Giao Diện Vẽ Biểu Đồ Ma Thuật (Tử Vi Trọn Đời).
/// </summary>
public static class ProfileHelper
{
    private const string Capricorn = "Capricorn (Ma Kết)";

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
    /// Nhận Cái Ngày Đẻ (Date Of Birth) Rút Ngược Coi Lọt Khe Nhánh Nào 12 Chòm Sao Cung Hoàng Đạo Tây Học.
    /// </summary>
    public static string CalculateZodiac(DateTime dob)
    {
        var dayCode = dob.Month * 100 + dob.Day;

        for (var index = ZodiacBoundaries.Length - 1; index >= 0; index--)
        {
            var (boundary, zodiac) = ZodiacBoundaries[index];
            if (dayCode >= boundary)
            {
                return zodiac;
            }
        }

        return Capricorn;
    }

    /// <summary>
    /// Cộng Trừ Nén Số Học Bản Thần Số (Numerology Của Pytago).
    /// Tính Tất Cả Con Số Trình Ngày Tháng Năm Sinh Cộng Lại Dính Một Khối, Trừ Về Duy Nhất 1 Con Số Đỉnh (Trừ Quái Số Master 11 22 33 Kẹp Giữ).
    /// Dùng Thổi Phập Bài Toán UI (Số Của Bạn Tuần Này Là 6).
    /// </summary>
    public static int CalculateNumerology(DateTime dob)
    {
        // Nhào Ra Sợi Dây Format "19980516" (Calculate sum of digits of dob)
        string dateString = dob.ToString("yyyyMMdd");
        int sum = 0;

        foreach (char c in dateString)
        {
            if (char.IsDigit(c))
            {
                // Ép Trừ 0 Tức Thành Cast Kiểu Int (Bóc Các Số 1+9+9+8+0+5+1+6).
                sum += c - '0';
            }
        }

        // Bóp Gọn Nó Trừ Co Lại Rỗng Xuống Nhánh Dưới Về 1 Ký Tự
        return ReduceToSingleDigitOrMaster(sum);
    }

    /// <summary>
    /// Lọc Trừ Tách Riêng Số Nếu Tổng Cao Chót Vót (Ví Dụ 38 Phát Thì Tách 3+8 = 11, Sau Đó Kiểm 11 Gặp Chốt Trùm Bỏ Không Cưa Xuống 2 Nữa Để Nguyên Thành Số Master Định Mệnh Chói Thượng Sâu).
    /// </summary>
    private static int ReduceToSingleDigitOrMaster(int number)
    {
        // Rơi Xuống Trên Mức Giữ Lưới Cho Master Numbers Đỉnh Vú Từng Bực Nút Cao Trùm
        while (number > 9 && number != 11 && number != 22 && number != 33)
        {
            int tempSum = 0;
            int n = number;
            while (n > 0)
            {
                // Chia 10 Lấy Dư Quét Mã Cho Cộng Phụ Số Rập Từng Số.
                tempSum += n % 10;
                n /= 10;
            }
            number = tempSum;
        }

        return number;
    }
}
