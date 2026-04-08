using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed class LiveKitRoomGateway : ILiveKitRoomGateway
{
    private readonly HttpClient _httpClient;
    private readonly LiveKitOptions _options;
    private readonly LiveKitTokenFactory _tokenFactory;

    public LiveKitRoomGateway(
        HttpClient httpClient,
        IOptions<LiveKitOptions> options,
        LiveKitTokenFactory tokenFactory)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _tokenFactory = tokenFactory;
    }

    public async Task<bool> CreateRoomAsync(string roomName, CancellationToken ct = default)
    {
        var payload = new { name = roomName, empty_timeout = 30 };
        var endpoint = BuildEndpoint("CreateRoom");
        return await PostAsync(endpoint, payload, roomName, ct);
    }

    public async Task<bool> DeleteRoomAsync(string roomName, CancellationToken ct = default)
    {
        var payload = new { room = roomName };
        var endpoint = BuildEndpoint("DeleteRoom");
        return await PostAsync(endpoint, payload, roomName, ct);
    }

    private async Task<bool> PostAsync(string endpoint, object payload, string roomName, CancellationToken ct)
    {
        var token = _tokenFactory.CreateRoomAdminToken(roomName);
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(payload),
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await _httpClient.SendAsync(request, ct);
        return response.IsSuccessStatusCode;
    }

    private string BuildEndpoint(string action)
    {
        var baseUrl = _options.Url.Trim().TrimEnd('/');
        return $"{baseUrl}/twirp/livekit.RoomService/{action}";
    }
}
