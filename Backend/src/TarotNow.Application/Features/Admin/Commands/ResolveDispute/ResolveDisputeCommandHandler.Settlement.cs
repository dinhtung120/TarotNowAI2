using TarotNow.Application.Exceptions;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    /// <summary>
    /// Thực thi settlement dispute cho question item theo action đã chuẩn hóa.
    /// Luồng xử lý: khóa item để cập nhật, kiểm tra trạng thái hợp lệ, áp dụng policy reader, rẽ nhánh settle, lưu thay đổi.
    /// </summary>
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
            // Rule an toàn: chỉ settle item đang disputed, chặn các trạng thái khác.
            throw new BadRequestException("Câu hỏi không ở trạng thái dispute.");
        }

        if (item.ReleasedAt != null || item.RefundedAt != null)
        {
            // Edge case idempotency: item đã settle trước đó thì không xử lý lại.
            throw new BadRequestException("Dispute này đã được settle trước đó.");
        }

        // Áp dụng chính sách reader trước khi thực hiện các bút toán settle.
        await FreezeReaderIfDisputeThresholdExceededAsync(item, cancellationToken);

        if (action == "release")
        {
            // Nhánh release: chuyển tiền cho reader theo rule phí nền tảng.
            await ReleaseToReaderAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }
        else if (action == "refund")
        {
            // Nhánh refund: hoàn toàn bộ tiền về user trả phí.
            await RefundToUserAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }
        else
        {
            // Nhánh split: chia tiền giữa reader và user theo phần trăm admin quyết định.
            var defaultSplitPercent = _systemConfigSettings.AdminDisputeDefaultSplitPercentToReader;
            await SplitBetweenReaderAndUserAsync(
                request.AdminId,
                item,
                request.SplitPercentToReader ?? defaultSplitPercent,
                auditMetadata,
                cancellationToken);
        }

        // Persist đồng bộ item/session trước khi lưu toàn transaction.
        await _financeRepo.UpdateItemAsync(item, cancellationToken);
        await ReduceSessionFrozenBalanceAsync(item, cancellationToken);
        await _financeRepo.SaveChangesAsync(cancellationToken);
        // Cập nhật conversation và phát system message sau khi settle tài chính thành công.
        await MarkConversationResolvedAsync(item, cancellationToken);
    }

    /// <summary>
    /// Chuyển hội thoại từ disputed về completed và thêm system message thông báo admin đã xử lý.
    /// Luồng xử lý: tải conversation, kiểm tra trạng thái, cập nhật trạng thái + thời gian, thêm tin nhắn hệ thống.
    /// </summary>
    private async Task MarkConversationResolvedAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(item.ConversationRef, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Disputed)
        {
            // Edge case hội thoại không tồn tại hoặc không còn disputed: bỏ qua cập nhật trạng thái.
            return;
        }

        var now = DateTime.UtcNow;
        // Đổi state hội thoại về completed sau khi tranh chấp đã có kết quả cuối.
        conversation.Status = ConversationStatus.Completed;
        conversation.UpdatedAt = now;

        // Thêm system message để cả hai phía biết dispute đã được admin giải quyết.
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
        // Persist trạng thái hội thoại sau khi đã có message hệ thống mới nhất.
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
