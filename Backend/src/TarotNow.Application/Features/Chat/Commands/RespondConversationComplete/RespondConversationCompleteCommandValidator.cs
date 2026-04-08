using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

// Validator đầu vào cho command phản hồi hoàn thành conversation.
public class RespondConversationCompleteCommandValidator : AbstractValidator<RespondConversationCompleteCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RespondConversationCompleteCommand.
    /// Luồng xử lý: bắt buộc ConversationId và RequesterId để xác định đúng conversation và participant phản hồi.
    /// </summary>
    public RespondConversationCompleteCommandValidator()
    {
        // ConversationId bắt buộc để định vị cuộc trò chuyện.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // RequesterId bắt buộc để kiểm tra quyền phản hồi.
        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
