using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    public async Task<CallSessionV2Dto> EndAsync(Guid requesterId, string callSessionId, string reason, CancellationToken ct = default)
    {
        var requesterIdText = requesterId.ToString();
        var session = await GetSessionOrThrowAsync(callSessionId, ct);

        EnsureConversationParticipantOrThrow(session, requesterIdText);
        return await FinalizeSessionAsync(session, NormalizeEndReason(reason), ct);
    }

    private async Task<CallSessionV2Dto> FinalizeSessionAsync(CallSessionV2Dto session, string reason, CancellationToken ct)
    {
        if (CallSessionV2Statuses.IsFinal(session.Status)) return session;

        var ending = await _sessions.TryPatchAsync(session.Id, new CallSessionV2Patch
        {
            NewStatus = CallSessionV2Statuses.Ending,
            ExpectedPreviousStatuses = CallSessionV2Statuses.ActiveStates,
        }, ct);

        var snapshot = ending ?? await GetSessionOrThrowAsync(session.Id, ct);
        if (CallSessionV2Statuses.IsFinal(snapshot.Status)) return snapshot;

        await _rooms.DeleteRoomAsync(snapshot.RoomName, ct);
        var completed = await _sessions.TryPatchAsync(snapshot.Id, new CallSessionV2Patch
        {
            NewStatus = CallSessionV2Statuses.Ended,
            EndedAt = DateTime.UtcNow,
            EndReason = reason,
            ExpectedPreviousStatuses =
            [
                CallSessionV2Statuses.Ending,
                CallSessionV2Statuses.Requested,
                CallSessionV2Statuses.Accepted,
                CallSessionV2Statuses.Joining,
                CallSessionV2Statuses.Connected,
            ],
        }, ct);

        var finalized = completed ?? await GetSessionOrThrowAsync(session.Id, ct);
        if (CallSessionV2Statuses.IsFinal(finalized.Status))
        {
            var finalReason = finalized.EndReason ?? reason;
            if (IsAbnormalEnd(finalized.Status, finalReason))
            {
                CallV2Telemetry.RecordCallDrop(finalized.Status, finalReason);
            }

            // Phát thông báo realtime cuộc gọi kết thúc qua SignalR (để đóng popup cuộc gọi).
            await _realtimePush.BroadcastEndedAsync(finalized, finalReason, ct);

            // Tạo tin nhắn nhật ký cuộc gọi trong lịch sử chat để lưu lại kết quả.
            // Sử dụng Try để tránh lỗi tạo log làm gián đoạn luồng kết thúc call chính.
            if (finalized.IsLogCreated == false)
            {
                await TryCreateCallLogAsync(finalized, finalReason, ct);
            }
        }

        return finalized;
    }

    /// <summary>
    /// Tạo bản ghi nhật ký cuộc gọi dưới dạng một tin nhắn chat hệ thống.
    /// Luồng xử lý: tính toán thời lượng, map dữ liệu sang CallSessionDto và gửi SendMessageCommand.
    /// </summary>
    private async Task TryCreateCallLogAsync(CallSessionV2Dto session, string reason, CancellationToken ct)
    {
        try
        {
            // Kiểm tra và đánh dấu nguyên tử để tránh tạo nhiều log cho cùng một session.
            var patchResult = await _sessions.TryPatchAsync(session.Id, new CallSessionV2Patch
            {
                NewStatus = session.Status,
                IsLogCreated = true,
                ExpectedPreviousStatuses = [session.Status]
            }, ct);

            if (patchResult == null || (session.IsLogCreated == false && patchResult.IsLogCreated == false))
            {
                // Có vẻ như một instance khác đã xử lý hoặc việc đánh dấu thất bại.
                return;
            }

            // Nếu dữ liệu trả về cho thấy log đã được tạo trước đó (từ một lần gọi khác), thoát ngay.
            if (session.IsLogCreated) return;
            // Tính toán thời lượng cuộc gọi dựa trên các mốc thời gian.
            // Ưu tiên ConnectedAt, nếu thiếu (do lỗi đồng bộ) thì dùng CreatedAt làm mốc bắt đầu.
            var startTime = session.ConnectedAt ?? session.CreatedAt;
            var durationSeconds = 0;
            
            if (session.EndedAt.HasValue)
            {
                var diff = session.EndedAt.Value - startTime;
                durationSeconds = (int)Math.Max(0, diff.TotalSeconds);
            }

            _logger.LogInformation("[CallV2] Logging Session: {Id}, Start: {Start}, End: {End}, Duration: {Duration}s", 
                session.Id, startTime, session.EndedAt, durationSeconds);

            // Dựng payload cuộc gọi để frontend có thể parse và hiển thị UI (Video/Audio/Bị nhỡ).
            var callDto = new CallSessionDto
            {
                Id = session.Id,
                ConversationId = session.ConversationId,
                InitiatorId = session.InitiatorId,
                Type = session.Type == CallTypeValues.Video ? CallType.Video : CallType.Audio,
                Status = CallSessionStatus.Ended,
                DurationSeconds = durationSeconds,
                EndReason = reason
            };

            // Cấu hình JsonSerializer để tương thích tốt nhất với Frontend (camelCase + String Enums).
            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(callDto, jsonOptions);

            // Gửi lệnh qua MediatoR để tạo và lưu tin nhắn ChatMessageType.CallLog.
            var messageDto = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = session.ConversationId,
                SenderId = Guid.Parse(session.InitiatorId),
                Type = ChatMessageType.CallLog,
                Content = jsonContent,
                CallPayload = callDto
            }, ct);

            // PHÁT TIN NHẮN REALTIME:
            // Bước này cực kỳ quan trọng để tin nhắn hiện ngay trên màn hình chat của người dùng mà không cần F5.
            await _chatPush.BroadcastMessageAsync(session.ConversationId, messageDto, ct);

            _logger.LogInformation("[CallV2] Đã tạo và phát nhật ký cuộc gọi realtime cho phiên {SessionId}. Thời lượng: {Duration}s", session.Id, durationSeconds);
        }
        catch (Exception ex)
        {
            // Chỉ log cảnh báo, không ném lỗi ra ngoài để đảm bảo phiên gọi vẫn được đánh dấu kết thúc trong DB.
            _logger.LogWarning(ex, "[CallV2] Không thể tạo tin nhắn nhật ký cho phiên gọi {SessionId}", session.Id);
        }
    }

    private static bool IsAbnormalEnd(string status, string reason)
    {
        if (string.Equals(status, CallSessionV2Statuses.Failed, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return string.Equals(reason, "normal", StringComparison.OrdinalIgnoreCase) == false;
    }
}
