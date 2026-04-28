using Microsoft.Extensions.Logging;
using Moq;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure.UnitTests.Configuration;

public sealed class ReadingPromptServiceTests
{
    [Fact]
    public void Build_WhenInitialReading_ShouldRenderPromptFromSystemConfig()
    {
        var snapshotStore = BuildSnapshotStore();
        var service = new ReadingPromptService(snapshotStore, Mock.Of<ILogger<ReadingPromptService>>());
        var session = BuildCompletedSession(
            question: "Will this week be better?",
            spreadType: SpreadType.Spread3Cards);

        var result = service.Build(session, followupQuestion: null, language: "vi");

        Assert.Equal("vi", result.ResolvedLanguage);
        Assert.Contains("reply purely in Vietnamese", result.SystemPrompt, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Will this week be better?", result.UserPrompt, StringComparison.Ordinal);
        Assert.Contains("Spread Type: spread_3", result.UserPrompt, StringComparison.Ordinal);
        Assert.Contains("The Fool", result.UserPrompt, StringComparison.Ordinal);
        Assert.Contains("Upright", result.UserPrompt, StringComparison.Ordinal);
    }

    [Fact]
    public void Build_WhenFollowupReading_ShouldRenderFollowupTemplate()
    {
        var snapshotStore = BuildSnapshotStore();
        var service = new ReadingPromptService(snapshotStore, Mock.Of<ILogger<ReadingPromptService>>());
        var session = BuildCompletedSession(
            question: "What should I focus on?",
            spreadType: SpreadType.Spread5Cards);

        var result = service.Build(
            session,
            followupQuestion: "What should I do next?",
            language: "en");

        Assert.Equal("en", result.ResolvedLanguage);
        Assert.Contains("answer my follow-up question: What should I do next?", result.UserPrompt, StringComparison.Ordinal);
    }

    [Fact]
    public void Build_WhenLanguageUnsupported_ShouldFallbackToDefaultLocale()
    {
        var snapshotStore = BuildSnapshotStore();
        var service = new ReadingPromptService(snapshotStore, Mock.Of<ILogger<ReadingPromptService>>());
        var session = BuildCompletedSession(
            question: "Will things get better?",
            spreadType: SpreadType.Spread10Cards);

        var result = service.Build(session, followupQuestion: null, language: "ja");

        Assert.Equal("vi", result.ResolvedLanguage);
        Assert.Contains("reply purely in Vietnamese", result.SystemPrompt, StringComparison.OrdinalIgnoreCase);
    }

    private static ReadingSession BuildCompletedSession(string question, string spreadType)
    {
        var session = new ReadingSession(
            userId: Guid.NewGuid().ToString(),
            spreadType: spreadType,
            question: question);
        session.CompleteSession(ReadingDrawnCardCodec.Serialize(
        [
            new ReadingDrawnCard
            {
                CardId = 0,
                Position = 0,
                Orientation = CardOrientation.Upright
            }
        ]));

        return session;
    }

    private static SystemConfigSnapshotStore BuildSnapshotStore()
    {
        var store = new SystemConfigSnapshotStore();
        store.Replace(
        [
            new SystemConfig(
                key: "ai.reading.prompts",
                value: BuildPromptCatalogJson(),
                valueKind: "json",
                description: "reading prompts",
                updatedBy: null)
        ]);
        return store;
    }

    private static string BuildPromptCatalogJson()
    {
        return
            """
            {
              "defaultLocale": "vi",
              "system": {
                "vi": "You MUST reply purely in Vietnamese (Tiếng Việt).",
                "en": "You MUST reply purely in English.",
                "zh": "You MUST reply purely in Chinese (繁體中文)."
              },
              "initial": {
                "default": "My question: \"{{question}}\". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"
              },
              "followup": {
                "default": "Based on my previous reading (Question: \"{{question}}\", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}"
              },
              "context": {
                "defaultQuestion": "A general reading about my current life.",
                "orientation": {
                  "upright": "Upright",
                  "reversed": "Reversed"
                },
                "unknownCardLabel": "Unknown Card"
              }
            }
            """;
    }
}
