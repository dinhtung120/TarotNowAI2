using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.ConfirmAvatarUpload;

/// <summary>
/// Command xác nhận upload avatar thành công trên R2.
/// </summary>
public sealed class ConfirmAvatarUploadCommand : IRequest<ConfirmAvatarUploadResult>
{
    /// <summary>Định danh user xác nhận avatar.</summary>
    public Guid UserId { get; set; }

    /// <summary>Object key vừa upload lên R2.</summary>
    public string ObjectKey { get; set; } = string.Empty;

    /// <summary>Public URL vừa upload lên R2.</summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>Upload token one-time do endpoint presign cấp.</summary>
    public string UploadToken { get; set; } = string.Empty;
}

/// <summary>
/// Kết quả confirm avatar upload.
/// </summary>
public sealed record ConfirmAvatarUploadResult(string AvatarUrl, string ObjectKey);

/// <summary>
/// Handler xác nhận avatar upload.
/// </summary>
public sealed class ConfirmAvatarUploadCommandExecutor : ICommandExecutionExecutor<ConfirmAvatarUploadCommand, ConfirmAvatarUploadResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUploadSessionRepository _uploadSessionRepository;
    private readonly IR2UploadService _r2UploadService;
    private readonly ILogger<ConfirmAvatarUploadCommandExecutor> _logger;

    /// <summary>
    /// Khởi tạo handler confirm avatar.
    /// </summary>
    public ConfirmAvatarUploadCommandExecutor(
        IUserRepository userRepository,
        IReaderProfileRepository readerProfileRepository,
        IUploadSessionRepository uploadSessionRepository,
        IR2UploadService r2UploadService,
        ILogger<ConfirmAvatarUploadCommandExecutor> logger)
    {
        _userRepository = userRepository;
        _readerProfileRepository = readerProfileRepository;
        _uploadSessionRepository = uploadSessionRepository;
        _r2UploadService = r2UploadService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ConfirmAvatarUploadResult> Handle(ConfirmAvatarUploadCommand request, CancellationToken cancellationToken)
    {
        EnsureR2UploadEnabled();
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        var session = await _uploadSessionRepository.GetByTokenAsync(request.UploadToken, cancellationToken)
            ?? throw new BadRequestException("Upload token không tồn tại.");

        ValidateUploadSession(session, request);
        var consumed = await _uploadSessionRepository.ConsumeAsync(request.UploadToken, DateTime.UtcNow, cancellationToken);
        if (!consumed)
        {
            throw new BadRequestException("Upload token đã được dùng hoặc đã hết hạn.");
        }

        var oldObjectKey = user.AvatarObjectKey;
        user.ApplyManagedAvatar(request.PublicUrl, request.ObjectKey);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await UpdateReaderAvatarIfExistsAsync(user.Id, request.PublicUrl, cancellationToken);
        await TryDeleteOldAvatarAsync(oldObjectKey, request.ObjectKey, cancellationToken);

        return new ConfirmAvatarUploadResult(request.PublicUrl, request.ObjectKey);
    }

    private void EnsureR2UploadEnabled()
    {
        if (_r2UploadService.IsEnabled == false)
        {
            throw new BadRequestException("R2 upload chưa được cấu hình. Vui lòng kiểm tra ObjectStorage:R2.");
        }
    }

    private static void ValidateUploadSession(UploadSessionRecord session, ConfirmAvatarUploadCommand request)
    {
        if (session.OwnerUserId != request.UserId)
        {
            throw new BadRequestException("Upload token không thuộc user hiện tại.");
        }

        if (!string.Equals(session.Scope, MediaUploadConstants.ScopeAvatar, StringComparison.Ordinal))
        {
            throw new BadRequestException("Upload token scope không hợp lệ cho avatar.");
        }

        if (session.ConsumedAtUtc.HasValue || session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new BadRequestException("Upload token đã hết hạn hoặc đã consume.");
        }

        if (!string.Equals(session.ObjectKey, request.ObjectKey, StringComparison.Ordinal))
        {
            throw new BadRequestException("Object key không khớp với upload token.");
        }

        if (!string.Equals(session.PublicUrl, request.PublicUrl, StringComparison.Ordinal))
        {
            throw new BadRequestException("Public URL không khớp với upload token.");
        }
    }

    private async Task UpdateReaderAvatarIfExistsAsync(Guid userId, string avatarUrl, CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(userId.ToString(), cancellationToken);
        if (readerProfile is null)
        {
            return;
        }

        readerProfile.AvatarUrl = avatarUrl;
        await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
    }

    private async Task TryDeleteOldAvatarAsync(string? oldObjectKey, string newObjectKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(oldObjectKey) || string.Equals(oldObjectKey, newObjectKey, StringComparison.Ordinal))
        {
            return;
        }

        try
        {
            await _r2UploadService.DeleteObjectAsync(oldObjectKey, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không xóa được avatar cũ key={ObjectKey}", oldObjectKey);
        }
    }
}
