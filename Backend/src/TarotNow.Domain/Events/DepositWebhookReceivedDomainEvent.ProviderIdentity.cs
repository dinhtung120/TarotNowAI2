using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TarotNow.Domain.Events;

public sealed partial class DepositWebhookReceivedDomainEvent
{
    private static string? TryBuildProviderIdentityKey(string? rawPayload)
    {
        if (string.IsNullOrWhiteSpace(rawPayload))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(rawPayload);
            var root = document.RootElement;
            var data = TryGetObjectProperty(root, "data", out var dataElement) ? dataElement : root;

            var orderCode = TryReadInt64(data, "orderCode");
            if (!orderCode.HasValue || orderCode.Value <= 0)
            {
                return null;
            }

            var reference = NormalizeOptional(TryReadString(data, "reference"));
            var paymentLinkId = NormalizeOptional(TryReadString(data, "paymentLinkId"));
            var transactionDateTime = NormalizeOptional(TryReadString(data, "transactionDateTime"));
            var gatewayCode = NormalizeOptional(TryReadString(data, "code") ?? TryReadString(root, "code"));
            var amount = TryReadInt64(data, "amount");
            var success = TryReadBool(root, "success");

            if (string.IsNullOrWhiteSpace(reference)
                && string.IsNullOrWhiteSpace(transactionDateTime)
                && string.IsNullOrWhiteSpace(gatewayCode))
            {
                return null;
            }

            var businessIdentity = string.Join(
                '|',
                ProviderName,
                orderCode.Value.ToString(CultureInfo.InvariantCulture),
                reference ?? string.Empty,
                paymentLinkId ?? string.Empty,
                transactionDateTime ?? string.Empty,
                gatewayCode ?? string.Empty,
                amount?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                success.HasValue ? (success.Value ? "1" : "0") : string.Empty);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(businessIdentity));
            var fingerprint = Convert.ToHexString(hash.AsSpan(0, 16)).ToLowerInvariant();
            return $"{EventKeyPrefix}:{ProviderName}:{fingerprint}";
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static bool TryGetObjectProperty(JsonElement element, string name, out JsonElement value)
    {
        value = default;
        if (element.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        return false;
    }

    private static string? TryReadString(JsonElement element, string propertyName)
    {
        if (!TryGetObjectProperty(element, propertyName, out var value))
        {
            return null;
        }

        if (value.ValueKind == JsonValueKind.String)
        {
            return value.GetString();
        }

        return value.ValueKind switch
        {
            JsonValueKind.Number => value.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => null
        };
    }

    private static long? TryReadInt64(JsonElement element, string propertyName)
    {
        if (!TryGetObjectProperty(element, propertyName, out var value))
        {
            return null;
        }

        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt64(out var number))
        {
            return number;
        }

        if (value.ValueKind == JsonValueKind.String
            && long.TryParse(value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static bool? TryReadBool(JsonElement element, string propertyName)
    {
        if (!TryGetObjectProperty(element, propertyName, out var value))
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String when bool.TryParse(value.GetString(), out var parsed) => parsed,
            JsonValueKind.Number when value.TryGetInt64(out var number) => number != 0,
            _ => null
        };
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
