using System.Collections.Generic;
using System.Linq;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// Factory dựng prompt cho AI stream reading dựa trên session và follow-up question.
internal static class StreamReadingPromptFactory
{
    // Danh mục 78 lá bài Tarot dùng để resolve tên lá theo id.
    private static readonly string[] CardNames =
    {
        "The Fool", "The Magician", "The High Priestess", "The Empress", "The Emperor", "The Hierophant", "The Lovers", "The Chariot", "Strength", "The Hermit", "Wheel of Fortune", "Justice", "The Hanged Man", "Death", "Temperance", "The Devil", "The Tower", "The Star", "The Moon", "The Sun", "Judgement", "The World",
        "Ace of Wands", "Two of Wands", "Three of Wands", "Four of Wands", "Five of Wands", "Six of Wands", "Seven of Wands", "Eight of Wands", "Nine of Wands", "Ten of Wands", "Page of Wands", "Knight of Wands", "Queen of Wands", "King of Wands",
        "Ace of Cups", "Two of Cups", "Three of Cups", "Four of Cups", "Five of Cups", "Six of Cups", "Seven of Cups", "Eight of Cups", "Nine of Cups", "Ten of Cups", "Page of Cups", "Knight of Cups", "Queen of Cups", "King of Cups",
        "Ace of Swords", "Two of Swords", "Three of Swords", "Four of Swords", "Five of Swords", "Six of Swords", "Seven of Swords", "Eight of Swords", "Nine of Swords", "Ten of Swords", "Page of Swords", "Knight of Swords", "Queen of Swords", "King of Swords",
        "Ace of Pentacles", "Two of Pentacles", "Three of Pentacles", "Four of Pentacles", "Five of Pentacles", "Six of Pentacles", "Seven of Pentacles", "Eight of Pentacles", "Nine of Pentacles", "Ten of Pentacles", "Page of Pentacles", "Knight of Pentacles", "Queen of Pentacles", "King of Pentacles"
    };

    /// <summary>
    /// Dựng cặp system/user prompt cho request stream reading.
    /// Luồng xử lý: xây system prompt theo ngôn ngữ và user prompt theo ngữ cảnh session + follow-up.
    /// </summary>
    public static (string SystemPrompt, string UserPrompt) Build(
        ReadingSession session,
        string? followupQuestion,
        string language)
    {
        var systemPrompt = BuildSystemPrompt(language);
        var userPrompt = BuildUserPrompt(session, followupQuestion);
        return (systemPrompt, userPrompt);
    }

    /// <summary>
    /// Dựng system prompt theo ngôn ngữ đầu ra mong muốn.
    /// Luồng xử lý: map mã ngôn ngữ sang instruction cụ thể, mặc định dùng tiếng Anh khi không thuộc vi/zh.
    /// </summary>
    private static string BuildSystemPrompt(string language)
    {
        var languageInstruction = language switch
        {
            "vi" => "You MUST reply purely in Vietnamese (Tiếng Việt).",
            "zh" => "You MUST reply purely in Chinese (繁體中文).",
            _ => "You MUST reply purely in English."
        };
        // Khóa ngôn ngữ đầu ra để đồng bộ với lựa chọn của người dùng trên client.

        return $@"You are a mystical, wise, and empathetic Tarot Reader.
Format your response clearly using Markdown.
You give highly accurate and deeply personalized readings.
{languageInstruction}";
    }

    /// <summary>
    /// Dựng user prompt theo ngữ cảnh phiên đọc và follow-up.
    /// Luồng xử lý: chuẩn hóa context câu hỏi gốc + cards; nếu có follow-up thì tạo prompt tiếp nối, ngược lại tạo prompt diễn giải lần đầu.
    /// </summary>
    private static string BuildUserPrompt(ReadingSession session, string? followupQuestion)
    {
        var questionContext = string.IsNullOrWhiteSpace(session.Question)
            ? "A general reading about my current life."
            : session.Question;
        // Edge case: session không có câu hỏi gốc thì dùng prompt mặc định để AI vẫn có ngữ cảnh.

        var drawnCardsContext = BuildDrawnCardsContext(session.CardsDrawn);
        if (string.IsNullOrWhiteSpace(followupQuestion))
        {
            // Luồng initial interpretation sau khi reveal cards.
            return $"My question: \"{questionContext}\". Interpret this reading for me. Spread Type: {session.SpreadType}. Cards Chosen: {drawnCardsContext}";
        }

        // Luồng follow-up cần gắn lại toàn bộ context phiên trước để AI trả lời liên tục.
        return $"Based on my previous reading (Question: \"{questionContext}\", Spread: {session.SpreadType}, Cards: {drawnCardsContext}), answer my follow-up question: {followupQuestion}";
    }

    /// <summary>
    /// Dựng chuỗi mô tả danh sách lá bài đã rút theo thứ tự vị trí.
    /// Luồng xử lý: parse JSON cardsDrawn thành id list rồi map từng id sang tên lá.
    /// </summary>
    private static string BuildDrawnCardsContext(string? cardsDrawn)
    {
        var cards = ReadingDrawnCardCodec.Parse(cardsDrawn);
        // Edge case: dữ liệu cardsDrawn trống/null thì fallback danh sách rỗng an toàn.

        return string.Join(
            ", ",
            cards.Select(card =>
                $"[Position {card.Position + 1}: {ResolveCardName(card.CardId)} ({ResolveOrientationLabel(card.Orientation)})]"));
    }

    /// <summary>
    /// Resolve tên lá bài theo card id.
    /// Luồng xử lý: tra mảng tên 78 lá; id ngoài phạm vi trả về "Unknown Card".
    /// </summary>
    private static string ResolveCardName(int cardId)
    {
        return cardId >= 0 && cardId < CardNames.Length
            ? CardNames[cardId]
            : "Unknown Card";
    }

    private static string ResolveOrientationLabel(string orientation)
    {
        return orientation == CardOrientation.Reversed ? "Reversed" : "Upright";
    }
}
