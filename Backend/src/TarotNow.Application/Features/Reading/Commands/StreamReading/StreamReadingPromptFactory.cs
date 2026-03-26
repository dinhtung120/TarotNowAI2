using System.Text.Json;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

internal static class StreamReadingPromptFactory
{
    private static readonly string[] CardNames =
    {
        "The Fool", "The Magician", "The High Priestess", "The Empress", "The Emperor", "The Hierophant", "The Lovers", "The Chariot", "Strength", "The Hermit", "Wheel of Fortune", "Justice", "The Hanged Man", "Death", "Temperance", "The Devil", "The Tower", "The Star", "The Moon", "The Sun", "Judgement", "The World",
        "Ace of Wands", "Two of Wands", "Three of Wands", "Four of Wands", "Five of Wands", "Six of Wands", "Seven of Wands", "Eight of Wands", "Nine of Wands", "Ten of Wands", "Page of Wands", "Knight of Wands", "Queen of Wands", "King of Wands",
        "Ace of Cups", "Two of Cups", "Three of Cups", "Four of Cups", "Five of Cups", "Six of Cups", "Seven of Cups", "Eight of Cups", "Nine of Cups", "Ten of Cups", "Page of Cups", "Knight of Cups", "Queen of Cups", "King of Cups",
        "Ace of Swords", "Two of Swords", "Three of Swords", "Four of Swords", "Five of Swords", "Six of Swords", "Seven of Swords", "Eight of Swords", "Nine of Swords", "Ten of Swords", "Page of Swords", "Knight of Swords", "Queen of Swords", "King of Swords",
        "Ace of Pentacles", "Two of Pentacles", "Three of Pentacles", "Four of Pentacles", "Five of Pentacles", "Six of Pentacles", "Seven of Pentacles", "Eight of Pentacles", "Nine of Pentacles", "Ten of Pentacles", "Page of Pentacles", "Knight of Pentacles", "Queen of Pentacles", "King of Pentacles"
    };

    public static (string SystemPrompt, string UserPrompt) Build(ReadingSession session, string? followupQuestion, string language)
    {
        var systemPrompt = BuildSystemPrompt(language);
        var userPrompt = BuildUserPrompt(session, followupQuestion);
        return (systemPrompt, userPrompt);
    }

    private static string BuildSystemPrompt(string language)
    {
        var languageInstruction = language switch
        {
            "vi" => "You MUST reply purely in Vietnamese (Tiếng Việt).",
            "zh" => "You MUST reply purely in Chinese (繁體中文).",
            _ => "You MUST reply purely in English."
        };

        return $@"You are a mystical, wise, and empathetic Tarot Reader.
Format your response clearly using Markdown.
You give highly accurate and deeply personalized readings.
{languageInstruction}";
    }

    private static string BuildUserPrompt(ReadingSession session, string? followupQuestion)
    {
        var questionContext = string.IsNullOrWhiteSpace(session.Question)
            ? "A general reading about my current life."
            : session.Question;

        var drawnCardsContext = BuildDrawnCardsContext(session.CardsDrawn);
        if (string.IsNullOrWhiteSpace(followupQuestion))
        {
            return $"My question: \"{questionContext}\". Interpret this reading for me. Spread Type: {session.SpreadType}. Cards Chosen: {drawnCardsContext}";
        }

        return $"Based on my previous reading (Question: \"{questionContext}\", Spread: {session.SpreadType}, Cards: {drawnCardsContext}), answer my follow-up question: {followupQuestion}";
    }

    private static string BuildDrawnCardsContext(string? cardsDrawn)
    {
        var cardIds = JsonSerializer.Deserialize<List<int>>(cardsDrawn ?? "[]") ?? [];

        return string.Join(
            ", ",
            cardIds.Select((cardId, index) => $"[Position {index + 1}: {ResolveCardName(cardId)}]"));
    }

    private static string ResolveCardName(int cardId)
    {
        return cardId >= 0 && cardId < CardNames.Length
            ? CardNames[cardId]
            : "Unknown Card";
    }
}
