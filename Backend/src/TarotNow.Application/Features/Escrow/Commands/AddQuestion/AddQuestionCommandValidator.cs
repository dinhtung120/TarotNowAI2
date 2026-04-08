using FluentValidation;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

// Validator đầu vào cho command add question.
public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho AddQuestionCommand.
    /// Luồng xử lý: bắt buộc UserId/ConversationRef/AmountDiamond/IdempotencyKey và giới hạn ProposalMessageRef.
    /// </summary>
    public AddQuestionCommandValidator()
    {
        // UserId bắt buộc để xác định payer.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // ConversationRef bắt buộc để định vị session tài chính.
        RuleFor(x => x.ConversationRef)
            .NotEmpty();

        // AmountDiamond phải dương.
        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);

        // ProposalMessageRef là tùy chọn nhưng giới hạn độ dài.
        RuleFor(x => x.ProposalMessageRef)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.ProposalMessageRef) == false);

        // IdempotencyKey bắt buộc và giới hạn độ dài.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
