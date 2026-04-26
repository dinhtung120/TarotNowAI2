namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái vòng đời của finance session trong chat escrow.
/// </summary>
public static class ChatFinanceSessionStatus
{
    public const string Pending = "pending";
    public const string Active = "active";
    public const string Disputed = "disputed";
    public const string Completed = "completed";
    public const string Refunded = "refunded";
}
