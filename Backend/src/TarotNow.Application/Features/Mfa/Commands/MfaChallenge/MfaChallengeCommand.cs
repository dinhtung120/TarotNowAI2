using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

// Command xác thực MFA khi user đăng nhập bằng mã TOTP hoặc backup code.
public class MfaChallengeCommand : IRequest<bool>
{
    // Định danh user đang thực hiện bước challenge MFA.
    public Guid UserId { get; set; }

    // Mã MFA người dùng nhập (TOTP hoặc backup code).
    public string Code { get; set; } = string.Empty;
}

// Handler xử lý logic challenge MFA.
public class MfaChallengeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MfaChallengeCommandHandlerRequestedDomainEvent>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    /// <summary>
    /// Khởi tạo handler challenge MFA.
    /// Luồng xử lý: nhận user repository để tải/cập nhật user và MFA service để verify mã.
    /// </summary>
    public MfaChallengeCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepo,
        IMfaService mfaService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    /// <summary>
    /// Xử lý xác thực MFA cho user.
    /// Luồng xử lý: kiểm tra điều kiện MFA đã bật, ưu tiên verify TOTP, fallback sang backup code hợp lệ một lần.
    /// </summary>
    public async Task<bool> Handle(MfaChallengeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
        {
            // Edge case: tài khoản chưa hoàn tất cấu hình MFA thì không cho chạy challenge.
            throw new BadRequestException("Tài khoản chưa bật cấu hình MFA.");
        }

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        var isValid = _mfaService.VerifyCode(plainSecret, request.Code);
        // Ưu tiên xác thực theo TOTP để không tiêu hao backup code nếu không cần.

        if (isValid)
        {
            // TOTP hợp lệ thì kết thúc sớm vì không cần thay đổi trạng thái backup code.
            return true;
        }

        if (TryConsumeBackupCode(user, request.Code))
        {
            // Backup code hợp lệ phải được "đốt" ngay để đảm bảo mỗi mã chỉ dùng đúng một lần.
            await _userRepo.UpdateAsync(user, cancellationToken);
            // Cập nhật state user sau khi đã loại bỏ backup code vừa dùng.
            return true;
        }

        // Cả TOTP và backup code đều không hợp lệ thì từ chối challenge.
        throw new BadRequestException("Mã MFA không chính xác hoặc đã hết hạn.");
    }

    protected override async Task HandleDomainEventAsync(
        MfaChallengeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }

    /// <summary>
    /// Thử tiêu thụ một backup code của user.
    /// Luồng xử lý: đọc danh sách hash backup code, so khớp mã đầu vào theo fixed-time compare, xóa mã trùng nếu tìm thấy.
    /// </summary>
    private bool TryConsumeBackupCode(Domain.Entities.User user, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(user.MfaBackupCodesHashJson))
        {
            // Không có mã đầu vào hoặc user chưa có kho backup code thì không thể fallback.
            return false;
        }

        List<string>? backupCodeHashes;
        try
        {
            backupCodeHashes = JsonSerializer.Deserialize<List<string>>(user.MfaBackupCodesHashJson);
        }
        catch
        {
            // Edge case: dữ liệu JSON bị lỗi thì coi như không còn backup code hợp lệ.
            return false;
        }

        if (backupCodeHashes is null || backupCodeHashes.Count == 0)
        {
            // Không còn mã dự phòng khả dụng.
            return false;
        }

        var matchedIndex = backupCodeHashes.FindIndex(hash => _mfaService.VerifyBackupCode(code, hash));
        // So khớp bằng fixed-time compare để giảm rủi ro timing attack khi đối chiếu hash.

        if (matchedIndex < 0)
        {
            // Không tìm thấy backup code trùng với input.
            return false;
        }

        backupCodeHashes.RemoveAt(matchedIndex);
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        // Thay đổi state user: xóa mã đã dùng và lưu lại danh sách mới.

        return true;
    }
}
