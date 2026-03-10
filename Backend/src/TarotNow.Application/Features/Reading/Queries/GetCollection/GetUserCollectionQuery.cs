using MediatR;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

public class GetUserCollectionQuery : IRequest<List<UserCollectionDto>>
{
    public Guid UserId { get; set; }
}

public class UserCollectionDto
{
    public int CardId { get; set; }
    public int Level { get; set; }
    public int Copies { get; set; }
    public long ExpGained { get; set; }
    public DateTime LastDrawnAt { get; set; }
}
