using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public class StreamReadingCommandValidator : AbstractValidator<StreamReadingCommand>
{
    public StreamReadingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ReadingSessionId)
            .NotEmpty();

        RuleFor(x => x.FollowupQuestion)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.FollowupQuestion) == false);

        RuleFor(x => x.Language)
            .NotEmpty()
            .MaximumLength(10);
    }
}
