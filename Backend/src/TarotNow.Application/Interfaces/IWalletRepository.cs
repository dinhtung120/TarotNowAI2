

using TarotNow.Domain.Enums;
using TarotNow.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IWalletRepository
{
    Task<WalletOperationResult> CreditWithResultAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task CreditAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    Task<WalletOperationResult> DebitWithResultAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task DebitAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task FreezeAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task ReleaseAsync(Guid payerId, Guid receiverId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task RefundAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

        Task ConsumeAsync(Guid userId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);
}
