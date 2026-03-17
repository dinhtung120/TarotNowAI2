using MediatR;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Command để user gửi đơn xin trở thành Reader.
///
/// Thuộc CQRS "Command" side — thay đổi trạng thái hệ thống:
/// → Tạo document mới trong reader_requests collection (MongoDB).
///
/// Caller: ReaderController.Apply() — yêu cầu [Authorize].
/// UserId lấy từ JWT claims, không từ request body (bảo mật).
/// </summary>
public class SubmitReaderRequestCommand : IRequest<bool>
{
    /// <summary>UUID của user — lấy từ JWT ClaimTypes.NameIdentifier.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Lời giới thiệu — tại sao user muốn trở thành Reader.
    /// Bắt buộc, validate không trống qua FluentValidation.
    /// </summary>
    public string IntroText { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách URL tài liệu chứng minh (ảnh bằng cấp, chứng chỉ...).
    /// Tùy chọn, có thể trống.
    /// </summary>
    public List<string> ProofDocuments { get; set; } = new();
}
