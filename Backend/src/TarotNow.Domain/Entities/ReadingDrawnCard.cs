using System.Text.Json;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Snapshot một lá bài đã rút trong reading session.
/// </summary>
public sealed class ReadingDrawnCard
{
    /// <summary>
    /// Id lá bài trong bộ 78 lá.
    /// </summary>
    public int CardId { get; init; }

    /// <summary>
    /// Vị trí lá bài trong spread.
    /// </summary>
    public int Position { get; init; }

    /// <summary>
    /// Orientation của lá bài (upright/reversed).
    /// </summary>
    public string Orientation { get; init; } = CardOrientation.Upright;
}

/// <summary>
/// Codec chuẩn hóa serialize/deserialize dữ liệu cardsDrawn để tương thích dữ liệu cũ và mới.
/// </summary>
public static class ReadingDrawnCardCodec
{
    private static readonly JsonSerializerOptions CamelCaseJson = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Parse JSON cardsDrawn về danh sách card snapshot.
    /// Hỗ trợ cả schema cũ (mảng số) và schema mới (mảng object có orientation).
    /// </summary>
    public static IReadOnlyList<ReadingDrawnCard> Parse(string? cardsDrawnJson)
    {
        if (string.IsNullOrWhiteSpace(cardsDrawnJson))
        {
            return [];
        }

        try
        {
            using var document = JsonDocument.Parse(cardsDrawnJson);
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            var result = new List<ReadingDrawnCard>();
            var index = 0;
            foreach (var item in document.RootElement.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.Number && item.TryGetInt32(out var legacyCardId))
                {
                    result.Add(new ReadingDrawnCard
                    {
                        CardId = legacyCardId,
                        Position = index,
                        Orientation = CardOrientation.Upright
                    });
                    index++;
                    continue;
                }

                if (item.ValueKind != JsonValueKind.Object)
                {
                    index++;
                    continue;
                }

                if (!TryResolveCardId(item, out var cardId))
                {
                    index++;
                    continue;
                }

                var position = TryResolvePosition(item, index);
                var orientation = ResolveOrientation(item);

                result.Add(new ReadingDrawnCard
                {
                    CardId = cardId,
                    Position = position,
                    Orientation = orientation
                });
                index++;
            }

            return result
                .OrderBy(card => card.Position)
                .ToArray();
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Serialize danh sách card snapshot về JSON lưu session.
    /// </summary>
    public static string Serialize(IEnumerable<ReadingDrawnCard> cards)
    {
        var normalized = cards
            .OrderBy(card => card.Position)
            .Select(card => new ReadingDrawnCard
            {
                CardId = card.CardId,
                Position = card.Position,
                Orientation = NormalizeOrientation(card.Orientation)
            })
            .ToArray();

        return JsonSerializer.Serialize(normalized, CamelCaseJson);
    }

    /// <summary>
    /// Trả danh sách card id phục vụ logic pricing/prompt.
    /// </summary>
    public static int[] ExtractCardIds(string? cardsDrawnJson)
    {
        return Parse(cardsDrawnJson)
            .Select(card => card.CardId)
            .ToArray();
    }

    /// <summary>
    /// Chuẩn hóa orientation input về giá trị hợp lệ.
    /// </summary>
    public static string NormalizeOrientation(string? orientation)
    {
        return string.Equals(orientation, CardOrientation.Reversed, StringComparison.OrdinalIgnoreCase)
            ? CardOrientation.Reversed
            : CardOrientation.Upright;
    }

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
}
