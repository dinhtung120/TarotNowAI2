using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// FluentValidation validator cho SubmitReaderRequestCommand.
///
/// Tại sao validate ở đây thay vì trong Handler?
/// → Separation of Concerns: Handler xử lý business logic, Validator xử lý input.
/// → ValidationBehavior pipeline tự động gọi trước Handler (đã đăng ký trong DI).
/// → Trả về validation errors nhất quán (ProblemDetails format).
/// </summary>
public class SubmitReaderRequestValidator : AbstractValidator<SubmitReaderRequestCommand>
{
    public SubmitReaderRequestValidator()
    {
        // UserId bắt buộc — không cho phép GUID rỗng (dấu hiệu của lỗi mapping)
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");

        // IntroText bắt buộc, tối thiểu 20 ký tự, tối đa 2000
        // Tại sao 20 ký tự tối thiểu? → Đủ để admin đánh giá nghiêm túc
        RuleFor(x => x.IntroText)
            .NotEmpty()
            .WithMessage("Lời giới thiệu không được để trống.")
            .MinimumLength(20)
            .WithMessage("Lời giới thiệu phải có ít nhất 20 ký tự.")
            .MaximumLength(2000)
            .WithMessage("Lời giới thiệu không được vượt quá 2000 ký tự.");
    }
}
