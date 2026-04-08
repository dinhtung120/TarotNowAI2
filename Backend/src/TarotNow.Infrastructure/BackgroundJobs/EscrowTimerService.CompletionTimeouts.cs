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
                // Giữ vòng quét tiếp tục cho conversation khác dù một item lỗi.
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

        await AutoSettleCompletionTimeoutAsync(dependencies, escrowSettlementService, conversation.Id, cancellationToken);
        var systemMessage = BuildCompletionTimeoutMessage(conversation);
        await dependencies.MessageRepository.AddAsync(systemMessage, cancellationToken);
        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.Confirm!.AutoResolveAt = null;
        conversation.LastMessageAt = systemMessage.CreatedAt;
        conversation.UpdatedAt = systemMessage.CreatedAt;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
        // Đồng bộ trạng thái completed + system message để UI hiển thị kết quả timeout nhất quán.
    }
}
