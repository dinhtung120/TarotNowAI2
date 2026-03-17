namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại câu hỏi trong escrow.
/// main_question: câu hỏi chính (tạo khi accept offer).
/// add_question: câu hỏi bổ sung (cộng dồn escrow).
/// </summary>
public static class QuestionItemType
{
    public const string MainQuestion = "main_question";
    public const string AddQuestion = "add_question";
}
