/*
 * ===================================================================
 * FILE: IMediaProcessorService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý nén và tối ưu hóa đa phương tiện (Image/Voice) trước khi lưu trữ
 *   theo đúng thiết kế kỹ thuật (AVIF cho ảnh, Opus cho âm thanh).
 * ===================================================================
 */

using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IMediaProcessorService
{
    /// <summary>
    /// Chuyển đổi và nén ảnh sang định dạng AVIF (giảm 70% quality, gỡ bỏ metadata EXIF).
    /// </summary>
    /// <param name="imageBytes">Dữ liệu ảnh gốc (WebP, JPEG, PNG...)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dữ liệu đã nén dưới dạng bytes và MimeType mới.</returns>
    Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(byte[] imageBytes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Chuyển đổi và nén tệp âm thanh sang Audio Opus (16kbps).
    /// </summary>
    /// <param name="voiceBytes">Dữ liệu âm thanh gốc</param>
    /// <param name="extension">Phần mở rộng file gốc phục vụ FFmpeg</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dữ liệu đã nén dưới dạng bytes và MimeType mới.</returns>
    Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(byte[] voiceBytes, string extension, CancellationToken cancellationToken = default);
}
