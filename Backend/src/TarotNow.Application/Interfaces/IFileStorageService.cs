namespace TarotNow.Application.Interfaces;

public interface IFileStorageService
{
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder, CancellationToken ct = default);
    
        Task DeleteFileAsync(string relativePath, CancellationToken ct = default);
}
