using System.Text.Json;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisRealtimeSignalRBridgeService
{
    private static string[] GetUserGroups(JsonElement payload)
    {
        var groups = new List<string>(2);
        AddUserGroup(groups, GetStringProperty(payload, "userId"));
        AddUserGroup(groups, GetStringProperty(payload, "readerId"));
        return groups.ToArray();
    }

    private static void AddUserGroup(List<string> groups, string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var groupName = $"user:{userId}";
        if (groups.Contains(groupName, StringComparer.Ordinal))
        {
            return;
        }

        groups.Add(groupName);
    }

    private static string? GetStringProperty(JsonElement payload, string propertyName)
    {
        if (!TryGetProperty(payload, propertyName, out var value))
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.GetRawText(),
            _ => null
        };
    }

    private static bool TryGetProperty(JsonElement payload, string propertyName, out JsonElement value)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            value = default;
            return false;
        }

        if (payload.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in payload.EnumerateObject())
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
