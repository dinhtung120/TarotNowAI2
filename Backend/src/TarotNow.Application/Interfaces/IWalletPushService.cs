

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IWalletPushService
{
        Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default);
}
