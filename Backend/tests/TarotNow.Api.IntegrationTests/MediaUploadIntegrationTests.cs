using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public class MediaUploadIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MediaUploadIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    [Fact]
    public async Task LegacyAvatarUploadEndpoint_ShouldReturnNotFound()
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent("legacy"), "file", "legacy.txt");

        var response = await _client.PostAsync("/api/v1/profile/avatar", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LegacyCommunityUploadEndpoint_ShouldReturnNotFound()
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent("legacy"), "file", "legacy.txt");

        var response = await _client.PostAsync("/api/v1/community/images", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CommunityImage_PresignThenConfirm_ShouldSucceed()
    {
        var presign = await _client.PostAsJsonAsync("/api/v1/community/image/presign", new
        {
            contextType = "post",
            contextDraftId = "draft-post-media-1",
            contentType = "image/webp",
            sizeBytes = 120_000L,
        });
        presign.EnsureSuccessStatusCode();

        var presignPayload = await presign.Content.ReadFromJsonAsync<PresignedUploadPayload>();
        Assert.NotNull(presignPayload);
        Assert.StartsWith("community/post/", presignPayload!.ObjectKey);

        var confirm = await _client.PostAsJsonAsync("/api/v1/community/image/confirm", new
        {
            contextType = "post",
            contextDraftId = "draft-post-media-1",
            objectKey = presignPayload.ObjectKey,
            publicUrl = presignPayload.PublicUrl,
            uploadToken = presignPayload.UploadToken,
        });
        confirm.EnsureSuccessStatusCode();

        var confirmPayload = await confirm.Content.ReadFromJsonAsync<CommunityConfirmPayload>();
        Assert.NotNull(confirmPayload);
        Assert.True(confirmPayload!.Success);
        Assert.Equal(presignPayload.ObjectKey, confirmPayload.ObjectKey);
        Assert.Equal(presignPayload.PublicUrl, confirmPayload.PublicUrl);
    }

    [Fact]
    public async Task ChatImage_PresignThenSendMessage_ShouldSucceed()
    {
        var conversationId = ObjectId.GenerateNewId().ToString();
        var senderId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        await EnsureConversationAsync(conversationId, senderId);

        var presign = await _client.PostAsJsonAsync($"/api/v1/conversations/{conversationId}/media/presign", new
        {
            mediaKind = "image",
            contentType = "image/webp",
            sizeBytes = 220_000L,
        });
        presign.EnsureSuccessStatusCode();

        var presignPayload = await presign.Content.ReadFromJsonAsync<PresignedUploadPayload>();
        Assert.NotNull(presignPayload);
        Assert.StartsWith($"chat/{conversationId}/images/", presignPayload!.ObjectKey);

        var sendMessage = await _client.PostAsJsonAsync($"/api/v1/conversations/{conversationId}/messages", new
        {
            type = "image",
            content = "",
            mediaPayload = new
            {
                url = presignPayload.PublicUrl,
                objectKey = presignPayload.ObjectKey,
                uploadToken = presignPayload.UploadToken,
                mimeType = "image/webp",
                sizeBytes = 220_000,
            },
        });
        sendMessage.EnsureSuccessStatusCode();

        var message = await sendMessage.Content.ReadFromJsonAsync<ChatMessageDto>();
        Assert.NotNull(message);
        Assert.Equal("image", message!.Type);
        Assert.Equal("[image]", message.Content);
        Assert.NotNull(message.MediaPayload);
        Assert.Equal(presignPayload.PublicUrl, message.MediaPayload!.Url);
        Assert.Equal(presignPayload.ObjectKey, message.MediaPayload.ObjectKey);
        Assert.Null(message.MediaPayload.UploadToken);
    }

    private async Task EnsureConversationAsync(string conversationId, Guid senderId)
    {
        using var scope = _factory.Services.CreateScope();
        var conversationRepository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();

        var existing = await conversationRepository.GetByIdAsync(conversationId, CancellationToken.None);
        if (existing is not null)
        {
            return;
        }

        await conversationRepository.AddAsync(new ConversationDto
        {
            Id = conversationId,
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        }, CancellationToken.None);
    }

    private sealed class PresignedUploadPayload
    {
        public string UploadUrl { get; set; } = string.Empty;
        public string ObjectKey { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public string UploadToken { get; set; } = string.Empty;
    }

    private sealed class CommunityConfirmPayload
    {
        public bool Success { get; set; }
        public string ObjectKey { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
    }
}
