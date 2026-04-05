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
                var callRepo = _callSessionRepository;
                
                // Tránh N+1: Lấy danh sách conversation 1 lần
                var result = await _conversationRepository.GetByParticipantIdPaginatedAsync(
                    userIdStr, 1, 50, new[] 
                    { 
                        ConversationStatuses.Ongoing, 
                        ConversationStatuses.Pending, 
                        ConversationStatuses.AwaitingAcceptance 
                    });

                var conversationIds = result.Items.Select(x => x.Id).ToList();
                if (conversationIds.Any())
                {
                    // Tránh N+1: Lấy danh sách các cuộc gọi Active theo danh sách Conversation 1 lần
                    var activeCalls = await callRepo.GetActiveByConversationIdsAsync(conversationIds);
                    
                    foreach (var activeCall in activeCalls)
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
                            activeCall.Status = CallSessionStatus.Ended;
                            activeCall.EndedAt = DateTime.UtcNow;
                            activeCall.EndReason = "disconnected";

                            // Thông báo mạng
                            await Clients.Group(ConversationGroup(activeCall.ConversationId)).SendAsync("call.ended", new
                            {
                                session = activeCall,
                                reason = "disconnected"
                            });

                            // FIX #2: Sinh CallLog khi disconnect
                            try
                            {
                                var logCmd = new TarotNow.Application.Features.Chat.Commands.SendMessage.SendMessageCommand
                                {
                                    ConversationId = activeCall.ConversationId,
                                    SenderId = Guid.Parse(activeCall.InitiatorId),
                                    Type = ChatMessageType.CallLog,
                                    Content = string.Empty,
                                    CallPayload = activeCall
                                };

                                var messageDto = await _mediator.Send(logCmd);
                                await _chatHubContext.Clients.Group(ConversationGroup(activeCall.ConversationId)).SendAsync("message.created", messageDto);
                                await _chatHubContext.Clients.Group(ConversationGroup(activeCall.ConversationId)).SendAsync("conversation.updated", new
                                {
                                    conversationId = activeCall.ConversationId,
                                    type = "message_created"
                                });
                            }
                            catch (Exception chatEx)
                            {
                                _logger.LogWarning(chatEx, "Không tạo được dòng Log khi disconnect {SessionId}: {Msg}", activeCall.Id, chatEx.Message);
                            }
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
