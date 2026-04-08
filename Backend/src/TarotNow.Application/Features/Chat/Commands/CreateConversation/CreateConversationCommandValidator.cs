using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

// Validator đầu vào cho command tạo conversation.
public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho CreateConversationCommand.
    /// Luồng xử lý: kiểm tra user/reader id bắt buộc, hai id khác nhau và sla trong ngưỡng hợp lệ.
    /// </summary>
    public CreateConversationCommandValidator()
    {
        // UserId bắt buộc.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // ReaderId bắt buộc.
        RuleFor(x => x.ReaderId)
            .NotEmpty();

        // UserId và ReaderId phải khác nhau.
        RuleFor(x => x)
            .Must(x => x.UserId != x.ReaderId)
            .WithMessage("UserId và ReaderId phải khác nhau.");

        // SLA cho phép trong [1,168] ở tầng validation biên.
        RuleFor(x => x.SlaHours)
            .InclusiveBetween(1, 168);
    }
}
