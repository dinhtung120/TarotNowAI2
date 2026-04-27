using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    private const string FirstMessageFreezeLockPrefix = "chat:first-message-freeze";
    private static readonly TimeSpan FirstMessageFreezeLockLease = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Thử đóng băng phí câu hỏi chính khi user gửi tin nhắn đầu tiên.
    /// Luồng xử lý: chỉ kích hoạt ở trạng thái Pending và sender là user, đọc giá reader, dựng context freeze, thực thi transaction freeze.
    /// </summary>
    private async Task<FirstMessageFreezeResult> TryFreezeMainQuestionOnFirstUserMessageAsync(
        ConversationDto conversation,
        string senderId,
        string firstUserMessageId,
        CancellationToken cancellationToken)
    {
        if (conversation.Status != ConversationStatus.Pending || senderId != conversation.UserId)
        {
            // Không phải tin đầu tiên của user thì bỏ qua cơ chế freeze.
            return FirstMessageFreezeResult.None;
        }

        if (string.IsNullOrWhiteSpace(firstUserMessageId))
        {
            throw new BadRequestException("Không thể xác định idempotency cho tin nhắn đầu tiên.");
        }

        var lockKey = BuildFirstMessageFreezeLockKey(conversation.Id);
        var lockOwner = await AcquireFirstMessageFreezeLockAsync(conversation.Id, lockKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(lockOwner))
        {
            return FirstMessageFreezeResult.None;
        }

        try
        {
            return await ExecuteFirstMessageFreezeAsync(conversation, firstUserMessageId, cancellationToken);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockOwner, CancellationToken.None);
        }
    }

    private async Task<string?> AcquireFirstMessageFreezeLockAsync(
        string conversationId,
        string lockKey,
        CancellationToken cancellationToken)
    {
        var lockOwner = Guid.NewGuid().ToString("N");
        if (await _cacheService.AcquireLockAsync(lockKey, lockOwner, FirstMessageFreezeLockLease, cancellationToken))
        {
            return lockOwner;
        }

        var latestConversation = await _conversationRepo.GetByIdAsync(conversationId, cancellationToken);
        if (latestConversation?.Status != ConversationStatus.Pending)
        {
            return null;
        }

        throw new BadRequestException("Cuộc trò chuyện đang được xử lý tin nhắn đầu tiên. Vui lòng thử lại.");
    }

    private async Task<FirstMessageFreezeResult> ExecuteFirstMessageFreezeAsync(
        ConversationDto conversation,
        string firstUserMessageId,
        CancellationToken cancellationToken)
    {
        var ids = ParseConversationParticipants(conversation);
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(conversation.ReaderId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        if (readerProfile.DiamondPerQuestion <= 0)
        {
            throw new BadRequestException("Giá câu hỏi của Reader không hợp lệ.");
        }

        var offerExpiresAtUtc = DateTime.UtcNow.AddHours(ResolveAwaitingAcceptanceHours(conversation.SlaHours));
        var freezeContext = new MainQuestionFreezeContext(
            ids.UserId,
            ids.ReaderId,
            readerProfile.DiamondPerQuestion,
            offerExpiresAtUtc,
            BuildMainQuestionIdempotencyKey(conversation.Id, firstUserMessageId));
        var executionResult = MainQuestionFreezeExecutionResult.Skipped(
            freezeContext.OfferExpiresAtUtc,
            freezeContext.IdempotencyKey);

        await _transactionCoordinator.ExecuteAsync(
            async transactionCt => executionResult = await FreezeMainQuestionAsync(conversation, freezeContext, transactionCt),
            cancellationToken);

        return executionResult.ToFirstMessageFreezeResult(freezeContext.AmountDiamond);
    }

    private static string BuildFirstMessageFreezeLockKey(string conversationId)
    {
        return $"{FirstMessageFreezeLockPrefix}:{conversationId}";
    }

    private static string BuildMainQuestionIdempotencyKey(string conversationId, string firstUserMessageId)
    {
        return $"main_question_{conversationId}_{firstUserMessageId}";
    }

    private enum FirstMessageFreezeState
    {
        None = 0,
        Applied = 1,
        AlreadyFrozen = 2
    }

    private readonly record struct MainQuestionFreezeExecutionResult(
        FirstMessageFreezeState State,
        DateTime OfferExpiresAtUtc,
        string FreezeItemIdempotencyKey)
    {
        public static MainQuestionFreezeExecutionResult Applied(
            DateTime offerExpiresAtUtc,
            string freezeItemIdempotencyKey)
            => new(FirstMessageFreezeState.Applied, offerExpiresAtUtc, freezeItemIdempotencyKey);

        public static MainQuestionFreezeExecutionResult Skipped(
            DateTime offerExpiresAtUtc,
            string freezeItemIdempotencyKey)
            => new(FirstMessageFreezeState.AlreadyFrozen, offerExpiresAtUtc, freezeItemIdempotencyKey);

        public FirstMessageFreezeResult ToFirstMessageFreezeResult(long amountDiamond)
        {
            return State switch
            {
                FirstMessageFreezeState.Applied => FirstMessageFreezeResult.Applied(
                    amountDiamond,
                    OfferExpiresAtUtc,
                    FreezeItemIdempotencyKey),
                FirstMessageFreezeState.AlreadyFrozen => FirstMessageFreezeResult.AlreadyFrozen(
                    amountDiamond,
                    OfferExpiresAtUtc,
                    FreezeItemIdempotencyKey),
                _ => FirstMessageFreezeResult.None
            };
        }
    }

    // Kết quả freeze của tin nhắn đầu tiên để điều phối cập nhật state và push số dư.
    private readonly record struct FirstMessageFreezeResult(
        FirstMessageFreezeState State,
        long AmountDiamond,
        DateTime? OfferExpiresAtUtc,
        string? FreezeItemIdempotencyKey)
    {
        public bool IsApplied => State == FirstMessageFreezeState.Applied;

        // Trạng thái mặc định khi không phát sinh freeze.
        public static FirstMessageFreezeResult None => new(FirstMessageFreezeState.None, 0, null, null);

        public static FirstMessageFreezeResult Applied(
            long amountDiamond,
            DateTime offerExpiresAtUtc,
            string freezeItemIdempotencyKey)
            => new(FirstMessageFreezeState.Applied, amountDiamond, offerExpiresAtUtc, freezeItemIdempotencyKey);

        public static FirstMessageFreezeResult AlreadyFrozen(
            long amountDiamond,
            DateTime offerExpiresAtUtc,
            string freezeItemIdempotencyKey)
            => new(FirstMessageFreezeState.AlreadyFrozen, amountDiamond, offerExpiresAtUtc, freezeItemIdempotencyKey);
    }

    // Context đóng băng câu hỏi chính dùng xuyên suốt transaction freeze.
    private readonly record struct MainQuestionFreezeContext(
        Guid UserId,
        Guid ReaderId,
        long AmountDiamond,
        DateTime OfferExpiresAtUtc,
        string IdempotencyKey);

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
}
