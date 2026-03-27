/*
 * ===================================================================
 * FILE: CreateConversationCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.CreateConversation
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý logic khi Người Dùng (Khách) bấm nút "Bắt đầu Chat" với một Reader.
 *   Quản lý vòng đời Box-Chat (Document MongoDB).
 *
 * MỘT SỐ NGUYÊN TẮC QUAN TRỌNG:
 *   1. Chặn Spam (1 User - 1 Reader chỉ được phép mở 1 Box Chat duy nhất nếu chưa kết thúc).
 *   2. Thân thiện với Frontend (UX Tốt):
 *      Nếu frontend gọi lỡ nhấn đúp mở box 2 lần, đừng văng Lỗi Error Server. 
 *      Thay vào đó, chỉ lấy cái Box hiện tại ném về là xong.
 *   3. Định dạng thời gian ngâm Chat (24 Giờ).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

/// <summary>
/// Xử lý tạo Object Document Conversation nhúng vào MongoDB.
/// </summary>
public partial class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
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
        ValidateRequest(request);
        await EnsureReaderIsAvailableAsync(request, cancellationToken);
        await EnsureUserActiveLimitAsync(request, cancellationToken);

        var existing = await _conversationRepo.GetActiveByParticipantsAsync(
            request.UserId.ToString(),
            request.ReaderId.ToString(),
            cancellationToken);
        if (existing != null)
        {
            return existing;
        }

        var conversation = BuildConversation(request);
        await _conversationRepo.AddAsync(conversation, cancellationToken);
        return conversation;
    }
}
