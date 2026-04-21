namespace TarotNow.Domain.Enums;

/// <summary>
/// Bộ chuyên môn cố định dành cho Reader.
/// </summary>
public static class ReaderSpecialties
{
    /// <summary>
    /// Chuyên môn tình yêu.
    /// </summary>
    public const string Love = "love";

    /// <summary>
    /// Chuyên môn sự nghiệp.
    /// </summary>
    public const string Career = "career";

    /// <summary>
    /// Chuyên môn tổng quát.
    /// </summary>
    public const string General = "general";

    /// <summary>
    /// Chuyên môn sức khỏe.
    /// </summary>
    public const string Health = "health";

    /// <summary>
    /// Chuyên môn tài chính.
    /// </summary>
    public const string Finance = "finance";

    /// <summary>
    /// Tập chuyên môn hợp lệ.
    /// </summary>
    public static IReadOnlyCollection<string> All { get; } =
        new[] { Love, Career, General, Health, Finance };

    /// <summary>
    /// Kiểm tra chuyên môn có hợp lệ theo tập cố định hay không.
    /// </summary>
    public static bool IsSupported(string? value)
    {
        var normalized = Normalize(value);
        return string.IsNullOrEmpty(normalized) == false && All.Contains(normalized);
    }

    /// <summary>
    /// Chuẩn hóa chuyên môn về lower-case trim.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// Chuẩn hóa danh sách chuyên môn và loại bỏ trùng lặp.
    /// </summary>
    public static IReadOnlyList<string> NormalizeDistinct(IEnumerable<string>? values)
    {
        if (values is null)
        {
            return [];
        }

        return values
            .Select(Normalize)
            .Where(x => string.IsNullOrEmpty(x) == false)
            .Cast<string>()
            .Distinct(StringComparer.Ordinal)
            .ToList();
    }
}
