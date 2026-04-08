using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;

// Validator đầu vào cho command mở tranh chấp conversation.
public class OpenConversationDisputeCommandValidator : AbstractValidator<OpenConversationDisputeCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho OpenConversationDisputeCommand.
    /// Luồng xử lý: kiểm tra conversation/user id, item id tùy chọn và reason đủ độ dài.
    /// </summary>
    public OpenConversationDisputeCommandValidator()
    {
        // ConversationId bắt buộc.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // UserId bắt buộc để kiểm tra quyền thao tác.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // Nếu có ItemId thì không được là Guid.Empty.
        RuleFor(x => x.ItemId)
            .NotEqual(Guid.Empty)
            .When(x => x.ItemId.HasValue);

        // Reason bắt buộc và đủ dài cho nghiệp vụ dispute.
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}
