namespace TarotNow.Infrastructure.Options;

public sealed class RedisRequirementOptions
{
    public const string SectionName = "Redis";

    public bool? RequireRedis { get; init; }
}
