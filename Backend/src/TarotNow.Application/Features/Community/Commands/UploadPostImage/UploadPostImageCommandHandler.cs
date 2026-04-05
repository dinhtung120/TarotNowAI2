using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

public class UploadPostImageCommandHandler : IRequestHandler<UploadPostImageCommand, string>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;

    public UploadPostImageCommandHandler(
        IFileStorageService fileStorageService,
        IImageProcessingService imageProcessingService)
    {
        _fileStorageService = fileStorageService;
        _imageProcessingService = imageProcessingService;
    }

    public async Task<string> Handle(UploadPostImageCommand request, CancellationToken cancellationToken)
    {
        if (request.ImageStream == null || request.ImageStream.Length == 0)
        {
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        // Validate Content Type
        if (!request.ContentType.StartsWith("image/"))
        {
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        // Nén và resize ảnh (Kích thước lớn hơn Avatar một chút)
        using var compressedStream = await _imageProcessingService.CompressAsync(request.ImageStream, maxDimension: 1024, quality: 80, cancellationToken);
        
        // Encode sang WebP
        var newFileName = System.IO.Path.ChangeExtension(request.FileName, ".webp");
        
        // Lưu ảnh bài viết Community
        var relativeUrl = await _fileStorageService.SaveFileAsync(compressedStream, newFileName, "community", cancellationToken);

        return relativeUrl;
    }
}
