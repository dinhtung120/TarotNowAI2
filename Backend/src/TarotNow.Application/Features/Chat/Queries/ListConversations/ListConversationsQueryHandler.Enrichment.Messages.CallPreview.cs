using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    private static string BuildCallPreview(ChatMessageDto message)
    {
        if (!TryExtractCallPreview(message, out var isVideo, out var duration))
        {
            return "📞 Cuộc gọi";
        }

        var icon = isVideo ? "🎥" : "📞";
        var type = isVideo ? "Cuộc gọi video" : "Cuộc gọi thoại";
        if (duration <= 0)
        {
            return $"{icon} {type} bị nhỡ";
        }

        var minutes = duration / 60;
        var seconds = duration % 60;
        return $"{icon} {type} ({minutes:00}:{seconds:00})";
    }

    private static bool TryExtractCallPreview(ChatMessageDto message, out bool isVideo, out int durationSeconds)
    {
        isVideo = false;
        durationSeconds = 0;

        if (message.CallPayload != null)
        {
            isVideo = message.CallPayload.Type == CallType.Video;
            durationSeconds = Math.Max(0, message.CallPayload.DurationSeconds ?? 0);
            return true;
        }

        if (string.IsNullOrWhiteSpace(message.Content))
        {
            return false;
        }

        return TryExtractCallPreviewFromJson(message.Content, out isVideo, out durationSeconds);
    }

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
            return false;
        }
    }

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
