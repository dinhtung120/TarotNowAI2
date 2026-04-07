

using System;

namespace TarotNow.Domain.Helpers;

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

        public static int CalculateNumerology(DateTime dob)
    {
        
        string dateString = dob.ToString("yyyyMMdd");
        int sum = 0;

        foreach (char c in dateString)
        {
            if (char.IsDigit(c))
            {
                
                sum += c - '0';
            }
        }

        
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
