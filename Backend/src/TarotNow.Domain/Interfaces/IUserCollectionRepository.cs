using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Interfaces;

public interface IUserCollectionRepository
{
    /// <summary>
    /// Upsert card: Nếu user chưa có thẻ này -> insert Level 1, Copies 1.
    /// Nếu đã có -> Update +1 Copy, cộng dồn EXP, Level up nếu đủ điều kiện.
    /// </summary>
    Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default);

    Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);
}
