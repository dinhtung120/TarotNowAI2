namespace TarotNow.Application.Interfaces;

public interface IImageProcessingService
{
        Task<Stream> CompressAsync(Stream input, int maxDimension = 512, int quality = 80, CancellationToken ct = default);
}
