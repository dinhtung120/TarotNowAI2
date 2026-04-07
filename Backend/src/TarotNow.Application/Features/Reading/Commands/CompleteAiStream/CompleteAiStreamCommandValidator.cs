using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public class CompleteAiStreamCommandValidator : AbstractValidator<CompleteAiStreamCommand>
{
    public CompleteAiStreamCommandValidator()
    {
        RuleFor(x => x.AiRequestId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.FinalStatus)
            .NotEmpty()
            .Must(AiStreamFinalStatuses.IsSupported)
            .WithMessage("FinalStatus không hợp lệ.");

        RuleFor(x => x.OutputTokens)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.LatencyMs)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ErrorMessage)
            .MaximumLength(4000)
            .When(x => string.IsNullOrWhiteSpace(x.ErrorMessage) == false);
    }
}
