

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IChatFinanceRepository
{
    
    Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default);
    Task<List<ChatFinanceSession>> GetSessionsByConversationRefsAsync(IEnumerable<string> conversationRefs, CancellationToken ct = default);
    Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default);
    
    
    Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default);
    Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default);
    Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default);

    
    Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default);
    Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default); 
    Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default);
    Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
    Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default);
    Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default);

    
    
    
    Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default);

    
    Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default);

    
    Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default);

    Task<List<ChatQuestionItem>> GetDisputedItemsForAutoResolveAsync(DateTime dueAtUtc, CancellationToken ct = default);

    Task<(IReadOnlyList<ChatQuestionItem> Items, long TotalCount)> GetDisputedItemsPaginatedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<long> CountRecentDisputesByReceiverAsync(
        Guid receiverId,
        DateTime fromUtc,
        CancellationToken ct = default);

    
    Task SaveChangesAsync(CancellationToken ct = default);
}
