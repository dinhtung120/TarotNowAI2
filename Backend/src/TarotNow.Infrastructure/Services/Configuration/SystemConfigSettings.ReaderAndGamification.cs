namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Số năm kinh nghiệm tối thiểu cho Reader.
    public int ReaderMinYearsOfExperience => ResolvePositiveInt(
        ReadInt(["reader.min_years_of_experience"], _options.Reader.MinYearsOfExperience),
        fallback: 1);

    // Giá Diamond/câu hỏi tối thiểu cho Reader.
    public long ReaderMinDiamondPerQuestion => ResolvePositiveLong(
        ReadLong(["reader.min_diamond_per_question"], _options.Reader.MinDiamondPerQuestion),
        fallback: 50);

    // Quest type mặc định cho gamification.
    public string GamificationDefaultQuestType
    {
        get
        {
            var fromConfig = NormalizeQuestType(ReadString("gamification.default_quest_type"));
            if (fromConfig is not null)
            {
                return fromConfig;
            }

            return NormalizeQuestType(_options.Gamification.DefaultQuestType) ?? "daily";
        }
    }

    // Leaderboard track mặc định cho gamification.
    public string GamificationDefaultLeaderboardTrack
    {
        get
        {
            var fromConfig = NormalizeLeaderboardTrack(ReadString("gamification.default_leaderboard_track"));
            if (fromConfig is not null)
            {
                return fromConfig;
            }

            return NormalizeLeaderboardTrack(_options.Gamification.DefaultLeaderboardTrack) ?? "spent_gold_daily";
        }
    }

    private static string? NormalizeQuestType(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        var normalized = rawValue.Trim().ToLowerInvariant();
        return normalized is "daily" or "weekly" ? normalized : null;
    }

    private static string? NormalizeLeaderboardTrack(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        var normalized = rawValue.Trim().ToLowerInvariant();

        return normalized.Length > 64
            ? null
            : System.Text.RegularExpressions.Regex.IsMatch(normalized, "^[a-z0-9_]+$")
                ? normalized
                : null;
    }
}
