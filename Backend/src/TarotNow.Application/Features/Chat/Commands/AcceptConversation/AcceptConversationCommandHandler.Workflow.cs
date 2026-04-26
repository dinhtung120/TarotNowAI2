using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public partial class AcceptConversationCommandExecutor
{
    /// <summary>
    /// Tải conversation và kiểm tra điều kiện hợp lệ để reader accept.
    /// Luồng xử lý: lấy conversation theo id, kiểm tra reader ownership và trạng thái awaiting acceptance.
    /// </summary>
    private async Task<ConversationDto> LoadConversationForAcceptAsync(
        AcceptConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            // Chặn reader không thuộc conversation thực hiện accept.
            throw new BadRequestException("Bạn không thể accept cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.AwaitingAcceptance)
        {
            // Chỉ accept được khi conversation đang chờ phản hồi reader.
            throw new BadRequestException($"Không thể accept ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

    /// <summary>
    /// Accept câu hỏi chính và kích hoạt finance session tương ứng trong transaction.
    /// Luồng xử lý: lấy main question pending + session escrow, cập nhật state/due time rồi lưu đồng bộ.
    /// </summary>
    private async Task AcceptMainQuestionAsync(
        ConversationDto conversation,
        DateTime now,
        CancellationToken cancellationToken)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var mainQuestion = await GetPendingMainQuestionAsync(conversation, transactionCt);
            var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, transactionCt)
                ?? throw new BadRequestException("Không tìm thấy phiên escrow cho cuộc trò chuyện này.");

            // Đổi trạng thái câu hỏi chính và thiết lập mốc SLA/auto-refund.
            mainQuestion.Status = QuestionItemStatus.Accepted;
            mainQuestion.AcceptedAt = now;
            mainQuestion.OfferExpiresAt = null;
            mainQuestion.ReaderResponseDueAt = now.AddHours(ResolveSlaHours(conversation.SlaHours));
            mainQuestion.AutoRefundAt = mainQuestion.ReaderResponseDueAt;
            mainQuestion.UpdatedAt = now;

            // Kích hoạt finance session để bước xử lý chat/settlement tiếp theo sử dụng.
            session.Status = ChatFinanceSessionStatus.Active;
            session.UpdatedAt = now;

            await _financeRepository.UpdateItemAsync(mainQuestion, transactionCt);
            await _financeRepository.UpdateSessionAsync(session, transactionCt);
            await _financeRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);
    }

    /// <summary>
    /// Lấy main question pending mới nhất của conversation.
    /// Luồng xử lý: tải finance session theo conversation, lấy danh sách item, chọn main question mới nhất còn pending.
    /// </summary>
    private async Task<Domain.Entities.ChatQuestionItem> GetPendingMainQuestionAsync(
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy phiên escrow cho cuộc trò chuyện này.");

        var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        var mainQuestion = items
            .Where(item => item.Type == QuestionItemType.MainQuestion)
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefault()
            ?? throw new BadRequestException("Không tìm thấy câu hỏi chính của cuộc trò chuyện.");

        if (mainQuestion.Status != QuestionItemStatus.Pending)
        {
            // Edge case main question đã bị xử lý trước đó bởi luồng khác.
            throw new BadRequestException("Câu hỏi chính không còn ở trạng thái chờ accept.");
        }

        return mainQuestion;
    }
}
