using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

// Validator đảm bảo dữ liệu đầu vào tối thiểu trước khi vào handler.
public class RequestConversationCompleteCommandValidator : AbstractValidator<RequestConversationCompleteCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho yêu cầu hoàn thành conversation.
    /// Luồng xử lý: bắt buộc ConversationId và RequesterId có giá trị để tránh xử lý request rỗng.
    /// </summary>
    public RequestConversationCompleteCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
