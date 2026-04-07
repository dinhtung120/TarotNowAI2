using FluentValidation;

namespace TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

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
