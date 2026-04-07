using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

public class GetCallHistoryQuery : IRequest<(IEnumerable<CallSessionDto> Items, long TotalCount)>
{
        public string ConversationId { get; set; } = string.Empty;

        public Guid ParticipantId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
}
