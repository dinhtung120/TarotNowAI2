using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

// Validator biên cho command gửi message.
public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho SendMessageCommand.
    /// Luồng xử lý: bắt buộc conversation/sender/type và bắt buộc content khi type là text.
    /// </summary>
    public SendMessageCommandValidator()
    {
        // ConversationId bắt buộc để xác định phòng chat.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // SenderId bắt buộc để kiểm tra quyền participant.
        RuleFor(x => x.SenderId)
            .NotEmpty();

        // Type phải nằm trong tập giá trị đã hỗ trợ.
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(ChatMessageType.IsValid)
            .WithMessage("Loại tin nhắn không hợp lệ.");

        // Text message bắt buộc có content.
        RuleFor(x => x.Content)
            .NotEmpty()
            .When(x => x.Type == ChatMessageType.Text);

        RuleFor(x => x.MediaPayload)
            .NotNull()
            .When(x => x.Type is ChatMessageType.Image or ChatMessageType.Voice)
            .WithMessage("MediaPayload là bắt buộc với tin nhắn image/voice.");

        RuleFor(x => x.MediaPayload!.UploadToken)
            .NotEmpty()
            .When(x => x.Type is ChatMessageType.Image or ChatMessageType.Voice);

        RuleFor(x => x.MediaPayload!.ObjectKey)
            .NotEmpty()
            .When(x => x.Type is ChatMessageType.Image or ChatMessageType.Voice);
    }
}
