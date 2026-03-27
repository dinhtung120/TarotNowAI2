using TarotNow.Application.Exceptions;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private async Task ResolveDisputeAsync(
        ResolveDisputeCommand request,
        string action,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        var item = await _financeRepo.GetItemForUpdateAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

        if (item.Status != QuestionItemStatus.Disputed)
        {
            throw new BadRequestException("Câu hỏi không ở trạng thái dispute.");
        }

        if (item.ReleasedAt != null || item.RefundedAt != null)
        {
            throw new BadRequestException("Dispute này đã được settle trước đó.");
        }

        await FreezeReaderIfDisputeThresholdExceededAsync(item, cancellationToken);

        if (action == "release")
        {
            await ReleaseToReaderAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }
        else if (action == "refund")
        {
            await RefundToUserAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }
        else
        {
            await SplitBetweenReaderAndUserAsync(
                request.AdminId,
                item,
                request.SplitPercentToReader ?? 50,
                auditMetadata,
                cancellationToken);
        }

        await _financeRepo.UpdateItemAsync(item, cancellationToken);
        await ReduceSessionFrozenBalanceAsync(item, cancellationToken);
        await _financeRepo.SaveChangesAsync(cancellationToken);
        await MarkConversationResolvedAsync(item, cancellationToken);
    }

    private async Task MarkConversationResolvedAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(item.ConversationRef, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Disputed)
        {
            return;
        }

        var now = DateTime.UtcNow;
        conversation.Status = ConversationStatus.Completed;
        conversation.UpdatedAt = now;

        var systemMessage = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.System,
            Content = "Tranh chấp đã được Admin xử lý. Cuộc trò chuyện đã hoàn thành.",
            IsRead = false,
            CreatedAt = now
        };

        await _chatMessageRepository.AddAsync(systemMessage, cancellationToken);
        conversation.LastMessageAt = systemMessage.CreatedAt;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
