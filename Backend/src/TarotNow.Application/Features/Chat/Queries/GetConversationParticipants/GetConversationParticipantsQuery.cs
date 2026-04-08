using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetConversationParticipants;

// Query lấy thông tin participant của một conversation.
public class GetConversationParticipantsQuery : IRequest<ConversationParticipantsDto?>
{
    // Định danh conversation cần truy vấn.
    public string ConversationId { get; set; } = string.Empty;
}

// DTO trả về hai participant chính của conversation.
public class ConversationParticipantsDto
{
    // Định danh user trong conversation.
    public string UserId { get; set; } = string.Empty;

    // Định danh reader trong conversation.
    public string ReaderId { get; set; } = string.Empty;
}

// Handler truy vấn participant của conversation.
public class GetConversationParticipantsQueryHandler
    : IRequestHandler<GetConversationParticipantsQuery, ConversationParticipantsDto?>
{
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler lấy participant conversation.
    /// Luồng xử lý: nhận conversation repository để đọc dữ liệu participant.
    /// </summary>
    public GetConversationParticipantsQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý truy vấn lấy participant của conversation.
    /// Luồng xử lý: tải conversation theo id, trả null nếu không tồn tại, ngược lại map UserId/ReaderId sang DTO.
    /// </summary>
    public async Task<ConversationParticipantsDto?> Handle(
        GetConversationParticipantsQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(
            request.ConversationId,
            cancellationToken);

        if (conversation == null)
        {
            // Edge case: conversation không tồn tại thì trả null để caller tự quyết định phản hồi.
            return null;
        }

        return new ConversationParticipantsDto
        {
            UserId = conversation.UserId,
            ReaderId = conversation.ReaderId
        };
    }
}
