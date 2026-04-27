using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

/// <summary>
/// Mã hóa dữ liệu nhạy cảm lưu DB (field-level encryption, app-managed).
/// </summary>
public sealed class SensitiveDataProtector : ISensitiveDataProtector
{
    private const string Prefix = "enc.v1";
    private readonly byte[] _key;

    public SensitiveDataProtector(IOptions<SecurityOptions> options)
    {
        var configured = options.Value.WithdrawalDataEncryptionKey?.Trim();
        if (string.IsNullOrWhiteSpace(configured))
        {
            configured = options.Value.MfaEncryptionKey?.Trim();
        }

        if (string.IsNullOrWhiteSpace(configured))
        {
            throw new InvalidOperationException("Missing Security:WithdrawalDataEncryptionKey or Security:MfaEncryptionKey configuration.");
        }

        _key = SHA256.HashData(Encoding.UTF8.GetBytes(configured));
    }

    public string Protect(string? plaintext)
    {
        if (string.IsNullOrWhiteSpace(plaintext))
        {
            return string.Empty;
        }

        if (plaintext.StartsWith($"{Prefix}.", StringComparison.Ordinal))
        {
            return plaintext;
        }

        var nonce = RandomNumberGenerator.GetBytes(12);
        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
        var cipherBytes = new byte[plainBytes.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(_key, tag.Length);
        aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

        return string.Join(
            '.',
            Prefix,
            Convert.ToBase64String(nonce),
            Convert.ToBase64String(cipherBytes),
            Convert.ToBase64String(tag));
    }

    public string Unprotect(string? protectedValue)
    {
        if (string.IsNullOrWhiteSpace(protectedValue))
        {
            return string.Empty;
        }

        if (!protectedValue.StartsWith($"{Prefix}.", StringComparison.Ordinal))
        {
            return protectedValue;
        }

        var parts = protectedValue.Split('.', 5, StringSplitOptions.None);
        if (parts.Length != 5)
        {
            throw new InvalidOperationException("Invalid encrypted payload format.");
        }

        var nonce = Convert.FromBase64String(parts[2]);
        var cipherBytes = Convert.FromBase64String(parts[3]);
        var tag = Convert.FromBase64String(parts[4]);
        var plainBytes = new byte[cipherBytes.Length];

        using var aes = new AesGcm(_key, tag.Length);
        aes.Decrypt(nonce, cipherBytes, tag, plainBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
