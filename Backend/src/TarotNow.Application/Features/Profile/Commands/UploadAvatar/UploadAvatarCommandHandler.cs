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

        // Validate Content Type
        if (!request.ContentType.StartsWith("image/"))
        {
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        // Nén và resize ảnh
        using var compressedStream = await _imageProcessingService.CompressAsync(request.ImageStream, maxDimension: 512, quality: 80, cancellationToken);
        
        // File luôn được encode sang WebP, do đó phải ép đổi đuôi file để StaticFiles nhận diện MIME type chính xác
        var newFileName = System.IO.Path.ChangeExtension(request.FileName, ".webp");
        
        // Lưu file ảnh avatar
        var relativeUrl = await _fileStorageService.SaveFileAsync(compressedStream, newFileName, "avatars", cancellationToken);

        // Lưu giữ avatar URL cũ để xóa sau khi lưu DB thành công
        var oldAvatarUrl = user.AvatarUrl;

        // Cập nhật URL mới vào DB. Giữ nguyên displayName và dateOfBirth.
        user.UpdateProfile(user.DisplayName, relativeUrl, user.DateOfBirth);
        await _userRepository.UpdateAsync(user);

        // Đồng bộ hóa sang MongoDB (hiển thị ảnh trên Danh bạ /vi/readers) nếu user đóng vai trò Reader
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile != null)
        {
            readerProfile.AvatarUrl = relativeUrl;
            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
        }

        // Xóa ảnh cũ (nếu là local file, không phải URL bên ngoài)
        if (!string.IsNullOrEmpty(oldAvatarUrl) && !oldAvatarUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
        {
            await _fileStorageService.DeleteFileAsync(oldAvatarUrl, cancellationToken);
        }

        return relativeUrl;
    }
}
