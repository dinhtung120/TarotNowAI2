

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Api.Contracts;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public class ProfileIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ProfileIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

        [Fact]
    public async Task UpdateProfile_ShouldComputeZodiacAndNumerology_Correctly()
    {
        
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new TarotNow.Domain.Entities.User(
                email: "profile@tarotnow.com",
                username: "profileuser",
                passwordHash: "hash",
                displayName: "Old Name",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(TarotNow.Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        
        var updateRequest = new UpdateProfileRequest
        {
            DisplayName = "Test Name",
            DateOfBirth = new DateTime(1995, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            AvatarUrl = "http://example.com/avatar.png"
        };

        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/profile", updateRequest);
        patchResponse.EnsureSuccessStatusCode();

        
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();

        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal("Test Name", profile!.DisplayName);
        Assert.Equal("http://example.com/avatar.png", profile.AvatarUrl);
        
        
        Assert.Equal("Cancer (Cự Giải)", profile.Zodiac);

        
        Assert.Equal(1, profile.Numerology);
    }

        [Fact]
    public async Task UploadAvatar_ShouldCompressAndSaveFile()
    {
        
        var imageBytes = Convert.FromBase64String("/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAP//////////////////////////////////////////////////////////////////////////////////////wgALCAABAAEBAREA/8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPxA=");
        using var stream = new MemoryStream(imageBytes);
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(stream);
        streamContent.Headers.Add("Content-Type", "image/jpeg");
        content.Add(streamContent, "file", "test.jpg");

        
        var response = await _client.PostAsync("/api/v1/profile/avatar", content);
        response.EnsureSuccessStatusCode();

        
        var resultText = await response.Content.ReadAsStringAsync();
        Assert.Contains("success\":true", resultText);
        Assert.Contains("avatarUrl\":\"/uploads/avatars/", resultText);

        
        using var jsonDoc = System.Text.Json.JsonDocument.Parse(resultText);
        var avatarUrl = jsonDoc.RootElement.GetProperty("avatarUrl").GetString();

        
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();
        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal(avatarUrl, profile!.AvatarUrl);
    }
}
