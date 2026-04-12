using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Lưu/xóa object trên R2 hoặc storage tương đương.
/// </summary>
public interface IObjectStorageService
{
    /// <summary>
    /// Upload bytes với key prefix (vd: avatars, community); trả key đầy đủ và URL public.
    /// </summary>
    Task<(string ObjectKey, string PublicUrl)> PutBytesAsync(
        byte[] data,
        string contentType,
        string keyPrefix,
        string fileExtension,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default);
}
