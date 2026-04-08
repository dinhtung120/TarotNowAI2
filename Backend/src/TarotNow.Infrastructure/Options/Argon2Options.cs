namespace TarotNow.Infrastructure.Options;

// Options cấu hình tham số băm mật khẩu Argon2.
public sealed class Argon2Options
{
    // Lượng bộ nhớ Argon2 sử dụng (KB).
    public int MemoryKB { get; set; } = 19456;

    // Số vòng lặp băm.
    public int Iterations { get; set; } = 2;

    // Mức độ song song khi băm.
    public int Parallelism { get; set; } = 1;
}
