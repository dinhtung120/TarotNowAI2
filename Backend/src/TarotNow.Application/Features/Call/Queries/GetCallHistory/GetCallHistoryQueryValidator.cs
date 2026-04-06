using FluentValidation;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

/// <summary>
/// FluentValidation rules for call history queries.
/// </summary>
public sealed class GetCallHistoryQueryValidator : AbstractValidator<GetCallHistoryQuery>
{
    public GetCallHistoryQueryValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.ParticipantId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}
