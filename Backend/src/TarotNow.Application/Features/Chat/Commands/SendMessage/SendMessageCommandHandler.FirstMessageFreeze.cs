using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Thử đóng băng phí câu hỏi chính khi user gửi tin nhắn đầu tiên.
    /// Luồng xử lý: chỉ kích hoạt ở trạng thái Pending và sender là user, đọc giá reader, dựng context freeze, thực thi transaction freeze.
    /// </summary>
    private async Task<FirstMessageFreezeResult> TryFreezeMainQuestionOnFirstUserMessageAsync(
        ConversationDto conversation,
        string senderId,
        string messageType,
        CancellationToken cancellationToken)
    {
        if (conversation.Status != ConversationStatus.Pending || senderId != conversation.UserId)
        {
            // Không phải tin đầu tiên của user thì bỏ qua cơ chế freeze.
            return FirstMessageFreezeResult.None;
        }

        var ids = ParseConversationParticipants(conversation);
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(conversation.ReaderId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        if (readerProfile.DiamondPerQuestion <= 0)
        {
            // Business rule: giá câu hỏi phải dương để tính freeze hợp lệ.
            throw new BadRequestException("Giá câu hỏi của Reader không hợp lệ.");
        }

        var amountDiamond = readerProfile.DiamondPerQuestion;
        var offerExpiresAtUtc = DateTime.UtcNow.AddHours(ResolveAwaitingAcceptanceHours(conversation.SlaHours));
        var idempotencyKey = $"main_question_{conversation.Id}";
        var freezeContext = new MainQuestionFreezeContext(
            ids.UserId,
            ids.ReaderId,
            amountDiamond,
            offerExpiresAtUtc,
            idempotencyKey);

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => FreezeMainQuestionAsync(conversation, freezeContext, transactionCt),
            cancellationToken);

        return new FirstMessageFreezeResult(true, freezeContext.AmountDiamond, freezeContext.OfferExpiresAtUtc);
    }

    /// <summary>
    /// Parse và validate định danh participant trong conversation.
    /// Luồng xử lý: chuyển userId/readerId sang Guid, ném lỗi khi dữ liệu conversation không hợp lệ.
    /// </summary>
    private static (Guid UserId, Guid ReaderId) ParseConversationParticipants(ConversationDto conversation)
    {
        if (!Guid.TryParse(conversation.UserId, out var userId))
        {
            // Edge case dữ liệu conversation bị lệch format Guid.
            throw new BadRequestException("UserId của cuộc trò chuyện không hợp lệ.");
        }

        if (!Guid.TryParse(conversation.ReaderId, out var readerId))
        {
            // Edge case dữ liệu conversation bị lệch format Guid.
            throw new BadRequestException("ReaderId của cuộc trò chuyện không hợp lệ.");
        }

        return (userId, readerId);
    }

    /// <summary>
    /// Xác định số giờ chờ chấp thuận sau khi freeze câu hỏi chính.
    /// Luồng xử lý: SLA thấp dùng 6h để phản hồi nhanh; còn lại dùng 12h để cân bằng thời gian xử lý.
    /// </summary>
    private static int ResolveAwaitingAcceptanceHours(int slaHours)
    {
        return slaHours <= 6 ? 6 : 12;
    }

    /// <summary>
    /// Dựng system message thông báo đã đóng băng kim cương.
    /// Luồng xử lý: tạo message type System với nội dung thông báo tài chính cho user.
    /// </summary>
    private static ChatMessageDto BuildSystemMessage(string conversationId, string senderId, string content)
    {
        return new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = senderId,
            Type = ChatMessageType.System,
            Content = content,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Kết quả freeze của tin nhắn đầu tiên để điều phối cập nhật state và push số dư.
    private readonly record struct FirstMessageFreezeResult(
        bool IsTriggered,
        long AmountDiamond,
        DateTime? OfferExpiresAtUtc)
    {
        // Trạng thái mặc định khi không phát sinh freeze.
        public static FirstMessageFreezeResult None => new(false, 0, null);
    }

    // Context đóng băng câu hỏi chính dùng xuyên suốt transaction freeze.
    private readonly record struct MainQuestionFreezeContext(
        Guid UserId,
        Guid ReaderId,
        long AmountDiamond,
        DateTime OfferExpiresAtUtc,
        string IdempotencyKey);
}
