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
    /// <summary>
    /// Nhận Cái Ngày Đẻ (Date Of Birth) Rút Ngược Coi Lọt Khe Nhánh Nào 12 Chòm Sao Cung Hoàng Đạo Tây Học.
    /// </summary>
    public static string CalculateZodiac(DateTime dob)
    {
        int month = dob.Month;
        int day = dob.Day;

        // Bập Vào Tháng Đẻ - Cứa Cột Ngày Giao Thoa Xem Trượt Sang Mép Chòm Mới Chưa.
        if (month == 1) return day >= 20 ? "Aquarius (Bảo Bình)" : "Capricorn (Ma Kết)";
        if (month == 2) return day >= 19 ? "Pisces (Song Ngư)" : "Aquarius (Bảo Bình)";
        if (month == 3) return day >= 21 ? "Aries (Bạch Dương)" : "Pisces (Song Ngư)";
        if (month == 4) return day >= 20 ? "Taurus (Kim Ngưu)" : "Aries (Bạch Dương)";
        if (month == 5) return day >= 21 ? "Gemini (Song Tử)" : "Taurus (Kim Ngưu)";
        if (month == 6) return day >= 22 ? "Cancer (Cự Giải)" : "Gemini (Song Tử)"; 
        if (month == 7) return day >= 23 ? "Leo (Sư Tử)" : "Cancer (Cự Giải)";
        if (month == 8) return day >= 23 ? "Virgo (Xử Nữ)" : "Leo (Sư Tử)";
        if (month == 9) return day >= 23 ? "Libra (Thiên Bình)" : "Virgo (Xử Nữ)";
        if (month == 10) return day >= 24 ? "Scorpio (Bọ Cạp)" : "Libra (Thiên Bình)";
        if (month == 11) return day >= 22 ? "Sagittarius (Nhân Mã)" : "Scorpio (Bọ Cạp)";
        if (month == 12) return day >= 22 ? "Capricorn (Ma Kết)" : "Sagittarius (Nhân Mã)";

        return "Unknown";
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
