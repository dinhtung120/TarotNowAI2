

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IUserCollectionRepository
{
        Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);
}
