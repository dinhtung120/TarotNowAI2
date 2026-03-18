using MediatR;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

/// <summary>
/// Bước 1: Setup MFA.
/// Generate secret, encrypt và lưu vào User, trả về URI để quét QR + Backup Codes.
/// Ghi chú: Lúc này MfaEnabled vẫn = false, chỉ đổi khi User verify code ở bước 2.
/// </summary>
public class MfaSetupCommand : IRequest<MfaSetupResult>
{
    public Guid UserId { get; set; }
}

public class MfaSetupResult
{
    public string QrCodeUri { get; set; } = string.Empty;
    public string SecretDisplay { get; set; } = string.Empty;
    public List<string> BackupCodes { get; set; } = new();
}

public class MfaSetupCommandHandler : IRequestHandler<MfaSetupCommand, MfaSetupResult>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaSetupCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<MfaSetupResult> Handle(MfaSetupCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        if (user.MfaEnabled)
            throw new BadRequestException("MFA đã được bật. Nếu muốn thiết lập lại, vui lòng tắt MFA trước.");

        // Generate mới
        var plainSecret = _mfaService.GenerateSecretKey();
        var encryptedSecret = _mfaService.EncryptSecret(plainSecret);
        var qrUri = _mfaService.GenerateQrCodeUri(plainSecret, user.Email);
        var backupCodes = _mfaService.GenerateBackupCodes();
        var backupCodeHashes = backupCodes.Select(HashBackupCode).ToList();

        // Lưu secret vào User (chưa enable)
        user.MfaSecretEncrypted = encryptedSecret;
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        
        await _userRepo.UpdateAsync(user, ct);

        return new MfaSetupResult
        {
            QrCodeUri = qrUri,
            SecretDisplay = plainSecret,
            BackupCodes = backupCodes
        };
    }

    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
