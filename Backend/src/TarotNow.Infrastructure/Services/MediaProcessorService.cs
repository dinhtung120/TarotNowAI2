

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
    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(byte[] imageBytes, CancellationToken cancellationToken = default)
    {
        using var inStream = new MemoryStream(imageBytes);
        using var image = await Image.LoadAsync(inStream, cancellationToken);

        
        if (image.Metadata.ExifProfile != null)
        {
            image.Metadata.ExifProfile = null;
        }

        
        if (image.Width > 2048 || image.Height > 2048)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(2048, 2048),
                Mode = ResizeMode.Max
            }));
        }

        using var outStream = new MemoryStream();

        
        
        try
        {
            
            
            
            
            
            
            
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
                
                await image.SaveAsWebpAsync(outStream, cancellationToken);
                return (outStream.ToArray(), "image/webp");
            }
        }
        catch
        {
            
            await image.SaveAsWebpAsync(outStream, cancellationToken);
            return (outStream.ToArray(), "image/webp");
        }
    }

    public async Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(byte[] voiceBytes, string extension, CancellationToken cancellationToken = default)
    {
        
        var tempIn = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
        var tempOut = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".opus");

        try
        {
            await File.WriteAllBytesAsync(tempIn, voiceBytes, cancellationToken);

            
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
            
            Console.WriteLine($"[MediaProcessor] FFmpeg lỗi: {ex.Message}");
            return (voiceBytes, "audio/webm"); 
        }
        finally
        {
            if (File.Exists(tempIn)) File.Delete(tempIn);
            if (File.Exists(tempOut)) File.Delete(tempOut);
        }
    }
}
