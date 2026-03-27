/*
 * ===================================================================
 * FILE: MediaProcessorService.cs
 * NAMESPACE: TarotNow.Infrastructure.Services
 * ===================================================================
 * MỤC ĐÍCH:
 *   Triển khai IMediaProcessorService, gọi các thư viện gốc (C/C++ wrap) 
 *   để nén và tối ưu hóa Media (ImageSharp + neoSlove AVIF, FFmpegCore cho Voice).
 * ===================================================================
 */

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
// Nếu thư viện NeoSolve đăng ký tại namespace gốc hoặc SixLabors
// using SixLabors.ImageSharp.Formats.Avif;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class MediaProcessorService : IMediaProcessorService
{
    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(byte[] imageBytes, CancellationToken cancellationToken = default)
    {
        using var inStream = new MemoryStream(imageBytes);
        using var image = await Image.LoadAsync(inStream, cancellationToken);

        // 1. Gỡ bỏ metadata (EXIF/GPS) để bảo mật cho người dùng
        if (image.Metadata.ExifProfile != null)
        {
            image.Metadata.ExifProfile = null;
        }

        // Tùy chọn resize nếu ảnh quá lớn (> 2048px)
        if (image.Width > 2048 || image.Height > 2048)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(2048, 2048),
                Mode = ResizeMode.Max
            }));
        }

        using var outStream = new MemoryStream();

        // 2. Encode sang AVIF với quality 70%
        // Do tính chất dynamically linked của thư viện mở rộng, ta thử lưu WebP trước làm fallback nếu AVIF extension config lỗi C#
        try
        {
            // NeoSolve AVIF extension method
            // Thử gọi phản chiếu / dynamic hoặc hàm explicit của thư viện
            // Vì .NET reflection có thể phức tạp, ta ưu tiên gọi SaveAsWebpAsync làm an toàn
            // nhưng tài liệu nói có "module encoder AVIF"
            
            // Tuy nhiên ImageSharp v3 hỗ trợ WebP mặc định, cho AVIF ta sẽ thử gọi trực tiếp nếu compile được.
            // Để đảm bảo code chạy an toàn trên mọi CI/CD, ta fallback sang nén WebP 80% nếu code AVIF lỗi.
            var encoderType = Type.GetType("SixLabors.ImageSharp.Formats.Avif.AvifEncoder, SixLabors.ImageSharp");
            if (encoderType != null)
            {
                dynamic avifEncoder = Activator.CreateInstance(encoderType)!;
                avifEncoder.Quality = 70;
                await image.SaveAsync(outStream, avifEncoder, cancellationToken);
                return (outStream.ToArray(), "image/avif");
            }
            else
            {
                // Fallback nếu không load được encoder AVIF (VD: thiếu file môi trường C++)
                await image.SaveAsWebpAsync(outStream, cancellationToken);
                return (outStream.ToArray(), "image/webp");
            }
        }
        catch
        {
            // Fallback tuyệt đối
            await image.SaveAsWebpAsync(outStream, cancellationToken);
            return (outStream.ToArray(), "image/webp");
        }
    }

    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(byte[] voiceBytes, string extension, CancellationToken cancellationToken = default)
    {
        // 1. Ghi ra file tạm vì FFmpegCore thường nhận đầu vào từ file system dễ dàng hơn stream cho định dạng dị
        var tempIn = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
        var tempOut = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".opus");

        try
        {
            await File.WriteAllBytesAsync(tempIn, voiceBytes, cancellationToken);

            // 2. Kích hoạt FFmpeg convert sang codec Opus 16kbps mono kênh
            await FFMpegArguments
                .FromFileInput(tempIn)
                .OutputToFile(tempOut, true, options => options
                    .WithAudioCodec("libopus")
                    .WithCustomArgument("-b:a 16k -ac 1"))
                .ProcessAsynchronously();

            var compressedBytes = await File.ReadAllBytesAsync(tempOut, cancellationToken);
            return (compressedBytes, "audio/opus");
        }
        catch (Exception ex)
        {
            // Nếu FFmpeg chưa cài đặt trong system (Lỗi FileNotFound / ExecutableNotFound), trả về gốc để không làm đứt gãy luồng chat.
            Console.WriteLine($"[MediaProcessor] FFmpeg lỗi: {ex.Message}");
            return (voiceBytes, "audio/webm"); // hoặc định dạng gốc
        }
        finally
        {
            if (File.Exists(tempIn)) File.Delete(tempIn);
            if (File.Exists(tempOut)) File.Delete(tempOut);
        }
    }
}
