using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

// Query lấy danh sách conversation id theo participant.
public class GetParticipantConversationIdsQuery : IRequest<IReadOnlyList<string>>
{
    // Định danh participant cần truy vấn.
    public string ParticipantId { get; set; } = string.Empty;

    // Số lượng tối đa conversation id cần lấy.
    public int MaxCount { get; set; }

    // Bộ lọc trạng thái conversation (tùy chọn).
    public IReadOnlyCollection<string>? Statuses { get; set; }
}

// Handler truy vấn conversation id theo participant.
public class GetParticipantConversationIdsQueryHandler
    : IRequestHandler<GetParticipantConversationIdsQuery, IReadOnlyList<string>>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler lấy danh sách conversation id theo participant.
    /// Luồng xử lý: nhận conversation repository để thực hiện truy vấn phân trang.
    /// </summary>
    public GetParticipantConversationIdsQueryHandler(
        IConversationRepository conversationRepository,
        ISystemConfigSettings systemConfigSettings)
    {
        _conversationRepository = conversationRepository;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý query lấy conversation id của participant.
    /// Luồng xử lý: kiểm tra participant rỗng, chuẩn hóa page size, truy vấn trang đầu và trả về tập id duy nhất.
    /// </summary>
    public async Task<IReadOnlyList<string>> Handle(
        GetParticipantConversationIdsQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ParticipantId))
        {
            // Edge case: participant rỗng thì không có dữ liệu hợp lệ để truy vấn.
            return Array.Empty<string>();
        }

        // Chuẩn hóa page size để tránh truy vấn quá lớn và giữ giới hạn hệ thống.
        var defaultPageSize = _systemConfigSettings.ChatParticipantsDefaultPageSize;
        var maxPageSize = _systemConfigSettings.ChatParticipantsMaxPageSize;
        var pageSize = request.MaxCount <= 0
            ? defaultPageSize
            : Math.Min(request.MaxCount, maxPageSize);
        var (items, _) = await _conversationRepository.GetByParticipantIdPaginatedAsync(
            request.ParticipantId,
            page: 1,
            pageSize: pageSize,
            statuses: request.Statuses,
            cancellationToken: cancellationToken);

        // Loại id rỗng và loại trùng để đảm bảo đầu ra gọn, ổn định.
        return items
            .Select(static x => x.Id)
            .Where(static id => string.IsNullOrWhiteSpace(id) == false)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
