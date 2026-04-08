using TarotNow.Infrastructure.Services.CallV2;

namespace TarotNow.Api.IntegrationTests;

internal sealed class FakeLiveKitRoomGateway : ILiveKitRoomGateway
{
    public List<string> CreatedRooms { get; } = [];
    public List<string> DeletedRooms { get; } = [];

    public Task<bool> CreateRoomAsync(string roomName, CancellationToken ct = default)
    {
        CreatedRooms.Add(roomName);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteRoomAsync(string roomName, CancellationToken ct = default)
    {
        DeletedRooms.Add(roomName);
        return Task.FromResult(true);
    }
}
