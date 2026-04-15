using System.Text.Json;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Helper parse cho codec cardsDrawn.
/// </summary>
public static partial class ReadingDrawnCardCodec
{
    private static bool TryResolveCardId(JsonElement objectElement, out int cardId)
    {
        if (TryGetProperty(objectElement, "cardId", out var cardIdElement) && cardIdElement.TryGetInt32(out cardId))
        {
            return true;
        }

        if (TryGetProperty(objectElement, "CardId", out var legacyCardIdElement) && legacyCardIdElement.TryGetInt32(out cardId))
        {
            return true;
        }

        if (TryGetProperty(objectElement, "id", out var idElement) && idElement.TryGetInt32(out cardId))
        {
            return true;
        }

        cardId = default;
        return false;
    }

    private static int TryResolvePosition(JsonElement objectElement, int fallbackPosition)
    {
        if (TryGetProperty(objectElement, "position", out var positionElement) && positionElement.TryGetInt32(out var position))
        {
            return position;
        }

        if (TryGetProperty(objectElement, "Position", out var legacyPositionElement) && legacyPositionElement.TryGetInt32(out position))
        {
            return position;
        }

        return fallbackPosition;
    }

    private static string ResolveOrientation(JsonElement objectElement)
    {
        if (TryGetProperty(objectElement, "orientation", out var orientationElement)
            && orientationElement.ValueKind == JsonValueKind.String)
        {
            return NormalizeOrientation(orientationElement.GetString());
        }

        if (TryGetProperty(objectElement, "Orientation", out var legacyOrientationElement)
            && legacyOrientationElement.ValueKind == JsonValueKind.String)
        {
            return NormalizeOrientation(legacyOrientationElement.GetString());
        }

        if (TryGetProperty(objectElement, "isReversed", out var isReversedElement)
            && isReversedElement.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            return isReversedElement.GetBoolean() ? CardOrientation.Reversed : CardOrientation.Upright;
        }

        if (TryGetProperty(objectElement, "IsReversed", out var legacyIsReversedElement)
            && legacyIsReversedElement.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            return legacyIsReversedElement.GetBoolean() ? CardOrientation.Reversed : CardOrientation.Upright;
        }

        return CardOrientation.Upright;
    }

    private static bool TryGetProperty(JsonElement objectElement, string propertyName, out JsonElement value)
    {
        if (objectElement.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in objectElement.EnumerateObject())
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

    private static IReadOnlyList<ReadingDrawnCard> ParseRootElement(JsonElement rootElement)
    {
        if (rootElement.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return ParseArrayItems(rootElement)
            .OrderBy(card => card.Position)
            .ToArray();
    }

    private static List<ReadingDrawnCard> ParseArrayItems(JsonElement rootElement)
    {
        var result = new List<ReadingDrawnCard>();
        var index = 0;
        foreach (var item in rootElement.EnumerateArray())
        {
            if (TryParseLegacyNumericCard(item, index, out var parsedCard)
                || TryParseObjectCard(item, index, out parsedCard))
            {
                result.Add(parsedCard);
            }

            index++;
        }

        return result;
    }

    private static bool TryParseLegacyNumericCard(
        JsonElement item,
        int index,
        out ReadingDrawnCard card)
    {
        if (item.ValueKind == JsonValueKind.Number && item.TryGetInt32(out var legacyCardId))
        {
            card = new ReadingDrawnCard
            {
                CardId = legacyCardId,
                Position = index,
                Orientation = CardOrientation.Upright
            };

            return true;
        }

        card = null!;
        return false;
    }

    private static bool TryParseObjectCard(JsonElement item, int fallbackPosition, out ReadingDrawnCard card)
    {
        if (item.ValueKind != JsonValueKind.Object || !TryResolveCardId(item, out var cardId))
        {
            card = null!;
            return false;
        }

        card = new ReadingDrawnCard
        {
            CardId = cardId,
            Position = TryResolvePosition(item, fallbackPosition),
            Orientation = ResolveOrientation(item)
        };

        return true;
    }
}
