namespace TarotNow.Application.Interfaces;

public interface IImageProcessingService
{
    /// <summary>Nén + resize ảnh, trả về stream ảnh đã xử lý</summary>
    Task<Stream> CompressAsync(Stream input, int maxDimension = 512, int quality = 80, CancellationToken ct = default);
}
