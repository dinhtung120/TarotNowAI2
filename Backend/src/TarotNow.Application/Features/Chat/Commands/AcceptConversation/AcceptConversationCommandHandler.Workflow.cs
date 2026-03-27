using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public partial class AcceptConversationCommandHandler
{
    private async Task<ConversationDto> LoadConversationForAcceptAsync(
        AcceptConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.ReaderId != request.ReaderId.ToString())
        {
            throw new BadRequestException("Bạn không thể accept cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.AwaitingAcceptance)
        {
            throw new BadRequestException($"Không thể accept ở trạng thái '{conversation.Status}'.");
        }

        return conversation;
    }

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

            mainQuestion.Status = QuestionItemStatus.Accepted;
            mainQuestion.AcceptedAt = now;
            mainQuestion.OfferExpiresAt = null;
            mainQuestion.ReaderResponseDueAt = now.AddHours(ResolveSlaHours(conversation.SlaHours));
            mainQuestion.AutoRefundAt = mainQuestion.ReaderResponseDueAt;
            mainQuestion.UpdatedAt = now;

            session.Status = "active";
            session.UpdatedAt = now;

            await _financeRepository.UpdateItemAsync(mainQuestion, transactionCt);
            await _financeRepository.UpdateSessionAsync(session, transactionCt);
            await _financeRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);
    }

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
            throw new BadRequestException("Câu hỏi chính không còn ở trạng thái chờ accept.");
        }

        return mainQuestion;
    }

}
