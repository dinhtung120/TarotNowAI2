
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái online của Reader kèm logic chuẩn hóa alias từ nhiều nguồn client.
public static class ReaderOnlineStatus
{
    // Reader sẵn sàng online.
    public const string Online = "online";

    // Reader offline hoặc không kết nối.
    public const string Offline = "offline";

    // Reader bận hoặc tạm không nhận thêm.
    public const string Busy = "busy";

    // Bảng ánh xạ alias trạng thái về bộ chuẩn nội bộ để xử lý dữ liệu nhập không đồng nhất.
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
    /// Kiểm tra trạng thái đầu vào có thể chuẩn hóa về bộ trạng thái hợp lệ hay không.
    /// Luồng xử lý: gọi TryNormalize và bỏ qua giá trị normalize trả về.
    /// </summary>
    public static bool IsValid(string? status)
        => TryNormalize(status, out _);

    /// <summary>
    /// Chuẩn hóa trạng thái reader từ nhiều alias về một giá trị chuẩn.
    /// Luồng xử lý: khởi tạo mặc định Offline, validate chuỗi đầu vào, map alias và trả kết quả bool.
    /// </summary>
    public static bool TryNormalize(string? status, out string normalized)
    {
        normalized = Offline;
        if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(status.Trim()))
        {
            // Edge case: trạng thái rỗng/blank không hợp lệ nên giữ mặc định Offline và trả false.
            return false;
        }

        if (!AliasToStatus.TryGetValue(status.Trim(), out var mapped))
        {
            // Trạng thái không nằm trong alias map thì coi là không hợp lệ.
            return false;
        }

        normalized = mapped;
        // Chuẩn hóa thành công về giá trị chuẩn để lưu và broadcast nhất quán.
        return true;
    }

    /// <summary>
    /// Chuẩn hóa trạng thái hoặc trả về giá trị mặc định khi đầu vào không hợp lệ.
    /// Luồng xử lý: thử normalize trước, nếu thất bại thì dùng defaultStatus.
    /// </summary>
    public static string NormalizeOrDefault(string? status, string defaultStatus = Offline)
        => TryNormalize(status, out var normalized) ? normalized : defaultStatus;
}
