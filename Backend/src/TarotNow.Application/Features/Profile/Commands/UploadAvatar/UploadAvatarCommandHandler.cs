using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.UserImageUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

// Handler upload avatar qua pipeline R2/local, đồng bộ URL + object key, xóa object cũ.
public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, UploadAvatarResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserImagePipeline _userImagePipeline;
    private readonly IObjectStorageService _objectStorage;
    private readonly ILogger<UploadAvatarCommandHandler> _logger;

    public UploadAvatarCommandHandler(
        IUserRepository userRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserImagePipeline userImagePipeline,
        IObjectStorageService objectStorage,
        ILogger<UploadAvatarCommandHandler> logger)
    {
        _userRepository = userRepository;
        _readerProfileRepository = readerProfileRepository;
        _userImagePipeline = userImagePipeline;
        _objectStorage = objectStorage;
        _logger = logger;
    }

    public async Task<UploadAvatarResult> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        if (request.ImageStream is null)
        {
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        var oldObjectKey = user.AvatarObjectKey;

        var pipelineResult = await _userImagePipeline.ProcessUploadAsync(
            request.ImageStream,
            request.FileName,
            request.ContentType,
            UserImageUploadKind.Avatar,
            cancellationToken);

        try
        {
            user.ApplyManagedAvatar(pipelineResult.PublicUrl, pipelineResult.PublicId);
            
            // Song song hóa các lệnh cập nhật DB để tận dụng IO overlap.
            var updateUserTask = _userRepository.UpdateAsync(user);
            
            var updateProfileTask = Task.Run(async () => {
                var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
                if (readerProfile is not null)
                {
                    readerProfile.AvatarUrl = pipelineResult.PublicUrl;
                    await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
                }
            }, cancellationToken);

            await Task.WhenAll(updateUserTask, updateProfileTask);

            // Chuyển việc xóa ảnh cũ sang chạy nền (fire-and-forget) để giảm latency.
            if (!string.IsNullOrEmpty(oldObjectKey))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // Dùng CancellationToken.None vì đây là tác vụ nền độc lập với request hiện tại.
                        await _objectStorage.DeleteAsync(oldObjectKey, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Background Task: Không xóa được avatar object cũ key={Key}", oldObjectKey);
                    }
                });
            }

            return new UploadAvatarResult(pipelineResult.PublicUrl, pipelineResult.PublicId);
        }
        catch
        {
            try
            {
                await _objectStorage.DeleteAsync(pipelineResult.PublicId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Rollback: không xóa được object avatar mới key={Key}", pipelineResult.PublicId);
            }

            throw;
        }
    }
}
