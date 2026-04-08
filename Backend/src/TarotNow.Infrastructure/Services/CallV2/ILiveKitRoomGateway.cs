namespace TarotNow.Infrastructure.Services.CallV2;

internal interface ILiveKitRoomGateway
{
    Task<bool> CreateRoomAsync(string roomName, CancellationToken ct = default);

    Task<bool> DeleteRoomAsync(string roomName, CancellationToken ct = default);
}
