namespace TarotNow.Infrastructure.Options;

public sealed class Argon2Options
{
    public int MemoryKB { get; set; } = 19456;
    public int Iterations { get; set; } = 2;
    public int Parallelism { get; set; } = 1;
}
