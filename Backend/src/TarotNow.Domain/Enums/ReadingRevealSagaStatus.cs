namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái vòng đời của saga reveal reading theo session.
/// </summary>
public static class ReadingRevealSagaStatus
{
    public const string Processing = "processing";
    public const string Failed = "failed";
    public const string Completed = "completed";
    public const string Compensated = "compensated";
}
