
namespace TarotNow.Domain.Enums;

// Tập hằng loại kỳ nhiệm vụ theo chu kỳ thời gian.
public static class QuestType
{
    // Nhiệm vụ hằng ngày.
    public const string Daily = "daily";

    // Nhiệm vụ hằng tuần.
    public const string Weekly = "weekly";

    // Nhiệm vụ hằng tháng.
    public const string Monthly = "monthly";

    // Nhiệm vụ theo mùa/quý.
    public const string Seasonal = "seasonal";
}
