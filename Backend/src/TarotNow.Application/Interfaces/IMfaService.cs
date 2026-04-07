

using System.Collections.Generic;

namespace TarotNow.Application.Interfaces;

public interface IMfaService
{
        string GenerateSecretKey();

        string EncryptSecret(string plainSecret);

        string DecryptSecret(string encryptedSecret);

        string GenerateQrCodeUri(string plainSecret, string userEmail);

        bool VerifyCode(string plainSecret, string code);

        List<string> GenerateBackupCodes(int count = 6);
}
