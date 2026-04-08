using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Call.Queries.GetActiveCallsByConversationIds;

// Query lấy danh sách cuộc gọi đang active theo tập conversation id.
public class GetActiveCallsByConversationIdsQuery : IRequest<IReadOnlyList<CallSessionDto>>
{
    // Danh sách conversation id cần tra cứu active call.
    public IReadOnlyCollection<string> ConversationIds { get; set; } = Array.Empty<string>();
}

// Handler truy vấn active call theo nhiều conversation id.
public class GetActiveCallsByConversationIdsQueryHandler
    : IRequestHandler<GetActiveCallsByConversationIdsQuery, IReadOnlyList<CallSessionDto>>
{
    private readonly ICallSessionRepository _callSessionRepository;

    /// <summary>
    /// Khởi tạo handler get active calls by conversation ids.
    /// Luồng xử lý: nhận call session repository để truy vấn batch active calls.
    /// </summary>
    public GetActiveCallsByConversationIdsQueryHandler(ICallSessionRepository callSessionRepository)
    {
        _callSessionRepository = callSessionRepository;
    }

    /// <summary>
    /// Xử lý query lấy active calls theo danh sách conversation.
    /// Luồng xử lý: trả rỗng nếu input rỗng, ngược lại truy vấn repository và materialize về mảng.
    /// </summary>
    public async Task<IReadOnlyList<CallSessionDto>> Handle(
        GetActiveCallsByConversationIdsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ConversationIds.Count == 0)
        {
            // Edge case không có conversation id: trả danh sách rỗng ngay để tránh query thừa.
            return Array.Empty<CallSessionDto>();
        }

        var calls = await _callSessionRepository.GetActiveByConversationIdsAsync(
            request.ConversationIds,
            cancellationToken);

        // Materialize để đảm bảo snapshot dữ liệu ổn định trước khi trả ra ngoài.
        return calls.ToArray();
    }
}
