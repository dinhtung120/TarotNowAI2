using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

// Query kiểm tra quyền truy cập conversation của requester.
public class ValidateConversationAccessQuery : IRequest<ConversationAccessStatus>
{
    // Định danh conversation cần kiểm tra.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh requester cần xác thực quyền.
    public Guid RequesterId { get; set; }
}

// Kết quả kiểm tra quyền truy cập conversation.
public enum ConversationAccessStatus
{
    // Requester có quyền truy cập conversation.
    Allowed = 1,

    // Conversation không tồn tại.
    NotFound = 2,

    // Conversation tồn tại nhưng requester không thuộc participant.
    Forbidden = 3
}

// Handler kiểm tra quyền truy cập conversation.
public class ValidateConversationAccessQueryHandler
    : IRequestHandler<ValidateConversationAccessQuery, ConversationAccessStatus>
{
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler kiểm tra quyền conversation.
    /// Luồng xử lý: nhận conversation repository để đối chiếu participant theo conversation id.
    /// </summary>
    public ValidateConversationAccessQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý query kiểm tra quyền truy cập conversation.
    /// Luồng xử lý: nếu conversation không tồn tại thì NotFound, nếu requester không thuộc participant thì Forbidden, ngược lại Allowed.
    /// </summary>
    public async Task<ConversationAccessStatus> Handle(
        ValidateConversationAccessQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
        {
            // Edge case: không có conversation tương ứng với id đầu vào.
            return ConversationAccessStatus.NotFound;
        }

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            // Requester không phải participant của conversation.
            return ConversationAccessStatus.Forbidden;
        }

        return ConversationAccessStatus.Allowed;
    }
}
