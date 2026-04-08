using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public partial class ListMessagesQueryHandler
{
    /// <summary>
    /// Tải conversation và kiểm tra requester có quyền truy cập hay không.
    /// Luồng xử lý: lấy conversation theo id, kiểm tra requester thuộc participant, trả conversation hợp lệ cho luồng list message.
    /// </summary>
    private async Task<ConversationDto> LoadAuthorizedConversationAsync(
        ListMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            // Chặn truy cập conversation của người không phải participant.
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }

        return conversation;
    }
}
