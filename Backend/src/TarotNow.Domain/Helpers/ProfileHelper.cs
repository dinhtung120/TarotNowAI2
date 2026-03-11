using System;

namespace TarotNow.Domain.Helpers;

public static class ProfileHelper
{
    public static string CalculateZodiac(DateTime dob)
    {
        int month = dob.Month;
        int day = dob.Day;

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

    public static int CalculateNumerology(DateTime dob)
    {
        // Calculate sum of digits of dob
        string dateString = dob.ToString("yyyyMMdd");
        int sum = 0;

        foreach (char c in dateString)
        {
            if (char.IsDigit(c))
            {
                sum += c - '0';
            }
        }

        // Reduce to a single digit or master number
        return ReduceToSingleDigitOrMaster(sum);
    }

    private static int ReduceToSingleDigitOrMaster(int number)
    {
        while (number > 9 && number != 11 && number != 22 && number != 33)
        {
            int tempSum = 0;
            int n = number;
            while (n > 0)
            {
                tempSum += n % 10;
                n /= 10;
            }
            number = tempSum;
        }

        return number;
    }
}
