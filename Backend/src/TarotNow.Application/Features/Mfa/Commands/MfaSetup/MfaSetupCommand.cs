using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

// Command khởi tạo dữ liệu MFA (secret, QR, backup codes) cho user.
public class MfaSetupCommand : IRequest<MfaSetupResult>
{
    // Định danh user cần thiết lập MFA.
    public Guid UserId { get; set; }
}

// DTO kết quả setup MFA trả về cho client.
public class MfaSetupResult
{
    // URI otpauth để client render QR code cho ứng dụng authenticator.
    public string QrCodeUri { get; set; } = string.Empty;

    // Secret dạng text để hiển thị fallback khi không quét được QR.
    public string SecretDisplay { get; set; } = string.Empty;

    // Danh sách backup code dạng plain-text trả về đúng một lần cho user lưu lại.
    public List<string> BackupCodes { get; set; } = new();
}

// Handler xử lý luồng setup MFA.
public class MfaSetupCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MfaSetupCommandHandlerRequestedDomainEvent>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    /// <summary>
    /// Khởi tạo handler setup MFA.
    /// Luồng xử lý: nhận user repository để đọc/cập nhật trạng thái người dùng và MFA service để sinh dữ liệu bảo mật.
    /// </summary>
    public MfaSetupCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepo,
        IMfaService mfaService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    /// <summary>
    /// Xử lý thiết lập MFA cho user.
    /// Luồng xử lý: kiểm tra điều kiện setup, sinh secret + QR + backup codes, lưu secret/backup hash và trả dữ liệu hiển thị.
    /// </summary>
    public async Task<MfaSetupResult> Handle(MfaSetupCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        if (user.MfaEnabled)
        {
            // Business rule: không cho setup chồng khi MFA đang bật để tránh ghi đè cấu hình đang hoạt động.
            throw new BadRequestException("MFA đã được bật. Nếu muốn thiết lập lại, vui lòng tắt MFA trước.");
        }

        var plainSecret = _mfaService.GenerateSecretKey();
        var encryptedSecret = _mfaService.EncryptSecret(plainSecret);
        var qrUri = _mfaService.GenerateQrCodeUri(plainSecret, user.Email);
        // Sinh đầy đủ dữ liệu onboarding từ cùng một secret để đồng bộ giữa QR và verify code.

        var backupCodes = _mfaService.GenerateBackupCodes();
        var backupCodeHashes = backupCodes.Select(_mfaService.HashBackupCode).ToList();
        // Chỉ lưu hash backup code trong DB để giảm rủi ro lộ mã dùng một lần.

        user.MfaSecretEncrypted = encryptedSecret;
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        // Thay đổi state bảo mật: ghi secret đã mã hóa và bộ backup code hash cho user.

        await _userRepo.UpdateAsync(user, cancellationToken);
        // Persist thay đổi trước khi trả kết quả để tránh client nhận dữ liệu nhưng DB chưa cập nhật.

        return new MfaSetupResult
        {
            QrCodeUri = qrUri,
            SecretDisplay = plainSecret,
            BackupCodes = backupCodes
        };
    }

    protected override async Task HandleDomainEventAsync(
        MfaSetupCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
