

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

// Handler chính cho luồng tạo conversation.
public partial class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;

    /// <summary>
    /// Khởi tạo handler create conversation.
    /// Luồng xử lý: nhận repository conversation và reader profile để kiểm tra điều kiện tạo mới.
    /// </summary>
    public CreateConversationCommandHandler(
        IConversationRepository conversationRepo,
        IReaderProfileRepository readerProfileRepo)
    {
        _conversationRepo = conversationRepo;
        _readerProfileRepo = readerProfileRepo;
    }

    /// <summary>
    /// Xử lý command tạo conversation.
    /// Luồng xử lý: validate request, kiểm tra reader/limit user, ưu tiên tái sử dụng active conversation hiện có, nếu không thì tạo mới.
    /// </summary>
    public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        await EnsureReaderIsAvailableAsync(request, cancellationToken);
        await EnsureUserActiveLimitAsync(request, cancellationToken);

        var existing = await _conversationRepo.GetActiveByParticipantsAsync(
            request.UserId.ToString(),
            request.ReaderId.ToString(),
            cancellationToken);
        if (existing != null)
        {
            // Tránh tạo conversation trùng khi hai participant đã có phiên active.
            return existing;
        }

        // Tạo conversation mới khi không có phiên active trùng participant.
        var conversation = BuildConversation(request);
        await _conversationRepo.AddAsync(conversation, cancellationToken);
        return conversation;
    }
}
