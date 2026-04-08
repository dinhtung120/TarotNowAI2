using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

// Validator đầu vào cho command đánh dấu message đã đọc.
public class MarkMessagesReadCommandValidator : AbstractValidator<MarkMessagesReadCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho MarkMessagesReadCommand.
    /// Luồng xử lý: kiểm tra conversation id và participant id bắt buộc.
    /// </summary>
    public MarkMessagesReadCommandValidator()
    {
        // ConversationId bắt buộc.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // ReaderId/participant id bắt buộc.
        RuleFor(x => x.ReaderId)
            .NotEmpty();
    }
}
