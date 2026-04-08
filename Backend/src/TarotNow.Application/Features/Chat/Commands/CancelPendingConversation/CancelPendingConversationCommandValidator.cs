using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;

public class CancelPendingConversationCommandValidator : AbstractValidator<CancelPendingConversationCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho CancelPendingConversationCommand.
    /// Luồng xử lý: kiểm tra conversation id và requester id bắt buộc.
    /// </summary>
    public CancelPendingConversationCommandValidator()
    {
        // ConversationId bắt buộc để xác định conversation cần hủy.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // RequesterId bắt buộc để kiểm tra quyền thao tác.
        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
