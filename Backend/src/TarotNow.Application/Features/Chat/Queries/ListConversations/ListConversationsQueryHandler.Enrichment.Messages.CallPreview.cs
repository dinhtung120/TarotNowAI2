using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    /// <summary>
    /// Dựng preview cho call-log message.
    /// Luồng xử lý: trích xuất loại cuộc gọi và thời lượng, trả nhãn missed-call khi duration không hợp lệ hoặc bằng 0.
    /// </summary>
    private static string BuildCallPreview(ChatMessageDto message)
    {
        if (!TryExtractCallPreview(message, out var isVideo, out var duration))
        {
            // Không parse được metadata cuộc gọi thì dùng nhãn call chung an toàn.
            return "📞 Cuộc gọi";
        }

        var icon = isVideo ? "🎥" : "📞";
        var type = isVideo ? "Cuộc gọi video" : "Cuộc gọi thoại";
        if (duration <= 0)
        {
            // Duration <= 0 được xem là cuộc gọi bị nhỡ.
            return $"{icon} {type} bị nhỡ";
        }

        var minutes = duration / 60;
        var seconds = duration % 60;
        return $"{icon} {type} ({minutes:00}:{seconds:00})";
    }

    /// <summary>
    /// Thử trích xuất thông tin preview cuộc gọi từ message.
    /// Luồng xử lý: ưu tiên CallPayload typed-data; fallback parse JSON từ content khi payload không có.
    /// </summary>
    private static bool TryExtractCallPreview(ChatMessageDto message, out bool isVideo, out int durationSeconds)
    {
        isVideo = false;
        durationSeconds = 0;

        if (message.CallPayload != null)
        {
            // Nhánh chuẩn: lấy trực tiếp từ CallPayload đã được map typed.
            isVideo = message.CallPayload.Type == CallType.Video;
            durationSeconds = Math.Max(0, message.CallPayload.DurationSeconds ?? 0);
            return true;
        }

        if (string.IsNullOrWhiteSpace(message.Content))
        {
            // Không còn nguồn dữ liệu nào để parse preview.
            return false;
        }

        return TryExtractCallPreviewFromJson(message.Content, out isVideo, out durationSeconds);
    }

    /// <summary>
    /// Parse thông tin cuộc gọi từ JSON string trong content.
    /// Luồng xử lý: đọc callType + duration với key primary/fallback; trả false khi JSON lỗi định dạng.
    /// </summary>
    private static bool TryExtractCallPreviewFromJson(string json, out bool isVideo, out int durationSeconds)
    {
        isVideo = false;
        durationSeconds = 0;

        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var callType = ReadStringProperty(doc.RootElement, "CallType", "callType") ?? "audio";
            var parsedDuration = ReadIntProperty(doc.RootElement, "DurationSeconds", "durationSeconds");

            isVideo = string.Equals(callType, "video", StringComparison.OrdinalIgnoreCase);
            durationSeconds = Math.Max(0, parsedDuration ?? 0);
            return true;
        }
        catch
        {
            // Edge case content không phải JSON hợp lệ hoặc thiếu schema mong đợi.
            return false;
        }
    }

    /// <summary>
    /// Đọc giá trị string từ JSON với key chính và key fallback.
    /// Luồng xử lý: thử key primary trước, nếu không có thì thử fallback.
    /// </summary>
    private static string? ReadStringProperty(System.Text.Json.JsonElement root, string primary, string fallback)
    {
        if (root.TryGetProperty(primary, out var primaryValue))
        {
            return primaryValue.GetString();
        }

        return root.TryGetProperty(fallback, out var fallbackValue)
            ? fallbackValue.GetString()
            : null;
    }

    /// <summary>
    /// Đọc giá trị int từ JSON với key chính và key fallback.
    /// Luồng xử lý: parse key primary trước, nếu không parse được thì fallback sang key phụ.
    /// </summary>
    private static int? ReadIntProperty(System.Text.Json.JsonElement root, string primary, string fallback)
    {
        if (root.TryGetProperty(primary, out var primaryValue) && primaryValue.TryGetInt32(out var primaryInt))
        {
            return primaryInt;
        }

        return root.TryGetProperty(fallback, out var fallbackValue) && fallbackValue.TryGetInt32(out var fallbackInt)
            ? fallbackInt
            : null;
    }
}
