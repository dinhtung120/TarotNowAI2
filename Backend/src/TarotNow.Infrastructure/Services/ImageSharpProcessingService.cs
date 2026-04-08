using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service nén và chuẩn hóa ảnh đầu vào bằng ImageSharp.
public class ImageSharpProcessingService : IImageProcessingService
{
    // Logger để theo dõi lỗi xử lý media ở tầng hạ tầng.
    private readonly ILogger<ImageSharpProcessingService> _logger;

    /// <summary>
    /// Khởi tạo service xử lý ảnh.
    /// Luồng inject logger để đảm bảo lỗi media được ghi nhận rõ ràng.
    /// </summary>
    public ImageSharpProcessingService(ILogger<ImageSharpProcessingService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Nén ảnh đầu vào về kích thước và chất lượng mục tiêu.
    /// Luồng reset stream nếu cần, resize theo cạnh lớn nhất, xóa metadata nhạy cảm rồi encode WebP.
    /// </summary>
    public async Task<Stream> CompressAsync(Stream input, int maxDimension = 512, int quality = 80, CancellationToken ct = default)
    {
        try
        {
            // Đưa con trỏ stream về đầu để tránh lỗi đọc thiếu dữ liệu khi stream đã được đọc trước đó.
            if (input.CanSeek && input.Position > 0)
            {
                input.Position = 0;
            }

            using var image = await Image.LoadAsync(input, ct);

            var ratio = Math.Min(1.0, (double)maxDimension / Math.Max(image.Width, image.Height));
            if (ratio < 1.0)
            {
                var targetWidth = (int)Math.Max(1, Math.Round(image.Width * ratio));
                var targetHeight = (int)Math.Max(1, Math.Round(image.Height * ratio));
                // Resize bằng Lanczos3 để giữ chất lượng tốt khi giảm kích thước.
                image.Mutate(x => x.Resize(targetWidth, targetHeight, KnownResamplers.Lanczos3));
            }

            // Xóa metadata để giảm kích thước và loại bỏ thông tin nhạy cảm.
            image.Metadata.ExifProfile = null;
            image.Metadata.XmpProfile = null;
            image.Metadata.IptcProfile = null;

            var resultStream = new MemoryStream();
            await image.SaveAsWebpAsync(resultStream, new WebpEncoder { Quality = quality }, ct);

            // Reset vị trí stream để caller có thể đọc ngay kết quả nén.
            resultStream.Position = 0;
            return resultStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xử lý hình ảnh qua ImageSharp");
            throw;
        }
    }
}
