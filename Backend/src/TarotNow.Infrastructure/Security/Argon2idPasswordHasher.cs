

using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

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
