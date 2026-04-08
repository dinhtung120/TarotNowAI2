using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

// Hasher mật khẩu theo chuẩn Argon2id để cân bằng bảo mật và chi phí xử lý.
public class Argon2idPasswordHasher : IPasswordHasher
{
    // Cấu hình mặc định an toàn khi chưa có giá trị hợp lệ từ appsettings.
    private const int DefaultMemoryKb = 19456;
    private const int DefaultIterations = 2;
    private const int DefaultParallelism = 1;

    // Tham số hash đã được chuẩn hóa để dùng nhất quán cho hash và verify.
    private readonly int _memoryCost;
    private readonly int _timeCost;
    private readonly int _parallelism;

    /// <summary>
    /// Khởi tạo hasher và chuẩn hóa tham số từ cấu hình.
    /// Luồng này đảm bảo tham số luôn trong ngưỡng an toàn để tránh cấu hình sai gây yếu bảo mật hoặc lỗi runtime.
    /// </summary>
    public Argon2idPasswordHasher(IOptions<Argon2Options> options)
    {
        var configured = options.Value;

        // Chuẩn hóa số lane để tránh cấu hình quá cao làm tăng chi phí CPU không cần thiết.
        var configuredParallelism = ReadPositiveInt(configured.Parallelism, DefaultParallelism, min: 1, max: 4);
        _parallelism = configuredParallelism;

        // Chuẩn hóa memory cost và căn theo segment Argon2 để thuật toán chạy đúng chuẩn.
        var configuredMemory = ReadPositiveInt(configured.MemoryKB, DefaultMemoryKb, min: 8 * 1024, max: 1_048_576);
        _memoryCost = NormalizeMemoryCost(configuredMemory, _parallelism);

        // Chuẩn hóa số vòng lặp để giữ cân bằng giữa độ an toàn và độ trễ đăng nhập.
        _timeCost = ReadPositiveInt(configured.Iterations, DefaultIterations, min: 1, max: 10);
    }

    /// <summary>
    /// Băm mật khẩu đầu vào theo tham số Argon2id đã chuẩn hóa.
    /// Luồng dùng chung một bộ tham số để tạo hash đồng nhất cho toàn hệ thống.
    /// </summary>
    public string HashPassword(string password)
    {
        // Hash length cố định giúp lưu trữ và so sánh định dạng nhất quán.
        return Argon2.Hash(
            password: password,
            timeCost: _timeCost,
            memoryCost: _memoryCost,
            parallelism: _parallelism,
            type: Argon2Type.HybridAddressing,
            hashLength: 32
        );
    }

    /// <summary>
    /// Xác minh mật khẩu người dùng nhập với hash đã lưu.
    /// Luồng verify tách biệt để tái sử dụng cho đăng nhập và đổi mật khẩu.
    /// </summary>
    public bool VerifyPassword(string hash, string providedPassword)
    {
        return Argon2.Verify(hash, providedPassword);
    }

    /// <summary>
    /// Kiểm tra hash hiện tại có cần băm lại theo cấu hình mới hay không.
    /// Luồng này hỗ trợ nâng chuẩn bảo mật dần mà không bắt buộc reset mật khẩu hàng loạt.
    /// </summary>
    public bool NeedsRehash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            // Hash rỗng/không hợp lệ luôn được coi là cần băm lại để tự phục hồi dữ liệu yếu.
            return true;
        }

        try
        {
            var parsedConfig = new Argon2Config();
            if (!parsedConfig.DecodeString(hash, out _))
            {
                // Không parse được định dạng hash thì yêu cầu rehash để tránh duy trì trạng thái lỗi.
                return true;
            }

            // So sánh toàn bộ tham số quan trọng để phát hiện hash cũ không còn đạt chính sách hiện tại.
            return parsedConfig.Type != Argon2Type.HybridAddressing ||
                   parsedConfig.Version != Argon2Version.Nineteen ||
                   parsedConfig.MemoryCost != _memoryCost ||
                   parsedConfig.TimeCost != _timeCost ||
                   parsedConfig.Lanes != _parallelism;
        }
        catch
        {
            // Bất kỳ lỗi decode nào cũng ép rehash để ưu tiên an toàn bảo mật.
            return true;
        }
    }

    /// <summary>
    /// Chuẩn hóa số nguyên dương theo ngưỡng min/max cho tham số bảo mật.
    /// Luồng này ngăn cấu hình âm, bằng 0 hoặc vượt biên gây hành vi bất định.
    /// </summary>
    private static int ReadPositiveInt(int configuredValue, int fallback, int min, int max)
    {
        if (configuredValue <= 0)
        {
            // Dùng fallback khi cấu hình thiếu hoặc sai để hệ thống vẫn chạy với mức an toàn tối thiểu.
            return fallback;
        }

        return Math.Clamp(configuredValue, min, max);
    }

    /// <summary>
    /// Căn chỉnh memory cost theo bội số segment bắt buộc của Argon2.
    /// Luồng này tránh tham số không tương thích với số lane dẫn đến hash không tối ưu.
    /// </summary>
    private static int NormalizeMemoryCost(int memoryCost, int lanes)
    {
        var minimum = 2 * Argon2.SyncPointCount * lanes;
        var adjusted = Math.Max(memoryCost, minimum);
        var segment = Argon2.SyncPointCount * lanes;
        // Làm tròn xuống theo segment để tham số cuối cùng hợp lệ với nội bộ Argon2.
        return adjusted - (adjusted % segment);
    }
}
