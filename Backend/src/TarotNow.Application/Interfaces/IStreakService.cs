using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IStreakService
{
        Task IncrementStreakOnValidDrawAsync(Guid userId, CancellationToken cancellationToken = default);
}
