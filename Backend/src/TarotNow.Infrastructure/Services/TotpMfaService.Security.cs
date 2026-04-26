using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Infrastructure.Services;

public partial class TotpMfaService
{
    /// <summary>
    /// Băm backup code bằng PBKDF2 + salt để chống brute-force offline.
    /// </summary>
    public string HashBackupCode(string code)
    {
        var normalizedCode = NormalizeBackupCode(code);
        var salt = RandomNumberGenerator.GetBytes(16);
        var derived = Rfc2898DeriveBytes.Pbkdf2(
            normalizedCode,
            salt,
            _kdfIterations,
            HashAlgorithmName.SHA256,
            32);

        return $"{BackupHashAlgorithm}:{_kdfIterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(derived)}";
    }

    /// <summary>
    /// Xác minh backup code đầu vào theo hash đã lưu (hỗ trợ cả format legacy).
    /// </summary>
    public bool VerifyBackupCode(string code, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        if (storedHash.StartsWith($"{BackupHashAlgorithm}:", StringComparison.Ordinal))
        {
            var parts = storedHash.Split(':', 4, StringSplitOptions.TrimEntries);
            if (parts.Length != 4 || !int.TryParse(parts[1], out var iterations))
            {
                return false;
            }

            try
            {
                var salt = Convert.FromBase64String(parts[2]);
                var expectedHash = Convert.FromBase64String(parts[3]);
                var normalizedCode = NormalizeBackupCode(code);
                var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    normalizedCode,
                    salt,
                    Math.Max(100_000, iterations),
                    HashAlgorithmName.SHA256,
                    expectedHash.Length);
                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch
            {
                return false;
            }
        }

        // Legacy SHA256 hex fallback để tương thích dữ liệu cũ trước khi rotate hash.
        var legacyHash = SHA256.HashData(Encoding.UTF8.GetBytes(code.Trim()));
        var legacyHashHex = Convert.ToHexString(legacyHash).ToLowerInvariant();
        var left = Encoding.UTF8.GetBytes(legacyHashHex);
        var right = Encoding.UTF8.GetBytes(storedHash.Trim());
        return left.Length == right.Length && CryptographicOperations.FixedTimeEquals(left, right);
    }

    /// <summary>
    /// Mã hóa secret TOTP trước khi lưu trữ.
    /// Luồng dùng AES-GCM + PBKDF2(salt) để có cả confidentiality và integrity.
    /// </summary>
    public string EncryptSecret(string plainSecret)
    {
        if (string.IsNullOrWhiteSpace(plainSecret))
        {
            throw new InvalidOperationException("MFA secret cannot be empty.");
        }

        var salt = RandomNumberGenerator.GetBytes(16);
        var nonce = RandomNumberGenerator.GetBytes(12);
        var key = DeriveEncryptionKey(salt);
        var plainBytes = Encoding.UTF8.GetBytes(plainSecret);
        var cipherBytes = new byte[plainBytes.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(key, tag.Length);
        aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

        return string.Join(
            '.',
            EncryptionVersionV2Prefix,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(nonce),
            Convert.ToBase64String(tag),
            Convert.ToBase64String(cipherBytes));
    }

    /// <summary>
    /// Giải mã secret TOTP đã lưu.
    /// Luồng tách IV và ciphertext từ payload Base64 rồi giải mã bằng cùng khóa AES dẫn xuất.
    /// </summary>
    public string DecryptSecret(string encryptedSecret)
    {
        if (encryptedSecret.StartsWith($"{EncryptionVersionV2Prefix}.", StringComparison.Ordinal))
        {
            return DecryptV2(encryptedSecret);
        }

        return DecryptLegacy(encryptedSecret);
    }

    private string DecryptV2(string encryptedSecret)
    {
        var parts = encryptedSecret.Split('.', 5, StringSplitOptions.TrimEntries);
        if (parts.Length != 5)
        {
            throw new InvalidOperationException("Invalid MFA secret payload.");
        }

        var salt = Convert.FromBase64String(parts[1]);
        var nonce = Convert.FromBase64String(parts[2]);
        var tag = Convert.FromBase64String(parts[3]);
        var cipherText = Convert.FromBase64String(parts[4]);
        var plainBytes = new byte[cipherText.Length];
        var key = DeriveEncryptionKey(salt);

        using var aes = new AesGcm(key, tag.Length);
        aes.Decrypt(nonce, cipherText, tag, plainBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private string DecryptLegacy(string encryptedSecret)
    {
        var fullBytes = Convert.FromBase64String(encryptedSecret);

        using var aes = Aes.Create();
        aes.Key = _legacyEncryptionKey;

        // Tách block đầu làm IV, phần còn lại là ciphertext.
        var iv = new byte[aes.BlockSize / 8];
        var cipherText = new byte[fullBytes.Length - iv.Length];

        Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullBytes, iv.Length, cipherText, 0, cipherText.Length);
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }

    private byte[] DeriveEncryptionKey(byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            _masterKeyBytes,
            salt,
            _kdfIterations,
            HashAlgorithmName.SHA256,
            32);
    }

    private static string NormalizeBackupCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }
}
