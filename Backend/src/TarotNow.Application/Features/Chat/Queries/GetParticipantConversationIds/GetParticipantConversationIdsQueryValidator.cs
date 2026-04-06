using FluentValidation;

namespace TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

/// <summary>
/// FluentValidation rules for loading participant conversation ids.
/// </summary>
public sealed class GetParticipantConversationIdsQueryValidator : AbstractValidator<GetParticipantConversationIdsQuery>
{
    public GetParticipantConversationIdsQueryValidator()
    {
        RuleFor(x => x.ParticipantId)
            .NotEmpty();

        RuleFor(x => x.MaxCount)
            .InclusiveBetween(1, 200);
    }
}
