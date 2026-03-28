namespace TarotNow.Application.Interfaces;

public interface IFileStorageService
{
    /// <summary>Lưu file và trả về đường dẫn tương đối (relative URL)</summary>
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder, CancellationToken ct = default);
    
    /// <summary>Xóa file cũ (nếu có)</summary>
    Task DeleteFileAsync(string relativePath, CancellationToken ct = default);
}
