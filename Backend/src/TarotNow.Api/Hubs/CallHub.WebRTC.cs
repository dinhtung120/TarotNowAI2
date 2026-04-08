namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    public async Task SendOffer(string conversationId, object sdpOffer)
    {
        await RejectLegacyWebRtcAsync(nameof(SendOffer), conversationId);
    }

    public async Task SendAnswer(string conversationId, object sdpAnswer)
    {
        await RejectLegacyWebRtcAsync(nameof(SendAnswer), conversationId);
    }

    public async Task SendIceCandidate(string conversationId, object candidate)
    {
        await RejectLegacyWebRtcAsync(nameof(SendIceCandidate), conversationId);
    }

    private async Task RejectLegacyWebRtcAsync(string methodName, string conversationId)
    {
        _logger.LogWarning("Legacy WebRTC relay blocked: {Method} conversation={ConversationId}", methodName, conversationId);
        await SendClientErrorAsync("legacy_call_disabled", "Kênh WebRTC cũ đã bị tắt. Vui lòng cập nhật ứng dụng.");
    }
}
