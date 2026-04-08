using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

// Validator đầu vào cho command queue chat moderation.
public class QueueChatModerationCommandValidator : AbstractValidator<QueueChatModerationCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho QueueChatModerationCommand.
    /// Luồng xử lý: đảm bảo payload tồn tại và các trường cốt lõi không rỗng.
    /// </summary>
    public QueueChatModerationCommandValidator()
    {
        // Payload moderation bắt buộc.
        RuleFor(x => x.Payload)
            .NotNull();

        // MessageId bắt buộc để truy vết item moderation.
        RuleFor(x => x.Payload.MessageId)
            .NotEmpty();

        // ConversationId bắt buộc để định vị ngữ cảnh cuộc trò chuyện.
        RuleFor(x => x.Payload.ConversationId)
            .NotEmpty();

        // SenderId bắt buộc để phục vụ enforcement theo user.
        RuleFor(x => x.Payload.SenderId)
            .NotEmpty();

        // Type bắt buộc để áp dụng đúng policy moderation.
        RuleFor(x => x.Payload.Type)
            .NotEmpty();

        // Content bắt buộc vì đây là dữ liệu chính cần kiểm duyệt.
        RuleFor(x => x.Payload.Content)
            .NotEmpty();
    }
}
