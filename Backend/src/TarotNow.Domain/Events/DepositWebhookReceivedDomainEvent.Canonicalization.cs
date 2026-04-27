using System.Linq;
using System.Text;
using System.Text.Json;

namespace TarotNow.Domain.Events;

public sealed partial class DepositWebhookReceivedDomainEvent
{
    private static string CanonicalizePayload(string? rawPayload)
    {
        if (string.IsNullOrWhiteSpace(rawPayload))
        {
            return string.Empty;
        }

        try
        {
            using var doc = JsonDocument.Parse(rawPayload);
            var builder = new StringBuilder();
            AppendCanonicalElement(doc.RootElement, builder);
            return builder.ToString();
        }
        catch (JsonException)
        {
            return rawPayload.Trim();
        }
    }

    private static void AppendCanonicalElement(JsonElement element, StringBuilder builder)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            AppendCanonicalObject(element, builder);
            return;
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            AppendCanonicalArray(element, builder);
            return;
        }

        AppendCanonicalScalar(element, builder);
    }

    private static void AppendCanonicalScalar(JsonElement element, StringBuilder builder)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                builder.Append(JsonSerializer.Serialize(element.GetString()));
                return;
            case JsonValueKind.Number:
                builder.Append(element.GetRawText());
                return;
            case JsonValueKind.True:
                AppendLiteral(builder, "true");
                return;
            case JsonValueKind.False:
                AppendLiteral(builder, "false");
                return;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                AppendLiteral(builder, "null");
                return;
            default:
                builder.Append(element.GetRawText());
                return;
        }
    }

    private static void AppendCanonicalObject(JsonElement element, StringBuilder builder)
    {
        builder.Append('{');
        var ordered = element.EnumerateObject()
            .OrderBy(property => property.Name, StringComparer.Ordinal)
            .ToArray();

        for (var index = 0; index < ordered.Length; index++)
        {
            if (index > 0)
            {
                builder.Append(',');
            }

            builder.Append(JsonSerializer.Serialize(ordered[index].Name)).Append(':');
            AppendCanonicalElement(ordered[index].Value, builder);
        }

        builder.Append('}');
    }

    private static void AppendCanonicalArray(JsonElement element, StringBuilder builder)
    {
        builder.Append('[');
        var first = true;

        foreach (var item in element.EnumerateArray())
        {
            if (!first)
            {
                builder.Append(',');
            }

            AppendCanonicalElement(item, builder);
            first = false;
        }

        builder.Append(']');
    }

    private static void AppendLiteral(StringBuilder builder, string literal)
    {
        builder.Append(literal);
    }
}
