using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

// Validator cho command gửi đơn reader.
public class SubmitReaderRequestValidator : AbstractValidator<SubmitReaderRequestCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu gửi đơn reader.
    /// Luồng xử lý: kiểm tra UserId và ràng buộc độ dài IntroText để bảo đảm đơn có nội dung đủ đánh giá.
    /// </summary>
    public SubmitReaderRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");
        // UserId bắt buộc để liên kết đơn với đúng tài khoản.

        RuleFor(x => x.IntroText)
            .NotEmpty()
            .WithMessage("Lời giới thiệu không được để trống.")
            .MinimumLength(20)
            .WithMessage("Lời giới thiệu phải có ít nhất 20 ký tự.")
            .MaximumLength(2000)
            .WithMessage("Lời giới thiệu không được vượt quá 2000 ký tự.");
        // Business rule: mô tả quá ngắn sẽ không đủ dữ liệu cho admin đánh giá năng lực reader.
    }
}
