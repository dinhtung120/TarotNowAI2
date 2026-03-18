using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

/// <summary>
/// Challenge: User nhập code để vượt qua chốt chặn (gate) dành cho Payout/Admin action.
/// </summary>
public class MfaChallengeCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = string.Empty;
}

public class MfaChallengeCommandHandler : IRequestHandler<MfaChallengeCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaChallengeCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<bool> Handle(MfaChallengeCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
            throw new BadRequestException("Tài khoản chưa bật cấu hình MFA.");

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        var isValid = _mfaService.VerifyCode(plainSecret, req.Code);
        if (isValid) return true;

        if (TryConsumeBackupCode(user, req.Code))
        {
            await _userRepo.UpdateAsync(user, ct);
            return true;
        }

        throw new BadRequestException("Mã MFA không chính xác hoặc đã hết hạn.");
    }

    private static bool TryConsumeBackupCode(Domain.Entities.User user, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(user.MfaBackupCodesHashJson))
            return false;

        List<string>? backupCodeHashes;
        try
        {
            backupCodeHashes = JsonSerializer.Deserialize<List<string>>(user.MfaBackupCodesHashJson);
        }
        catch
        {
            return false;
        }

        if (backupCodeHashes == null || backupCodeHashes.Count == 0)
            return false;

        var hashedInput = HashBackupCode(code);
        var matchedIndex = backupCodeHashes.FindIndex(hash => FixedTimeEquals(hash, hashedInput));
        if (matchedIndex < 0) return false;

        backupCodeHashes.RemoveAt(matchedIndex);
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        return true;
    }

    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
            && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
