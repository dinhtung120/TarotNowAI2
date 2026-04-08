namespace TarotNow.Application.Interfaces;

// Contract xử lý ảnh đầu vào để chuẩn hóa kích thước và chất lượng trước khi lưu trữ.
public interface IImageProcessingService
{
    /// <summary>
    /// Nén ảnh để giảm dung lượng nhưng vẫn giữ chất lượng hiển thị phù hợp.
    /// Luồng xử lý: đọc stream gốc, resize theo maxDimension, nén theo quality và trả stream đã tối ưu.
    /// </summary>
    Task<Stream> CompressAsync(Stream input, int maxDimension = 512, int quality = 80, CancellationToken ct = default);
}
