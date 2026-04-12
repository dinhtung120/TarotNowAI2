using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.UserImageUpload;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Pipeline chung: validate → quét virus (tuỳ cấu hình) → nén AVIF/WebP → upload object storage.
/// </summary>
public interface IUserImagePipeline
{
    /// <summary>
    /// Xử lý ảnh đầu vào và upload; không ghi DB.
    /// </summary>
    Task<UserImagePipelineResult> ProcessUploadAsync(
        Stream imageStream,
        string fileName,
        string contentType,
        UserImageUploadKind kind,
        CancellationToken cancellationToken = default);
}
