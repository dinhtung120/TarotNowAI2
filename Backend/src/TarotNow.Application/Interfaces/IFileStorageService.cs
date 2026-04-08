namespace TarotNow.Application.Interfaces;

// Contract lưu trữ tệp để tách business logic khỏi nhà cung cấp storage cụ thể.
public interface IFileStorageService
{
    /// <summary>
    /// Lưu tệp lên storage và trả về đường dẫn tương đối để lưu metadata.
    /// Luồng xử lý: nhận stream đầu vào, ghi vào subfolder đích và trả key truy cập nội bộ.
    /// </summary>
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder, CancellationToken ct = default);

    /// <summary>
    /// Xóa tệp khỏi storage khi nội dung không còn hợp lệ hoặc bị thay thế.
    /// Luồng xử lý: định vị tệp theo relativePath và yêu cầu provider xóa vật lý.
    /// </summary>
    Task DeleteFileAsync(string relativePath, CancellationToken ct = default);
}
