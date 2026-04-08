
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract xử lý media trước khi lưu để đảm bảo tối ưu dung lượng và định dạng đầu ra.
public interface IMediaProcessorService
{
    /// <summary>
    /// Xử lý và nén ảnh để giảm băng thông lưu trữ nhưng vẫn giữ chất lượng hiển thị.
    /// Luồng xử lý: nhận mảng byte ảnh đầu vào, áp pipeline tối ưu và trả dữ liệu ảnh đã nén cùng mime type.
    /// </summary>
    Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(byte[] imageBytes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xử lý và nén voice để chuẩn hóa định dạng phát lại và giảm kích thước tệp.
    /// Luồng xử lý: nhận byte voice + extension, thực thi chuyển đổi cần thiết và trả dữ liệu/mime type đầu ra.
    /// </summary>
    Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(byte[] voiceBytes, string extension, CancellationToken cancellationToken = default);
}
