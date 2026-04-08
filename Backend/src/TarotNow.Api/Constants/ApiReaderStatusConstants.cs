namespace TarotNow.Api.Constants;

/// <summary>
/// Chuẩn hóa trạng thái reader để toàn hệ thống dùng cùng một tập giá trị chuẩn.
/// Lý do: client có thể gửi nhiều alias khác nhau nhưng nghiệp vụ chỉ xử lý trạng thái chuẩn.
/// </summary>
public static class ApiReaderStatusConstants
{
    // Trạng thái reader đang sẵn sàng trực tuyến.
    public const string Online = "online";

    // Trạng thái reader đang ngoại tuyến.
    public const string Offline = "offline";

    // Trạng thái reader bận hoặc tạm thời không nhận phiên mới.
    public const string Busy = "busy";

    /// <summary>
    /// Bảng ánh xạ alias từ client về trạng thái chuẩn của hệ thống.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string> AliasToStatus =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [Online] = Online,
            ["active"] = Online,
            ["connected"] = Online,
            ["available"] = Online,
            [Offline] = Offline,
            ["disconnected"] = Offline,
            ["invisible"] = Offline,
            [Busy] = Busy,
            ["away"] = Busy,
            ["idle"] = Busy,
            ["accepting_questions"] = Busy,
            ["acceptingquestions"] = Busy,
            ["accepting-questions"] = Busy,
            ["accepting"] = Busy,
            ["ready"] = Busy
        };

    /// <summary>
    /// Chuẩn hóa trạng thái reader từ input tự do về bộ giá trị chuẩn.
    /// Luồng xử lý: gán mặc định, kiểm tra input, ánh xạ alias, rồi trả kết quả.
    /// </summary>
    /// <param name="status">Trạng thái thô nhận từ client.</param>
    /// <param name="normalized">Giá trị chuẩn hóa đầu ra khi ánh xạ thành công.</param>
    /// <returns><c>true</c> nếu ánh xạ được; ngược lại trả <c>false</c>.</returns>
    public static bool TryNormalize(string? status, out string normalized)
    {
        normalized = Offline;
        if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(status.Trim()))
        {
            // Edge case input rỗng: không đủ dữ liệu để suy ra trạng thái nghiệp vụ hợp lệ.
            return false;
        }

        if (!AliasToStatus.TryGetValue(status.Trim(), out var mapped))
        {
            // Chỉ chấp nhận alias nằm trong bảng chuẩn để tránh trạng thái ngoài kiểm soát.
            return false;
        }

        normalized = mapped;
        // Cập nhật trạng thái đã chuẩn hóa để downstream xử lý nhất quán.
        return true;
    }

    /// <summary>
    /// Trả trạng thái đã chuẩn hóa hoặc dùng mặc định khi đầu vào không hợp lệ.
    /// Luồng xử lý: tái sử dụng <see cref="TryNormalize(string?, out string)"/> để không tách rule chuẩn hóa.
    /// </summary>
    /// <param name="status">Trạng thái thô nhận từ client.</param>
    /// <param name="defaultStatus">Trạng thái fallback khi chuẩn hóa thất bại.</param>
    /// <returns>Trạng thái chuẩn hóa nếu có; nếu không trả trạng thái mặc định.</returns>
    public static string NormalizeOrDefault(string? status, string defaultStatus = Offline)
        => TryNormalize(status, out var normalized) ? normalized : defaultStatus;
}
