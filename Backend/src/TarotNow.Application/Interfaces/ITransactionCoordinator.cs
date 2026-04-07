

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface ITransactionCoordinator
{
        Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
}
