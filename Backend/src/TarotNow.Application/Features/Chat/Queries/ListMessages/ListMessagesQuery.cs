

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

// Query lấy danh sách message theo cursor của conversation.
public class ListMessagesQuery : IRequest<ListMessagesResult>
{
    // Định danh conversation cần lấy message.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh participant yêu cầu dữ liệu.
    public Guid RequesterId { get; set; }

    // Cursor phân trang (tùy chọn).
    public string? Cursor { get; set; }

    // Số lượng message tối đa mỗi lần tải.
    public int Limit { get; set; } = 50;
}

// DTO kết quả danh sách message theo cursor.
public class ListMessagesResult
{
    // Danh sách message của trang hiện tại.
    public IReadOnlyList<ChatMessageDto> Messages { get; set; } = Array.Empty<ChatMessageDto>();

    // Cursor kế tiếp cho lần truy vấn sau.
    public string? NextCursor { get; set; }

    // Conversation đã enrich profile/status/escrow để client render đồng bộ.
    public ConversationDto Conversation { get; set; } = null!;
}
