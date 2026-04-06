using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetUnreadTotal;

public class GetUnreadTotalQuery : IRequest<GetUnreadTotalResult>
{
    public Guid UserId { get; set; }
}

public class GetUnreadTotalResult
{
    public int Count { get; set; }
}

public class GetUnreadTotalQueryHandler : IRequestHandler<GetUnreadTotalQuery, GetUnreadTotalResult>
{
    private readonly IConversationRepository _conversationRepo;

    public GetUnreadTotalQueryHandler(IConversationRepository conversationRepo)
    {
        _conversationRepo = conversationRepo;
    }

    public async Task<GetUnreadTotalResult> Handle(GetUnreadTotalQuery request, CancellationToken cancellationToken)
    {
        var count = await _conversationRepo.GetTotalUnreadCountAsync(request.UserId.ToString(), cancellationToken);
        return new GetUnreadTotalResult { Count = count };
    }
}
