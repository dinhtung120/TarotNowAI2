using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public partial class CreateConversationCommandHandler
{
    private static void ValidateRequest(CreateConversationCommand request)
    {
        if (request.UserId == request.ReaderId)
        {
            throw new BadRequestException("Bạn không thể tạo cuộc trò chuyện với chính mình.");
        }

        if (request.SlaHours is not (6 or 12 or 24))
        {
            throw new BadRequestException("SLA chỉ chấp nhận 6, 12 hoặc 24 giờ.");
        }
    }

    private async Task EnsureReaderIsAvailableAsync(
        CreateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(request.ReaderId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy Reader.");

        if (!ReaderOnlineStatus.IsValid(readerProfile.Status))
        {
            throw new BadRequestException("Reader hiện chưa sẵn sàng nhận câu hỏi.");
        }
    }

    private async Task EnsureUserActiveLimitAsync(
        CreateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var activeCount = await _conversationRepo.CountActiveByUserIdAsync(request.UserId.ToString(), cancellationToken);
        if (activeCount >= 5)
        {
            throw new BadRequestException("Bạn đã đạt giới hạn 5 cuộc trò chuyện đang hoạt động.");
        }
    }

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
