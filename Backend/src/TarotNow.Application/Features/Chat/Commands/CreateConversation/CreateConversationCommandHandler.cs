using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

/// <summary>
/// Handler tạo conversation mới.
///
/// Validation:
/// 1. Không cho user chat với chính mình.
/// 2. Reader phải tồn tại + có profile.
/// 3. Không cho tạo duplicate (đã có conversation active).
/// 4. Tạo document với status = pending.
/// </summary>
public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;

    public CreateConversationCommandHandler(
        IConversationRepository conversationRepo,
        IReaderProfileRepository readerProfileRepo)
    {
        _conversationRepo = conversationRepo;
        _readerProfileRepo = readerProfileRepo;
    }

    public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        // 1. Không cho chat với chính mình
        if (request.UserId == request.ReaderId)
            throw new BadRequestException("Bạn không thể tạo cuộc trò chuyện với chính mình.");

        // 2. Kiểm tra reader có profile
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(
            request.ReaderId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy Reader.");

        // 3. Gate: Chỉ cho phép tạo conversation khi reader đang accepting_questions
        if (readerProfile.Status != ReaderOnlineStatus.AcceptingQuestions)
            throw new BadRequestException("Reader hiện chưa bật chế độ nhận câu hỏi.");

        // 4. Kiểm tra duplicate — đã có conversation active chưa
        var existing = await _conversationRepo.GetActiveByParticipantsAsync(
            request.UserId.ToString(), request.ReaderId.ToString(), cancellationToken);

        if (existing != null)
        {
            // Trả về conversation hiện có thay vì lỗi — UX tốt hơn
            return existing;
        }

        // 4. Tạo conversation mới
        var conversation = new ConversationDto
        {
            UserId = request.UserId.ToString(),
            ReaderId = request.ReaderId.ToString(),
            Status = ConversationStatus.Pending,
            UnreadCountUser = 0,
            UnreadCountReader = 0,
            OfferExpiresAt = DateTime.UtcNow.AddHours(24), // 24h để Reader phản hồi
            CreatedAt = DateTime.UtcNow
        };

        await _conversationRepo.AddAsync(conversation, cancellationToken);
        return conversation;
    }
}
