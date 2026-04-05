using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

/// <summary>
/// Query lấy danh sách conversation ids theo participant và status filter.
/// </summary>
public class GetParticipantConversationIdsQuery : IRequest<IReadOnlyList<string>>
{
    public string ParticipantId { get; set; } = string.Empty;
    public int MaxCount { get; set; } = 50;
    public IReadOnlyCollection<string>? Statuses { get; set; }
}

/// <summary>
/// Handler trả về các conversation ids mà participant đang tham gia.
/// </summary>
public class GetParticipantConversationIdsQueryHandler
    : IRequestHandler<GetParticipantConversationIdsQuery, IReadOnlyList<string>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetParticipantConversationIdsQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<IReadOnlyList<string>> Handle(
        GetParticipantConversationIdsQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ParticipantId))
        {
            return Array.Empty<string>();
        }

        var pageSize = request.MaxCount <= 0 ? 50 : Math.Min(request.MaxCount, 200);
        var (items, _) = await _conversationRepository.GetByParticipantIdPaginatedAsync(
            request.ParticipantId,
            page: 1,
            pageSize: pageSize,
            statuses: request.Statuses,
            cancellationToken: cancellationToken);

        return items
            .Select(static x => x.Id)
            .Where(static id => string.IsNullOrWhiteSpace(id) == false)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
