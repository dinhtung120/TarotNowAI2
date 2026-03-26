/*
 * FILE: Argon2idPasswordHasher.cs
 * MỤC ĐÍCH: Implementation băm/xác minh mật khẩu bằng thuật toán Argon2id.
 *
 *   TẠI SAO ARGON2ID THAY VÌ BCRYPT?
 *   → Argon2id là CHUẨN MÃ HÓA MẬT KHẨU HIỆN ĐẠI (RFC 9106, thắng Password Hashing Competition 2015).
 *   → Kết hợp ưu điểm của Argon2i (chống side-channel) và Argon2d (chống GPU).
 *   → Memory-hard: đòi hỏi NHIỀU RAM → GPU/ASIC rất khó brute-force (bcrypt chỉ CPU-hard).
 *   → Tham số mặc định của thư viện Isopoh đã tối ưu (64MB RAM, 3 iterations, 4 parallel).
 *
 *   CÁCH HOẠT ĐỘNG:
 *   → Hash: password → salt (random) + Argon2id → chuỗi hash (chứa cả tham số + salt).
 *   → Verify: hash + password → tách tham số + salt từ hash → băm lại → so sánh.
 *   → Không cần lưu salt riêng — salt được nhúng trong chuỗi hash.
 */

using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

/// <summary>
/// Implement IPasswordHasher — băm mật khẩu an toàn bằng Argon2id.
/// </summary>
public class Argon2idPasswordHasher : IPasswordHasher
{
    private const int DefaultMemoryKb = 19456;
    private const int DefaultIterations = 2;
    private const int DefaultParallelism = 1;

    private readonly int _memoryCost;
    private readonly int _timeCost;
    private readonly int _parallelism;

    public Argon2idPasswordHasher(IOptions<Argon2Options> options)
    {
        var configured = options.Value;

        var configuredParallelism = ReadPositiveInt(configured.Parallelism, DefaultParallelism, min: 1, max: 4);
        _parallelism = configuredParallelism;

        var configuredMemory = ReadPositiveInt(configured.MemoryKB, DefaultMemoryKb, min: 8 * 1024, max: 1_048_576);
        _memoryCost = NormalizeMemoryCost(configuredMemory, _parallelism);

        _timeCost = ReadPositiveInt(configured.Iterations, DefaultIterations, min: 1, max: 10);
    }

    /// <summary>
    /// Băm mật khẩu: tạo salt ngẫu nhiên + Argon2id → chuỗi hash (chứa cả config + salt).
    /// Tham số mặc định Isopoh: 64MB memory, 3 iterations, 4 parallel lanes.
    /// </summary>
    public string HashPassword(string password)
    {
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
    /// Xác minh mật khẩu: tách salt + tham số từ hash → băm lại → so sánh.
    /// Trả về true nếu khớp, false nếu sai.
    /// Hàm này timing-safe (constant-time compare) → chống timing attack.
    /// </summary>
    public bool VerifyPassword(string hash, string providedPassword)
    {
        return Argon2.Verify(hash, providedPassword);
    }

    public bool NeedsRehash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return true;
        }

        try
        {
            var parsedConfig = new Argon2Config();
            if (!parsedConfig.DecodeString(hash, out _))
            {
                return true;
            }

            return parsedConfig.Type != Argon2Type.HybridAddressing ||
                   parsedConfig.Version != Argon2Version.Nineteen ||
                   parsedConfig.MemoryCost != _memoryCost ||
                   parsedConfig.TimeCost != _timeCost ||
                   parsedConfig.Lanes != _parallelism;
        }
        catch
        {
            return true;
        }
    }

    private static int ReadPositiveInt(int configuredValue, int fallback, int min, int max)
    {
        if (configuredValue <= 0)
        {
            return fallback;
        }

        return Math.Clamp(configuredValue, min, max);
    }

    private static int NormalizeMemoryCost(int memoryCost, int lanes)
    {
        var minimum = 2 * Argon2.SyncPointCount * lanes;
        var adjusted = Math.Max(memoryCost, minimum);
        var segment = Argon2.SyncPointCount * lanes;
        return adjusted - (adjusted % segment);
    }
}
