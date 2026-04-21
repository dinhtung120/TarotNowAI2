using System.Globalization;
using System.Text;

namespace TarotNow.Application.Common.Helpers;

/// <summary>
/// Validator kiểm tra tên chủ tài khoản theo quy tắc chữ hoa không dấu.
/// </summary>
public static class AccountHolderNameValidator
{
    /// <summary>
    /// Kiểm tra chuỗi đầu vào có đúng định dạng tên tài khoản chữ hoa không dấu hay không.
    /// </summary>
    public static bool IsValidUppercaseNoAccent(string? accountHolder)
    {
        var normalized = accountHolder?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return false;
        }

        if (!string.Equals(normalized, normalized.ToUpperInvariant(), StringComparison.Ordinal))
        {
            return false;
        }

        foreach (var character in normalized.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                return false;
            }
        }

        foreach (var character in normalized)
        {
            var isUpperLetter = character is >= 'A' and <= 'Z';
            var isWhitespace = character == ' ';
            if (!isUpperLetter && !isWhitespace)
            {
                return false;
            }
        }

        return true;
    }
}
