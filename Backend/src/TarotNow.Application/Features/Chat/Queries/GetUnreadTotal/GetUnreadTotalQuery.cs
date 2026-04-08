using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetUnreadTotal;

// Query lấy tổng số tin nhắn chưa đọc của user.
public class GetUnreadTotalQuery : IRequest<GetUnreadTotalResult>
{
    // Định danh user cần tính unread tổng.
    public Guid UserId { get; set; }
}

// DTO kết quả tổng unread.
public class GetUnreadTotalResult
{
    // Tổng số tin chưa đọc của user trên toàn bộ conversation.
    public int Count { get; set; }
}

// Handler truy vấn unread tổng.
public class GetUnreadTotalQueryHandler : IRequestHandler<GetUnreadTotalQuery, GetUnreadTotalResult>
{
    private readonly IConversationRepository _conversationRepo;

    /// <summary>
    /// Khởi tạo handler lấy unread tổng.
    /// Luồng xử lý: nhận conversation repository để tính tổng unread theo user.
    /// </summary>
    public GetUnreadTotalQueryHandler(IConversationRepository conversationRepo)
    {
        _conversationRepo = conversationRepo;
    }

    /// <summary>
    /// Xử lý truy vấn tổng unread.
    /// Luồng xử lý: gọi repository tính unread theo user id, rồi map ra DTO kết quả.
    /// </summary>
    public async Task<GetUnreadTotalResult> Handle(GetUnreadTotalQuery request, CancellationToken cancellationToken)
    {
        var count = await _conversationRepo.GetTotalUnreadCountAsync(request.UserId.ToString(), cancellationToken);
        return new GetUnreadTotalResult { Count = count };
    }
}
