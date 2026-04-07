

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

public class MfaVerifyCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
        public string Code { get; set; } = string.Empty;
}

public class MfaVerifyCommandHandler : IRequestHandler<MfaVerifyCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaVerifyCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<bool> Handle(MfaVerifyCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        
        if (user.MfaEnabled)
            throw new BadRequestException("MFA đã được bật rồi.");

        
        if (string.IsNullOrEmpty(user.MfaSecretEncrypted))
            throw new BadRequestException("Vui lòng thực hiện bước Setup trước khi Verify.");

        
        
        
        
        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        
        
        var isValid = _mfaService.VerifyCode(plainSecret, req.Code);

        
        if (!isValid)
            throw new BadRequestException("Mã xác thực không hợp lệ hoặc đã hết hạn.");

        
        user.MfaEnabled = true;
        await _userRepo.UpdateAsync(user, ct);

        return true;
    }
}
