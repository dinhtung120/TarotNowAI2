using FluentValidation;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

// Validator đầu vào cho command request add money.
public class RequestConversationAddMoneyCommandValidator : AbstractValidator<RequestConversationAddMoneyCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RequestConversationAddMoneyCommand.
    /// Luồng xử lý: kiểm tra conversation id, reader id, amount dương, description và idempotency key.
    /// </summary>
    public RequestConversationAddMoneyCommandValidator(ISystemConfigSettings systemConfigSettings)
    {
        var maxNoteLength = systemConfigSettings.ChatPaymentOfferMaxNoteLength;

        // ConversationId bắt buộc.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // ReaderId bắt buộc.
        RuleFor(x => x.ReaderId)
            .NotEmpty();

        // Amount phải dương.
        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);

        // Description tùy chọn ở validator biên, giới hạn độ dài khi có giá trị.
        RuleFor(x => x.Description)
            .MaximumLength(maxNoteLength)
            .When(x => string.IsNullOrWhiteSpace(x.Description) == false);

        // Idempotency key bắt buộc để chống tạo proposal trùng.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
