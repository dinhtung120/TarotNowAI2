using System.Text.Json;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

public sealed partial class OutboxBatchProcessor
{
    private static readonly string[] PartitionKeyCandidates =
    {
        "PartitionKey",
        "UserId",
        "OwnerUserId",
        "ConversationId",
        "SessionId",
        "ReadingSessionId",
        "PostId",
        "CommentId",
        "RequestId",
        "OperationId",
        "ReaderRequestId",
        "WithdrawalId",
    };

    private static string ResolveProcessingPartitionKey(OutboxMessage message)
    {
        if (TryExtractBusinessPartitionKey(message.PayloadJson, out var key))
        {
            return key;
        }

        return string.IsNullOrWhiteSpace(message.EventType)
            ? "unknown"
            : $"event:{message.EventType}";
    }

    private static bool TryExtractBusinessPartitionKey(string payloadJson, out string key)
    {
        key = string.Empty;
        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return false;
        }

        try
        {
            using var doc = JsonDocument.Parse(payloadJson);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            foreach (var candidate in PartitionKeyCandidates)
            {
                if (!TryReadRootStringProperty(doc.RootElement, candidate, out var value))
                {
                    continue;
                }

                key = $"{candidate}:{value}";
                return true;
            }
        }
        catch (JsonException)
        {
            // Ignore malformed payload and fallback to event type partition.
        }

        return false;
    }

    private static bool TryReadRootStringProperty(JsonElement root, string propertyName, out string value)
    {
        value = string.Empty;
        if (!TryGetPropertyCaseInsensitive(root, propertyName, out var node))
        {
            return false;
        }

        if (node.ValueKind == JsonValueKind.String)
        {
            var text = node.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            value = text.Trim();
            return true;
        }

        if (node.ValueKind == JsonValueKind.Number)
        {
            value = node.GetRawText();
            return true;
        }

        if (node.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return false;
        }

        value = node.GetRawText();
        return string.IsNullOrWhiteSpace(value) == false;
    }

    private static bool TryGetPropertyCaseInsensitive(
        JsonElement root,
        string propertyName,
        out JsonElement value)
    {
        if (root.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in root.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}
