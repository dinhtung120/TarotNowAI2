using TarotNow.Application.Common.SystemConfigs;

namespace TarotNow.Application.UnitTests.Common;

public sealed class SystemConfigRegistryReadingPromptsValidationTests
{
    [Fact]
    public void ValidateReadingPromptCatalog_WhenPayloadIsValid_ShouldPass()
    {
        var validation = SystemConfigRegistry.Validate(
            key: "ai.reading.prompts",
            value: BuildValidPromptCatalogJson(),
            valueKind: "json");

        Assert.True(validation.IsValid, validation.Error);
    }

    [Fact]
    public void ValidateReadingPromptCatalog_WhenMissingRequiredField_ShouldFail()
    {
        var invalidJson =
            """
            {
              "defaultLocale": "vi",
              "system": {
                "vi": "x",
                "en": "y",
                "zh": "z"
              }
            }
            """;

        var validation = SystemConfigRegistry.Validate(
            key: "ai.reading.prompts",
            value: invalidJson,
            valueKind: "json");

        Assert.False(validation.IsValid);
        Assert.NotNull(validation.Error);
    }

    [Fact]
    public void ValidateReadingPromptCatalog_WhenLegacyLocalizedNonSystemShape_ShouldPass()
    {
        var legacyJson =
            """
            {
              "defaultLocale": "vi",
              "system": {
                "vi": "You MUST reply purely in Vietnamese (Tiếng Việt).",
                "en": "You MUST reply purely in English.",
                "zh": "You MUST reply purely in Chinese (繁體中文)."
              },
              "initial": {
                "default": {
                  "vi": "My question: \"{{question}}\". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}",
                  "en": "My question: \"{{question}}\". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}",
                  "zh": "My question: \"{{question}}\". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"
                }
              },
              "followup": {
                "default": {
                  "vi": "Based on my previous reading (Question: \"{{question}}\", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}",
                  "en": "Based on my previous reading (Question: \"{{question}}\", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}",
                  "zh": "Based on my previous reading (Question: \"{{question}}\", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}"
                }
              },
              "context": {
                "defaultQuestion": {
                  "vi": "A general reading about my current life.",
                  "en": "A general reading about my current life.",
                  "zh": "A general reading about my current life."
                },
                "orientation": {
                  "upright": {
                    "vi": "Upright",
                    "en": "Upright",
                    "zh": "Upright"
                  },
                  "reversed": {
                    "vi": "Reversed",
                    "en": "Reversed",
                    "zh": "Reversed"
                  }
                },
                "unknownCardLabel": {
                  "vi": "Unknown Card",
                  "en": "Unknown Card",
                  "zh": "Unknown Card"
                }
              }
            }
            """;

        var validation = SystemConfigRegistry.Validate(
            key: "ai.reading.prompts",
            value: legacyJson,
            valueKind: "json");

        Assert.True(validation.IsValid, validation.Error);
    }

    private static string BuildValidPromptCatalogJson()
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
