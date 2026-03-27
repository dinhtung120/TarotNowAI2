using Microsoft.Extensions.Logging;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessCompletionTimeouts(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        CancellationToken cancellationToken)
    {
        var dueConversations = await dependencies.ConversationRepository
            .GetConversationsAwaitingCompletionResolutionAsync(DateTime.UtcNow, 200, cancellationToken);

        foreach (var conversation in dueConversations)
        {
            try
            {
                await ProcessCompletionTimeoutConversationAsync(
                    dependencies,
                    escrowSettlementService,
                    conversation.Id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Completion timeout failed: {ConversationId}", conversation.Id);
            }
        }
    }

    private async Task ProcessCompletionTimeoutConversationAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        string conversationId,
        CancellationToken cancellationToken)
    {
        var conversation = await TryGetDueCompletionTimeoutConversationAsync(
            dependencies,
            conversationId,
            cancellationToken);
        if (conversation == null)
        {
            return;
        }

        if (BothSidesAlreadyConfirmed(conversation))
        {
            return;
        }

        await AutoSettleCompletionTimeoutAsync(dependencies, escrowSettlementService, conversation.Id, cancellationToken);
        var systemMessage = BuildCompletionTimeoutMessage(conversation);
        await dependencies.MessageRepository.AddAsync(systemMessage, cancellationToken);
        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.Confirm!.AutoResolveAt = null;
        conversation.LastMessageAt = systemMessage.CreatedAt;
        conversation.UpdatedAt = systemMessage.CreatedAt;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }

}
