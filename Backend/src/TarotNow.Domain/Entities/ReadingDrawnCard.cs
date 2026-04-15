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
public static partial class ReadingDrawnCardCodec
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
            return ParseRootElement(document.RootElement);
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

}
