using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UploadAvatarCommandHandler(
        IUserRepository userRepository,
        IFileStorageService fileStorageService,
        IImageProcessingService imageProcessingService,
        IReaderProfileRepository readerProfileRepository)
    {
        _userRepository = userRepository;
        _fileStorageService = fileStorageService;
        _imageProcessingService = imageProcessingService;
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        if (request.ImageStream == null || request.ImageStream.Length == 0)
        {
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        
        if (!request.ContentType.StartsWith("image/"))
        {
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        
        using var compressedStream = await _imageProcessingService.CompressAsync(request.ImageStream, maxDimension: 512, quality: 80, cancellationToken);
        
        
        var newFileName = System.IO.Path.ChangeExtension(request.FileName, ".webp");
        
        
        var relativeUrl = await _fileStorageService.SaveFileAsync(compressedStream, newFileName, "avatars", cancellationToken);

        
        var oldAvatarUrl = user.AvatarUrl;

        
        user.UpdateProfile(user.DisplayName, relativeUrl, user.DateOfBirth);
        await _userRepository.UpdateAsync(user);

        
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile != null)
        {
            readerProfile.AvatarUrl = relativeUrl;
            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
        }

        
        if (!string.IsNullOrEmpty(oldAvatarUrl) && !oldAvatarUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
        {
            await _fileStorageService.DeleteFileAsync(oldAvatarUrl, cancellationToken);
        }

        return relativeUrl;
    }
}
