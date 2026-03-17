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

        if (!isValid)
            throw new BadRequestException("Mã MFA không chính xác hoặc đã hết hạn.");

        // Verification okay.
        return true;
    }
}
