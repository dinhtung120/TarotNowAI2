using Microsoft.Extensions.Logging;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét các conversation tới hạn completion timeout và xử lý tự động.
    /// Luồng xử lý: lấy danh sách due conversation, xử lý từng conversation và bắt lỗi cục bộ.
    /// </summary>
    private async Task ProcessCompletionTimeouts(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        CancellationToken cancellationToken)
    {
        var batchSize = Math.Clamp(_systemConfigSettings.OperationalEscrowCompletionTimeoutBatchSize, 1, 1000);
        const int maxDrainBatches = 20;

        for (var batchIndex = 0; batchIndex < maxDrainBatches; batchIndex++)
        {
            var dueConversations = await dependencies.ConversationRepository
                .GetConversationsAwaitingCompletionResolutionAsync(DateTime.UtcNow, batchSize, cancellationToken);
            if (dueConversations.Count == 0)
            {
                break;
            }

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
                    // Giữ vòng quét tiếp tục cho conversation khác dù một item lỗi.
                }
            }

            if (dueConversations.Count < batchSize)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Xử lý completion timeout cho một conversation cụ thể.
    /// Luồng xử lý: kiểm tra due conversation, bỏ qua khi đã confirm đủ hai bên, auto-settle và cập nhật message + trạng thái.
    /// </summary>
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
            // Conversation không còn đủ điều kiện timeout ở thời điểm xử lý.
            return;
        }

        if (BothSidesAlreadyConfirmed(conversation))
        {
            // Hai bên đã xác nhận đủ nên không cần auto-settle.
            return;
        }

        await AutoSettleCompletionTimeoutAsync(dependencies, escrowSettlementService, conversation, cancellationToken);
        // Projection Mongo được đồng bộ qua outbox event để tránh partial success giữa PG và Mongo.
    }
}
