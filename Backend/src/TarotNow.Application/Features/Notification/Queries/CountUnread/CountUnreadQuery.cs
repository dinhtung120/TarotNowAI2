

using MediatR;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

public class CountUnreadQuery : IRequest<long>
{
        public Guid UserId { get; set; }

    public CountUnreadQuery(Guid userId)
    {
        UserId = userId;
    }
}
