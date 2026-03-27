using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
{
    private readonly record struct RequestContext(
        ConversationDto Conversation,
        string RequesterId,
        bool IsUserRequester,
        bool IsFirstRequest,
        DateTime? RequesterConfirmedAt,
        DateTime? ResponderConfirmedAt,
        DateTime Now);

    private async Task<RequestContext> BuildContextAsync(
        RequestConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            throw new BadRequestException("Bạn không thuộc cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            throw new BadRequestException($"Không thể yêu cầu hoàn thành ở trạng thái '{conversation.Status}'.");
        }

        var now = DateTime.UtcNow;
        conversation.Confirm ??= new ConversationConfirmDto();

        var isUserRequester = conversation.UserId == requesterId;
        var requesterConfirmedAt = isUserRequester ? conversation.Confirm.UserAt : conversation.Confirm.ReaderAt;
        var responderConfirmedAt = isUserRequester ? conversation.Confirm.ReaderAt : conversation.Confirm.UserAt;
        var isFirstRequest = conversation.Confirm.UserAt == null && conversation.Confirm.ReaderAt == null;

        return new RequestContext(
            conversation,
            requesterId,
            isUserRequester,
            isFirstRequest,
            requesterConfirmedAt,
            responderConfirmedAt,
            now);
    }

    private static bool IsAlreadyConfirmedByRequester(RequestContext context)
    {
        return context.RequesterConfirmedAt != null && context.ResponderConfirmedAt == null;
    }

}
