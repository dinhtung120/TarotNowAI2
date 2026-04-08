using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

// Validator đầu vào cho command phản hồi đề nghị cộng tiền.
public class RespondConversationAddMoneyCommandValidator : AbstractValidator<RespondConversationAddMoneyCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho RespondConversationAddMoneyCommand.
    /// Luồng xử lý: bắt buộc conversation/user/offer id; riêng rejectReason chỉ bắt buộc khi nhánh từ chối.
    /// </summary>
    public RespondConversationAddMoneyCommandValidator()
    {
        // ConversationId bắt buộc để xác định luồng xử lý.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // UserId bắt buộc để kiểm tra quyền phản hồi.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // OfferMessageId bắt buộc để khóa phản hồi vào đúng đề nghị.
        RuleFor(x => x.OfferMessageId)
            .NotEmpty();

        // Khi từ chối thì cần lý do có độ dài hợp lệ để hiển thị rõ trong message.
        RuleFor(x => x.RejectReason)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(1000)
            .When(x => x.Accept == false);
    }
}
