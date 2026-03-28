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

public class ImageSharpProcessingService : IImageProcessingService
{
    private readonly ILogger<ImageSharpProcessingService> _logger;

    public ImageSharpProcessingService(ILogger<ImageSharpProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<Stream> CompressAsync(Stream input, int maxDimension = 512, int quality = 80, CancellationToken ct = default)
    {
        try
        {
            // Reset position if it's not at the beginning
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
                
                image.Mutate(x => x.Resize(targetWidth, targetHeight, KnownResamplers.Lanczos3));
            }

            // Bỏ đi meta data EXIF cho bảo mật
            image.Metadata.ExifProfile = null;
            image.Metadata.XmpProfile = null;
            image.Metadata.IptcProfile = null;

            var resultStream = new MemoryStream();
            await image.SaveAsWebpAsync(resultStream, new WebpEncoder { Quality = quality }, ct);
            
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
