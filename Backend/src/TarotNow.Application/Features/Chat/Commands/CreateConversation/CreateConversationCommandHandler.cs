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
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

/// <summary>
/// Xử lý tạo Object Document Conversation nhúng vào MongoDB.
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
        // 1. Chặn tự Kỷ: Không cho chat với bản thân (Lỗi nghiệp vụ Logic).
        if (request.UserId == request.ReaderId)
            throw new BadRequestException("Bạn không thể tạo cuộc trò chuyện với chính mình.");

        // 2. Chặn lỗi ảo: Reader chả thấy bóng dáng đâu trên hệ thống.
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(
            request.ReaderId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy Reader.");

        // 3. Rào cản trạng thái Cửa hàng (Gate):
        // [CẬP NHẬT THEO YÊU CẦU]: Bỏ chặn trạng thái ở đây để user luôn có thể 
        // tạo cuộc trò chuyện và nhắn tin cho Reader ngay cả khi Reader đang ngoại tuyến (offline)
        // hoặc không nhận khách (not_accepting).
        // Xóa điều kiện (readerProfile.Status != ReaderOnlineStatus.AcceptingQuestions).

        // 4. Giải Quyết Vấn Đề UX (User Experience) - Double Click Bug:
        // Quét trên MongoDB xem 2 ID này đang có Hợp đồng chat nào Mở cửa (Active/Pending) không.
        var existing = await _conversationRepo.GetActiveByParticipantsAsync(
            request.UserId.ToString(), request.ReaderId.ToString(), cancellationToken);

        if (existing != null)
        {
            // Trả về Conversation cũ Đang có thay vì Throw Excéption để Front-end tự hiểu và điều hướng tới UI cũ.
            return existing;
        }

        // -------------------------------------------------------------
        //  TẠO ĐỘNG BỘ GIAO THỨC CHAT MỚI 100% (MongoDB)
        // -------------------------------------------------------------
        var conversation = new ConversationDto
        {
            UserId = request.UserId.ToString(),
            ReaderId = request.ReaderId.ToString(),
            // Khởi tạo Pending chờ Reader bốc máy. Reader chưa bốc máy thì chưa tính tiền.
            Status = ConversationStatus.Pending,
            
            UnreadCountUser = 0,
            UnreadCountReader = 0,
            
            // Ép Hạn chót: Reader phải Bốc máy trong vòng 24 Giờ. Nếu không Hợp đồng tự Tiêu Tán (Timeout Reject).
            OfferExpiresAt = DateTime.UtcNow.AddHours(24), 
            
            CreatedAt = DateTime.UtcNow
        };

        // Ghi xuống NoSQL Database.
        await _conversationRepo.AddAsync(conversation, cancellationToken);
        return conversation;
    }
}
