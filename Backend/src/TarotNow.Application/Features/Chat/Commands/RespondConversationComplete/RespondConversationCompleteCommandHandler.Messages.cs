using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandlerRequestedDomainEventHandler
{
    // Mô tả payload system message dùng chung cho các nhánh phản hồi complete.
    private readonly record struct SystemMessageSpec(string Type, string Content, DateTime? CreatedAt);

    /// <summary>
    /// Ghi system message vào conversation và trả về thời điểm tạo message.
    /// Luồng xử lý: map spec vào ChatMessageDto, lưu qua repository, rồi trả CreatedAt để cập nhật timeline conversation.
    /// </summary>
    private async Task<DateTime> AddSystemMessageAsync(
        ConversationDto conversation,
        string senderId,
        SystemMessageSpec spec,
        CancellationToken cancellationToken)
    {
        var message = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = senderId,
            Type = spec.Type,
            Content = spec.Content,
            IsRead = false,
            CreatedAt = spec.CreatedAt ?? DateTime.UtcNow
        };

        await _chatMessageRepository.AddAsync(message, cancellationToken);
        // Trả thời điểm message thực tế để đồng bộ LastMessageAt/UpdatedAt chính xác.
        return message.CreatedAt;
    }
}
