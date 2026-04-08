using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public partial class AcceptConversationCommandHandler
{
    /// <summary>
    /// Thêm system message khi reader accept conversation.
    /// Luồng xử lý: dựng message hệ thống, lưu repository và trả mốc thời gian tạo message.
    /// </summary>
    private async Task<DateTime> AddAcceptedSystemMessageAsync(
        ConversationDto conversation,
        DateTime now,
        CancellationToken cancellationToken)
    {
        // Message hệ thống giúp hai bên biết conversation đã chính thức bắt đầu.
        var systemMessage = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.System,
            Content = "Reader đã chấp nhận câu hỏi. Cuộc trò chuyện bắt đầu.",
            IsRead = false,
            CreatedAt = now
        };

        await _chatMessageRepository.AddAsync(systemMessage, cancellationToken);
        return systemMessage.CreatedAt;
    }

    /// <summary>
    /// Chuẩn hóa SLA hours về tập giá trị cho phép.
    /// Luồng xử lý: nhận giá trị cấu hình, chỉ chấp nhận 6/12/24, fallback về 12 nếu ngoài tập.
    /// </summary>
    private static int ResolveSlaHours(int configuredSlaHours)
    {
        return configuredSlaHours is 6 or 12 or 24 ? configuredSlaHours : 12;
    }
}
