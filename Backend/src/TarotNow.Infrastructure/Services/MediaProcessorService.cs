using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service xử lý nén ảnh và chuyển đổi voice media cho chat/reading.
public class MediaProcessorService : IMediaProcessorService
{
    // Giới hạn kích thước ảnh mặc định (chat/media) để giảm băng thông.
    private const int MaxImageWidth = 2048;
    private const string WebpMimeType = "image/webp";
    private const string OpusMimeType = "audio/opus";
    private const string WebmMimeType = "audio/webm";
    private const string OpusExtension = ".opus";

    /// <summary>
    /// Xử lý và nén ảnh đầu vào sang AVIF hoặc WebP.
    /// Luồng xóa metadata, resize khi vượt ngưỡng rồi ưu tiên encode AVIF để tối ưu kích thước.
    /// </summary>
    public Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(
        byte[] imageBytes,
        CancellationToken cancellationToken = default)
        => ProcessAndCompressImageAsync(imageBytes, MaxImageWidth, cancellationToken);

    /// <inheritdoc />
    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(
        byte[] imageBytes,
        int maxEdgePixels,
        CancellationToken cancellationToken = default)
    {
        using var inStream = new MemoryStream(imageBytes);
        using var image = await Image.LoadAsync(inStream, cancellationToken);

        StripImageMetadata(image);
        ResizeImageIfNeeded(image, maxEdgePixels);

        using var outStream = new MemoryStream();
        /**
         * CHỈ SỬ DỤNG WEBP:
         * Đã loại bỏ hoàn toàn AVIF do thiếu thư viện native hệ thống trong Docker.
         * WebP của ImageSharp là giải pháp tối ưu nhất: chạy thuần C#, không gây lỗi logic,
         * chất lượng nén rất tốt và thời gian xử lý cực nhanh (vài trăm ms).
         */
        return await SaveAsWebpAsync(image, outStream, cancellationToken);
    }

    /// <summary>
    /// Chuyển đổi voice input về định dạng Opus bitrate thấp.
    /// Luồng dùng file tạm để gọi FFmpeg; nếu lỗi sẽ trả dữ liệu gốc nhằm không chặn luồng gửi tin nhắn.
    /// </summary>
    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(
        byte[] voiceBytes,
        string extension,
        CancellationToken cancellationToken = default)
    {
        var tempIn = CreateTempFilePath(extension);
        var tempOut = CreateTempFilePath(OpusExtension);

        try
        {
            // Ghi file tạm để FFmpeg đọc đầu vào ổn định.
            await File.WriteAllBytesAsync(tempIn, voiceBytes, cancellationToken);
            await ConvertToOpusAsync(tempIn, tempOut);

            var compressedBytes = await File.ReadAllBytesAsync(tempOut, cancellationToken);
            return (compressedBytes, OpusMimeType);
        }
        catch (Exception ex)
        {
            // Fallback giữ nguyên dữ liệu cũ để tránh làm hỏng trải nghiệm gửi voice khi FFmpeg lỗi.
            Console.WriteLine($"[MediaProcessor] FFmpeg lỗi: {ex.Message}");
            return (voiceBytes, WebmMimeType);
        }
        finally
        {
            // Luôn dọn file tạm để tránh rò rỉ disk theo thời gian.
            DeleteTempFileIfExists(tempIn);
            DeleteTempFileIfExists(tempOut);
        }
    }

    /// <summary>
    /// Xóa EXIF khỏi ảnh để giảm kích thước và loại bỏ metadata nhạy cảm.
    /// Luồng chỉ thao tác khi profile tồn tại để tránh xử lý thừa.
    /// </summary>
    private static void StripImageMetadata(Image image)
    {
        image.Metadata.ExifProfile = null;
        image.Metadata.XmpProfile = null;
        image.Metadata.IptcProfile = null;
    }

    /// <summary>
    /// Resize ảnh khi vượt kích thước tối đa cho phép.
    /// Luồng dùng ResizeMode.Max để giữ tỉ lệ và tránh biến dạng ảnh.
    /// </summary>
    private static void ResizeImageIfNeeded(Image image, int maxEdgePixels)
    {
        if (image.Width <= maxEdgePixels && image.Height <= maxEdgePixels) return;

        image.Mutate(operation => operation.Resize(new ResizeOptions
        {
            Size = new Size(maxEdgePixels, maxEdgePixels),
            Mode = ResizeMode.Max
        }));
    }


    /// <summary>
    /// Lưu ảnh sang WebP như định dạng dự phòng phổ biến.
    /// Luồng này được dùng khi AVIF không khả dụng hoặc encode thất bại.
    /// </summary>
    private static async Task<(byte[] Data, string MimeType)> SaveAsWebpAsync(
        Image image,
        MemoryStream outStream,
        CancellationToken cancellationToken)
    {
        outStream.SetLength(0);
        await image.SaveAsWebpAsync(outStream, new WebpEncoder 
        { 
            Quality = 82,
            Method = WebpEncodingMethod.Fastest 
        }, cancellationToken);
        return (outStream.ToArray(), WebpMimeType);
    }

    /// <summary>
    /// Tạo đường dẫn file tạm duy nhất theo phần mở rộng chỉ định.
    /// Luồng này tránh xung đột tên file khi xử lý song song nhiều request.
    /// </summary>
    private static string CreateTempFilePath(string extension)
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);
    }

    /// <summary>
    /// Dùng FFmpeg chuyển đổi audio sang Opus mono bitrate thấp.
    /// Luồng chuẩn hóa output để giảm dung lượng truyền tải voice message.
    /// </summary>
    private static async Task ConvertToOpusAsync(string inputPath, string outputPath)
    {
        await FFMpegArguments
            .FromFileInput(inputPath)
            .OutputToFile(outputPath, true, options => options
                .WithAudioCodec("libopus")
                .WithCustomArgument("-b:a 16k -ac 1"))
            .ProcessAsynchronously();
    }

    /// <summary>
    /// Xóa file tạm nếu còn tồn tại.
    /// Luồng tách helper để đảm bảo cleanup đồng nhất tại mọi điểm finally.
    /// </summary>
    private static void DeleteTempFileIfExists(string filePath)
    {
        if (!File.Exists(filePath)) return;
        File.Delete(filePath);
    }
}
