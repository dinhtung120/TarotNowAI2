using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public class AcceptConversationCommandValidator : AbstractValidator<AcceptConversationCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho AcceptConversationCommand.
    /// Luồng xử lý: kiểm tra conversation id và reader id bắt buộc.
    /// </summary>
    public AcceptConversationCommandValidator()
    {
        // ConversationId bắt buộc để xác định đúng phiên cần accept.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // ReaderId bắt buộc để kiểm tra quyền reader.
        RuleFor(x => x.ReaderId)
            .NotEmpty();
    }
}
