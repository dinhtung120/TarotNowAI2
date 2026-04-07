

using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

public class SubmitReaderRequestValidator : AbstractValidator<SubmitReaderRequestCommand>
{
    public SubmitReaderRequestValidator()
    {
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");

        
        RuleFor(x => x.IntroText)
            .NotEmpty()
            .WithMessage("Lời giới thiệu không được để trống.")
            
            .MinimumLength(20)
            .WithMessage("Lời giới thiệu phải có ít nhất 20 ký tự.")
            
            .MaximumLength(2000)
            .WithMessage("Lời giới thiệu không được vượt quá 2000 ký tự.");
    }
}
