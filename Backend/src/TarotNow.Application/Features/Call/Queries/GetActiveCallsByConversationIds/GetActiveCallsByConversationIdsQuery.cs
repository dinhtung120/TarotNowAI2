using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Call.Queries.GetActiveCallsByConversationIds;

public class GetActiveCallsByConversationIdsQuery : IRequest<IReadOnlyList<CallSessionDto>>
{
    public IReadOnlyCollection<string> ConversationIds { get; set; } = Array.Empty<string>();
}

public class GetActiveCallsByConversationIdsQueryHandler
    : IRequestHandler<GetActiveCallsByConversationIdsQuery, IReadOnlyList<CallSessionDto>>
{
    private readonly ICallSessionRepository _callSessionRepository;

    public GetActiveCallsByConversationIdsQueryHandler(ICallSessionRepository callSessionRepository)
    {
        _callSessionRepository = callSessionRepository;
    }

    public async Task<IReadOnlyList<CallSessionDto>> Handle(
        GetActiveCallsByConversationIdsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ConversationIds.Count == 0)
        {
            return Array.Empty<CallSessionDto>();
        }

        var calls = await _callSessionRepository.GetActiveByConversationIdsAsync(
            request.ConversationIds,
            cancellationToken);

        return calls.ToArray();
    }
}
