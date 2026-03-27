using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public partial class ListMessagesQueryHandler
{
    private async Task<ConversationDto> LoadAuthorizedConversationAsync(
        ListMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }

        return conversation;
    }
}
