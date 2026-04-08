using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

// Handler xử lý upload và nén ảnh bài viết community.
public class UploadPostImageCommandHandler : IRequestHandler<UploadPostImageCommand, string>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;

    /// <summary>
    /// Khởi tạo handler upload post image.
    /// Luồng xử lý: nhận service nén ảnh và lưu trữ file để xử lý pipeline upload.
    /// </summary>
    public UploadPostImageCommandHandler(
        IFileStorageService fileStorageService,
        IImageProcessingService imageProcessingService)
    {
        _fileStorageService = fileStorageService;
        _imageProcessingService = imageProcessingService;
    }

    /// <summary>
    /// Xử lý command upload ảnh bài viết.
    /// Luồng xử lý: validate stream + content type, nén ảnh sang kích thước mục tiêu, đổi đuôi file sang webp, lưu file và trả URL tương đối.
    /// </summary>
    public async Task<string> Handle(UploadPostImageCommand request, CancellationToken cancellationToken)
    {
        if (request.ImageStream == null || request.ImageStream.Length == 0)
        {
            // Không có dữ liệu ảnh hợp lệ thì dừng sớm.
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        if (!request.ContentType.StartsWith("image/"))
        {
            // Chỉ nhận file ảnh để tránh upload định dạng không an toàn.
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        // Nén ảnh về kích thước/quality chuẩn để tối ưu băng thông lưu trữ và tải.
        using var compressedStream = await _imageProcessingService.CompressAsync(request.ImageStream, maxDimension: 1024, quality: 80, cancellationToken);

        // Chuẩn hóa đuôi file về webp để thống nhất định dạng ảnh đã nén.
        var newFileName = System.IO.Path.ChangeExtension(request.FileName, ".webp");

        // Lưu vào thư mục community và trả URL tương đối để client dựng đường dẫn đầy đủ.
        var relativeUrl = await _fileStorageService.SaveFileAsync(compressedStream, newFileName, "community", cancellationToken);

        return relativeUrl;
    }
}
