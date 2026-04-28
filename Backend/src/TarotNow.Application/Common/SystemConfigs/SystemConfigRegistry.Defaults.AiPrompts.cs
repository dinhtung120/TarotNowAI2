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
            @default = BuildInitialPromptTemplate(),
            daily_1 = BuildInitialPromptTemplate(),
            spread_3 = BuildInitialPromptTemplate(),
            spread_5 = BuildInitialPromptTemplate(),
            spread_10 = BuildInitialPromptTemplate()
        };
    }

    private static object BuildFollowupPromptMap()
    {
        return new
        {
            @default = BuildFollowupPromptTemplate()
        };
    }

    private static object BuildPromptContext()
    {
        return new
        {
            defaultQuestion = BuildDefaultQuestionTemplate(),
            orientation = BuildOrientationPromptContext(),
            unknownCardLabel = BuildUnknownCardLabelTemplate()
        };
    }

    private static string BuildDefaultQuestionTemplate()
        => "A general reading about my current life.";

    private static object BuildOrientationPromptContext()
    {
        return new
        {
            upright = "Upright",
            reversed = "Reversed"
        };
    }

    private static string BuildUnknownCardLabelTemplate()
        => "Unknown Card";

    private static string BuildInitialPromptTemplate()
        => """My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}""";

    private static string BuildFollowupPromptTemplate()
        => """Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}""";
}
