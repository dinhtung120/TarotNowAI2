namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static object BuildDefaultReadingPromptCatalog()
    {
        return new
        {
            defaultLocale = "vi",
            system = BuildSystemPromptLocaleMap(),
            initial = BuildInitialPromptMap(),
            followup = BuildFollowupPromptMap(),
            context = BuildPromptContext()
        };
    }

    private static object BuildSystemPromptLocaleMap()
    {
        return new
        {
            vi = """
                 You are a mystical, wise, and empathetic Tarot Reader.
                 Format your response clearly using Markdown.
                 You give highly accurate and deeply personalized readings.
                 You MUST reply purely in Vietnamese (Tiếng Việt).
                 """,
            en = """
                 You are a mystical, wise, and empathetic Tarot Reader.
                 Format your response clearly using Markdown.
                 You give highly accurate and deeply personalized readings.
                 You MUST reply purely in English.
                 """,
            zh = """
                 You are a mystical, wise, and empathetic Tarot Reader.
                 Format your response clearly using Markdown.
                 You give highly accurate and deeply personalized readings.
                 You MUST reply purely in Chinese (繁體中文).
                 """
        };
    }

    private static object BuildInitialPromptMap()
    {
        return new
        {
            @default = BuildInitialPromptLocaleMap(),
            daily_1 = BuildInitialPromptLocaleMap(),
            spread_3 = BuildInitialPromptLocaleMap(),
            spread_5 = BuildInitialPromptLocaleMap(),
            spread_10 = BuildInitialPromptLocaleMap()
        };
    }

    private static object BuildFollowupPromptMap()
    {
        return new
        {
            @default = BuildFollowupPromptLocaleMap()
        };
    }

    private static object BuildPromptContext()
    {
        return new
        {
            defaultQuestion = BuildDefaultQuestionLocaleMap(),
            orientation = BuildOrientationPromptContext(),
            unknownCardLabel = BuildUnknownCardLabelLocaleMap()
        };
    }

    private static object BuildDefaultQuestionLocaleMap()
    {
        return new
        {
            vi = "A general reading about my current life.",
            en = "A general reading about my current life.",
            zh = "A general reading about my current life."
        };
    }

    private static object BuildOrientationPromptContext()
    {
        return new
        {
            upright = BuildOrientationLocaleMap("Upright"),
            reversed = BuildOrientationLocaleMap("Reversed")
        };
    }

    private static object BuildOrientationLocaleMap(string label)
    {
        return new
        {
            vi = label,
            en = label,
            zh = label
        };
    }

    private static object BuildUnknownCardLabelLocaleMap()
    {
        return new
        {
            vi = "Unknown Card",
            en = "Unknown Card",
            zh = "Unknown Card"
        };
    }

    private static object BuildInitialPromptLocaleMap()
    {
        return new
        {
            vi = """My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}""",
            en = """My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}""",
            zh = """My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"""
        };
    }

    private static object BuildFollowupPromptLocaleMap()
    {
        return new
        {
            vi = """Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}""",
            en = """Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}""",
            zh = """Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}"""
        };
    }
}
