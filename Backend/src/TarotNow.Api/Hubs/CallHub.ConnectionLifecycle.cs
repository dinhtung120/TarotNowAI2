using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Api.Hubs;

/// <summary>
/// Conversation status constants.
/// Dùng để tránh magic string khi lọc conversations.
/// </summary>
internal static class ConversationStatuses
{
    public const string Ongoing = "ongoing";
    public const string Pending = "pending";
    public const string AwaitingAcceptance = "awaiting_acceptance";
}

public partial class CallHub
{
    public override async Task OnConnectedAsync()
    {
        var userIdStr = GetUserId();
        if (string.IsNullOrEmpty(userIdStr))
            return;

        // Cho user vào Group "user:{userId}"
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userIdStr}");

        // FIX #2 và Tối ưu hóa (Scale-out): 
        // Lấy chính xác những cuộc gọi đang Active (Ongoing, Pending, AwaitingAcceptance).
        // Thay vì lấy 200 bản ghi lịch sử, Mongo sẽ trực tiếp filter để trả về ~0-3 cuộc gọi đang hoạt động.
        // Cực kì nhẹ và không tải (load) rác vào RAM Web Server.
        var result = await _conversationRepository.GetByParticipantIdPaginatedAsync(
            userIdStr, 1, 50, new[] 
            { 
                ConversationStatuses.Ongoing, 
                ConversationStatuses.Pending, 
                ConversationStatuses.AwaitingAcceptance 
            });
        
        foreach (var conv in result.Items)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ConversationGroup(conv.Id));
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// FIX #3: Cleanup khi user disconnect (đóng tab / mất mạng).
    /// 
    /// Nếu user đang có cuộc gọi active (Requested hoặc Accepted), tự động kết thúc nó.
    /// Nếu không làm điều này, cuộc gọi sẽ bị "zombie" (stuck mãi mãi trong DB)
    /// và chặn tất cả cuộc gọi mới vào conversation đó (do guard GetActiveByConversationAsync).
    /// 
    /// QUAN TRỌNG: Frontend beforeunload không đáng tin cậy (đặc biệt mobile),
    /// nên Backend PHẢI tự cleanup.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdStr = GetUserId();
        if (!string.IsNullOrEmpty(userIdStr))
        {
            try
            {
                // FIX #22: Sử dụng _callSessionRepository đã inject qua constructor.
                // KHÔNG dùng Context.GetHttpContext()?.RequestServices.GetService<>() 
                // vì RequestServices scope đã bị dispose khi WebSocket đóng.
                var callRepo = _callSessionRepository;

                // Tìm tất cả conversations active của user để check active calls
                var result = await _conversationRepository.GetByParticipantIdPaginatedAsync(
                    userIdStr, 1, 50, new[] { ConversationStatuses.Ongoing });

                foreach (var conv in result.Items)
                {
                    var activeCall = await callRepo.GetActiveByConversationAsync(conv.Id);
                    if (activeCall == null) continue;

                    // Chỉ cleanup nếu user hiện tại là một phần của cuộc gọi (caller hoặc receiver)
                    // — receiver = anyone in conversation who is NOT initiator
                    if (conv.UserId == userIdStr || conv.ReaderId == userIdStr)
                    {
                        var updated = await callRepo.UpdateStatusAsync(
                            activeCall.Id,
                            CallSessionStatus.Ended,
                            startedAt: null,
                            endedAt: DateTime.UtcNow,
                            endReason: "disconnected",
                            expectedPreviousStatus: activeCall.Status);

                        if (updated)
                        {
                            // Thông báo cho đối phương còn lại rằng cuộc gọi đã kết thúc
                            await Clients.Group(ConversationGroup(conv.Id)).SendAsync("call.ended", new
                            {
                                session = activeCall,
                                reason = "disconnected"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Lỗi cleanup active call khi disconnect user {UserId}", userIdStr);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
