using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

// Handler upload avatar, nén ảnh và đồng bộ URL mới vào hồ sơ người dùng.
public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler upload avatar.
    /// Luồng xử lý: nhận các service xử lý ảnh, lưu file và cập nhật dữ liệu user/reader profile.
    /// </summary>
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

    /// <summary>
    /// Xử lý upload avatar mới cho user.
    /// Luồng xử lý: validate input ảnh, nén và lưu file mới, cập nhật URL avatar cho user/reader profile, rồi dọn file cũ nếu cần.
    /// </summary>
    public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        if (request.ImageStream is null || request.ImageStream.Length == 0)
        {
            // Edge case: stream rỗng hoặc null thì không thể xử lý nén/lưu ảnh.
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        if (!request.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            // Chỉ cho phép MIME image/* để chặn upload file không phải ảnh.
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        using var compressedStream = await _imageProcessingService.CompressAsync(
            request.ImageStream,
            maxDimension: 512,
            quality: 80,
            cancellationToken);
        // Chuẩn hóa kích thước/chất lượng để giảm dung lượng và đồng nhất hiển thị avatar.

        var newFileName = Path.ChangeExtension(request.FileName, ".webp");
        var relativeUrl = await _fileStorageService.SaveFileAsync(compressedStream, newFileName, "avatars", cancellationToken);
        // Lưu ảnh đã nén dưới định dạng webp để tối ưu bandwidth khi client tải avatar.

        var oldAvatarUrl = user.AvatarUrl;

        user.UpdateProfile(user.DisplayName, relativeUrl, user.DateOfBirth);
        await _userRepository.UpdateAsync(user);
        // Đổi state avatar ở hồ sơ chính của user ngay sau khi lưu file thành công.

        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile is not null)
        {
            readerProfile.AvatarUrl = relativeUrl;
            // Đồng bộ avatar cho reader profile để tránh lệch dữ liệu ở khu vực cộng đồng.

            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
        }
        // Edge case: user không có reader profile thì bỏ qua đồng bộ phụ.

        if (!string.IsNullOrEmpty(oldAvatarUrl) &&
            !oldAvatarUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            // Chỉ xóa file cũ nội bộ; URL tuyệt đối ngoài hệ thống thì không can thiệp.
            await _fileStorageService.DeleteFileAsync(oldAvatarUrl, cancellationToken);
        }

        return relativeUrl;
    }
}
