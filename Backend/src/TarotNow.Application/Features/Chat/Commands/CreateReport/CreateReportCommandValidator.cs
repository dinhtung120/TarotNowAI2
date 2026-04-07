using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(x => x.ReporterId)
            .NotEmpty();

        RuleFor(x => x.TargetType)
            .NotEmpty()
            .Must(type =>
            {
                var normalized = type?.Trim().ToLowerInvariant();
                return normalized is "message" or "conversation" or "user";
            })
            .WithMessage("TargetType phải là message, conversation hoặc user.");

        RuleFor(x => x.TargetId)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.ConversationRef)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.ConversationRef) == false);

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}
