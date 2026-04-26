using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandExecutor
{
    /// <summary>
    /// Thử đánh dấu item Accepted đã có phản hồi từ Reader.
    /// Luồng xử lý: bỏ qua khi không đúng điều kiện, ngược lại chạy transaction để cập nhật repliedAt/autoReleaseAt cho các item liên quan.
    /// </summary>
    private async Task TryMarkReaderRepliedAsync(
        ConversationDto conversation,
        string senderId,
        string messageType,
        CancellationToken cancellationToken)
    {
        if (ShouldSkipReaderReplyMark(conversation, senderId, messageType))
        {
            // Không đạt điều kiện phản hồi hợp lệ thì không tác động item tài chính.
            return;
        }

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => MarkAcceptedItemsAsRepliedAsync(conversation.Id, transactionCt),
            cancellationToken);
    }

    /// <summary>
    /// Kiểm tra có cần bỏ qua đánh dấu reader replied hay không.
    /// Luồng xử lý: chỉ cho phép khi conversation Ongoing, sender là Reader và loại message là tín hiệu phản hồi thực tế.
    /// </summary>
    private static bool ShouldSkipReaderReplyMark(
        ConversationDto conversation,
        string senderId,
        string messageType)
    {
        return conversation.Status != ConversationStatus.Ongoing
               || senderId != conversation.ReaderId
               || IsReaderReplySignalMessageType(messageType) == false;
    }

    /// <summary>
    /// Đánh dấu các item Accepted chưa replied thành đã replied.
    /// Luồng xử lý: tải session/items, lọc candidate chưa replied, cập nhật repliedAt + autoReleaseAt rồi lưu.
    /// </summary>
    private async Task MarkAcceptedItemsAsRepliedAsync(
        string conversationId,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(conversationId, cancellationToken);
        if (session == null)
        {
            // Edge case: conversation chưa có finance session.
            return;
        }

        var items = await _financeRepo.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        var candidates = items
            .Where(item => item.Status == QuestionItemStatus.Accepted && item.RepliedAt == null)
            .ToList();

        if (candidates.Count == 0)
        {
            // Không có item cần cập nhật thì thoát sớm để tránh ghi DB thừa.
            return;
        }

        var now = DateTime.UtcNow;
        var autoReleaseHours = _systemConfigSettings.EscrowDisputeWindowHours;
        foreach (var item in candidates)
        {
            // Sau khi Reader phản hồi, bắt đầu cửa sổ auto-release cho item đã accepted.
            item.RepliedAt = now;
            item.AutoReleaseAt = now.AddHours(autoReleaseHours);
            item.UpdatedAt = now;
            await _financeRepo.UpdateItemAsync(item, cancellationToken);
        }

        // Persist batch cập nhật replied cho toàn bộ candidate.
        await _financeRepo.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Kiểm tra loại message có được xem là tín hiệu Reader đã trả lời hay không.
    /// Luồng xử lý: chỉ chấp nhận các loại message thể hiện nội dung trả lời thực tế.
    /// </summary>
    private static bool IsReaderReplySignalMessageType(string type)
    {
        return type is ChatMessageType.Text
            or ChatMessageType.Image
            or ChatMessageType.Voice
            or ChatMessageType.CardShare;
    }
}
