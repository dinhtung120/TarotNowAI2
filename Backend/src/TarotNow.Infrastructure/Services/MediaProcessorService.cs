using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class MediaProcessorService : IMediaProcessorService
{
    private const int MaxImageWidth = 2048;
    private const int MaxImageHeight = 2048;
    private const int AvifQuality = 70;
    private const string AvifEncoderTypeName = "SixLabors.ImageSharp.Formats.Avif.AvifEncoder, SixLabors.ImageSharp";
    private const string AvifMimeType = "image/avif";
    private const string WebpMimeType = "image/webp";
    private const string OpusMimeType = "audio/opus";
    private const string WebmMimeType = "audio/webm";
    private const string OpusExtension = ".opus";

    /// <inheritdoc />
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
            return await SaveAsAvifOrWebpAsync(image, outStream, cancellationToken);
        }
        catch
        {
            return await SaveAsWebpAsync(image, outStream, cancellationToken);
        }
    }

    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(
        byte[] voiceBytes,
        string extension,
        CancellationToken cancellationToken = default)
    {
        var tempIn = CreateTempFilePath(extension);
        var tempOut = CreateTempFilePath(OpusExtension);

        try
        {
            await File.WriteAllBytesAsync(tempIn, voiceBytes, cancellationToken);
            await ConvertToOpusAsync(tempIn, tempOut);

            var compressedBytes = await File.ReadAllBytesAsync(tempOut, cancellationToken);
            return (compressedBytes, OpusMimeType);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MediaProcessor] FFmpeg lỗi: {ex.Message}");
            return (voiceBytes, WebmMimeType);
        }
        finally
        {
            DeleteTempFileIfExists(tempIn);
            DeleteTempFileIfExists(tempOut);
        }
    }

    private static void StripImageExif(Image image)
    {
        if (image.Metadata.ExifProfile == null) return;
        image.Metadata.ExifProfile = null;
    }

    private static void ResizeImageIfNeeded(Image image)
    {
        if (image.Width <= MaxImageWidth && image.Height <= MaxImageHeight) return;

        image.Mutate(operation => operation.Resize(new ResizeOptions
        {
            Size = new Size(MaxImageWidth, MaxImageHeight),
            Mode = ResizeMode.Max
        }));
    }

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

    private static async Task<(byte[] Data, string MimeType)> SaveAsWebpAsync(
        Image image,
        MemoryStream outStream,
        CancellationToken cancellationToken)
    {
        await image.SaveAsWebpAsync(outStream, cancellationToken);
        return (outStream.ToArray(), WebpMimeType);
    }

    private static string CreateTempFilePath(string extension)
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);
    }

    private static async Task ConvertToOpusAsync(string inputPath, string outputPath)
    {
        await FFMpegArguments
            .FromFileInput(inputPath)
            .OutputToFile(outputPath, true, options => options
                .WithAudioCodec("libopus")
                .WithCustomArgument("-b:a 16k -ac 1"))
            .ProcessAsynchronously();
    }

    private static void DeleteTempFileIfExists(string filePath)
    {
        if (!File.Exists(filePath)) return;
        File.Delete(filePath);
    }
}
