

using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

public class TotpMfaService : IMfaService
{
    private const string Issuer = "TarotNowAI"; 
    private readonly byte[] _encryptionKey; 

    public TotpMfaService(IOptions<SecurityOptions> options)
    {
        
        var configuredKey = options.Value.MfaEncryptionKey?.Trim();
        if (string.IsNullOrWhiteSpace(configuredKey))
            throw new InvalidOperationException("Missing Security:MfaEncryptionKey configuration.");

        
        if (configuredKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Security:MfaEncryptionKey is not configured with a real secret.");

        
        _encryptionKey = SHA256.HashData(Encoding.UTF8.GetBytes(configuredKey));
    }

        public string GenerateSecretKey()
    {
        var secret = KeyGeneration.GenerateRandomKey(20); 
        return Base32Encoding.ToString(secret);
    }

        public string GenerateQrCodeUri(string plainSecret, string userEmail)
    {
        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);
        
        var encodedIssuer = Uri.EscapeDataString(Issuer);
        var encodedEmail = Uri.EscapeDataString(userEmail);
        
        return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={plainSecret}&issuer={encodedIssuer}";
    }

        public bool VerifyCode(string plainSecret, string code)
    {
        if (string.IsNullOrWhiteSpace(plainSecret) || string.IsNullOrWhiteSpace(code)) return false;

        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);
        
        
        return totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));
    }

        public List<string> GenerateBackupCodes(int count = 6)
    {
        var result = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var bytes = new byte[4]; 
            RandomNumberGenerator.Fill(bytes);
            var code = BitConverter.ToUInt32(bytes, 0) % 100000000; 
            result.Add(code.ToString("D8")); 
        }
        return result;
    }

        public string EncryptSecret(string plainSecret)
    {
        using var aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV(); 

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainSecret);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

        public string DecryptSecret(string encryptedSecret)
    {
        var fullBytes = Convert.FromBase64String(encryptedSecret);

        using var aes = Aes.Create();
        aes.Key = _encryptionKey;

        
        var iv = new byte[aes.BlockSize / 8]; 
        var cipherText = new byte[fullBytes.Length - iv.Length];

        Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullBytes, iv.Length, cipherText, 0, cipherText.Length);
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
