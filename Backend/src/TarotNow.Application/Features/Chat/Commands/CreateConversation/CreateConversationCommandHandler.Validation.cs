using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public partial class CreateConversationCommandHandler
{
    /// <summary>
    /// Validate nhanh các rule đầu vào cơ bản cho create conversation.
    /// Luồng xử lý: chặn self-chat và chỉ chấp nhận SLA 6/12/24 giờ.
    /// </summary>
    private static void ValidateRequest(CreateConversationCommand request)
    {
        if (request.UserId == request.ReaderId)
        {
            // Rule nghiệp vụ: user không được mở conversation với chính mình.
            throw new BadRequestException("Bạn không thể tạo cuộc trò chuyện với chính mình.");
        }

        if (request.SlaHours is not (6 or 12 or 24))
        {
            // Giữ SLA trong tập giá trị được hệ thống hỗ trợ.
            throw new BadRequestException("SLA chỉ chấp nhận 6, 12 hoặc 24 giờ.");
        }
    }

    /// <summary>
    /// Kiểm tra reader tồn tại và đang ở trạng thái sẵn sàng nhận câu hỏi.
    /// Luồng xử lý: lấy reader profile theo user id, kiểm tra status hợp lệ.
    /// </summary>
    private async Task EnsureReaderIsAvailableAsync(
        CreateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(request.ReaderId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy Reader.");

        if (!ReaderOnlineStatus.IsValid(readerProfile.Status))
        {
            // Chặn tạo conversation khi reader chưa sẵn sàng nhận phiên mới.
            throw new BadRequestException("Reader hiện chưa sẵn sàng nhận câu hỏi.");
        }
    }

    /// <summary>
    /// Kiểm tra số conversation active của user không vượt ngưỡng cho phép.
    /// Luồng xử lý: đếm active conversation theo user id và chặn nếu >= 5.
    /// </summary>
    private async Task EnsureUserActiveLimitAsync(
        CreateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var activeCount = await _conversationRepo.CountActiveByUserIdAsync(request.UserId.ToString(), cancellationToken);
        if (activeCount >= 5)
        {
            // Rule quota: giới hạn số conversation active để bảo vệ trải nghiệm và chi phí hệ thống.
            throw new BadRequestException("Bạn đã đạt giới hạn 5 cuộc trò chuyện đang hoạt động.");
        }
    }

    /// <summary>
    /// Dựng conversation mới từ command đầu vào.
    /// Luồng xử lý: map participant, trạng thái pending, counters mặc định và timestamp tạo mới.
    /// </summary>
    private static ConversationDto BuildConversation(CreateConversationCommand request)
    {
        return new ConversationDto
        {
            UserId = request.UserId.ToString(),
            ReaderId = request.ReaderId.ToString(),
            Status = ConversationStatus.Pending,
            UnreadCountUser = 0,
            UnreadCountReader = 0,
            SlaHours = request.SlaHours,
            CreatedAt = DateTime.UtcNow
        };
    }
}
