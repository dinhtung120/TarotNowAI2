using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service xử lý nén ảnh và chuyển đổi voice media cho chat/reading.
public class MediaProcessorService : IMediaProcessorService
{
    // Giới hạn kích thước ảnh để giảm băng thông và chi phí lưu trữ.
    private const int MaxImageWidth = 2048;
    private const int MaxImageHeight = 2048;
    // Mức chất lượng AVIF cân bằng giữa dung lượng và độ rõ.
    private const int AvifQuality = 70;
    // Type name động để tránh hard dependency khi encoder AVIF không có trong runtime.
    private const string AvifEncoderTypeName = "SixLabors.ImageSharp.Formats.Avif.AvifEncoder, SixLabors.ImageSharp";
    // MIME type chuẩn cho ảnh AVIF/WebP và voice Opus/WebM fallback.
    private const string AvifMimeType = "image/avif";
    private const string WebpMimeType = "image/webp";
    private const string OpusMimeType = "audio/opus";
    private const string WebmMimeType = "audio/webm";
    private const string OpusExtension = ".opus";

    /// <summary>
    /// Xử lý và nén ảnh đầu vào sang AVIF hoặc WebP.
    /// Luồng xóa metadata, resize khi vượt ngưỡng rồi ưu tiên encode AVIF để tối ưu kích thước.
    /// </summary>
    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(
        byte[] imageBytes,
        CancellationToken cancellationToken = default)
    {
        using var inStream = new MemoryStream(imageBytes);
        using var image = await Image.LoadAsync(inStream, cancellationToken);

        StripImageExif(image);
        ResizeImageIfNeeded(image);

        using var outStream = new MemoryStream();
        try
        {
            // Ưu tiên AVIF để đạt tỷ lệ nén tốt hơn khi encoder khả dụng.
            return await SaveAsAvifOrWebpAsync(image, outStream, cancellationToken);
        }
        catch
        {
            // Edge case: AVIF lỗi hoặc không hỗ trợ thì fallback WebP để luôn có kết quả hợp lệ.
            return await SaveAsWebpAsync(image, outStream, cancellationToken);
        }
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
    private static void StripImageExif(Image image)
    {
        if (image.Metadata.ExifProfile == null) return;
        image.Metadata.ExifProfile = null;
    }

    /// <summary>
    /// Resize ảnh khi vượt kích thước tối đa cho phép.
    /// Luồng dùng ResizeMode.Max để giữ tỉ lệ và tránh biến dạng ảnh.
    /// </summary>
    private static void ResizeImageIfNeeded(Image image)
    {
        if (image.Width <= MaxImageWidth && image.Height <= MaxImageHeight) return;

        image.Mutate(operation => operation.Resize(new ResizeOptions
        {
            Size = new Size(MaxImageWidth, MaxImageHeight),
            Mode = ResizeMode.Max
        }));
    }

    /// <summary>
    /// Lưu ảnh sang AVIF nếu encoder khả dụng, ngược lại fallback WebP.
    /// Luồng kiểm tra type động để tương thích môi trường thiếu package AVIF.
    /// </summary>
    private static async Task<(byte[] Data, string MimeType)> SaveAsAvifOrWebpAsync(
        Image image,
        MemoryStream outStream,
        CancellationToken cancellationToken)
    {
        var encoderType = Type.GetType(AvifEncoderTypeName);
        if (encoderType == null)
        {
            return await SaveAsWebpAsync(image, outStream, cancellationToken);
        }

        dynamic avifEncoder = Activator.CreateInstance(encoderType)!;
        avifEncoder.Quality = AvifQuality;
        await image.SaveAsync(outStream, avifEncoder, cancellationToken);
        return (outStream.ToArray(), AvifMimeType);
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
        await image.SaveAsWebpAsync(outStream, cancellationToken);
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
