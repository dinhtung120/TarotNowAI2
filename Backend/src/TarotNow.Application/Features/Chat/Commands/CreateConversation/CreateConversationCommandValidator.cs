using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ReaderId)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x => x.UserId != x.ReaderId)
            .WithMessage("UserId và ReaderId phải khác nhau.");

        RuleFor(x => x.SlaHours)
            .InclusiveBetween(1, 168);
    }
}
