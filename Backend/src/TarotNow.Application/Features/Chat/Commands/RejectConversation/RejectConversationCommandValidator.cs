using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

// Validator đầu vào cho command reject conversation.
public class RejectConversationCommandValidator : AbstractValidator<RejectConversationCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RejectConversationCommand.
    /// Luồng xử lý: kiểm tra conversation id, reader id bắt buộc và reason tối đa 1000 ký tự nếu có.
    /// </summary>
    public RejectConversationCommandValidator()
    {
        // ConversationId bắt buộc.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // ReaderId bắt buộc để kiểm tra quyền.
        RuleFor(x => x.ReaderId)
            .NotEmpty();

        // Reason tùy chọn nhưng cần giới hạn độ dài.
        RuleFor(x => x.Reason)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.Reason) == false);
    }
}
