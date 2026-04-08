namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    public async Task EndCall(string callSessionId, string reason = "normal")
    {
        _logger.LogWarning("Legacy call signaling blocked: {Method} callSessionId={CallSessionId} reason={Reason}", nameof(EndCall), callSessionId, reason);
        await SendClientErrorAsync("legacy_call_disabled", "Phiên bản gọi cũ đã bị vô hiệu hóa. Vui lòng cập nhật ứng dụng.");
    }
}
