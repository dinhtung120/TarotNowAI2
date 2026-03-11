using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Api.Controllers;
using TarotNow.Domain.Interfaces;
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
        // 0. Seed User
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (TarotNow.Domain.Entities.User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(TarotNow.Domain.Entities.User));
            typeof(TarotNow.Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);
            typeof(TarotNow.Domain.Entities.User).GetProperty("Email")?.SetValue(user, "profile@tarotnow.com");
            typeof(TarotNow.Domain.Entities.User).GetProperty("Username")?.SetValue(user, "profileuser");
            typeof(TarotNow.Domain.Entities.User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(TarotNow.Domain.Entities.User).GetProperty("DisplayName")?.SetValue(user, "Old Name");
            typeof(TarotNow.Domain.Entities.User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(TarotNow.Domain.Entities.User).GetProperty("Status")?.SetValue(user, "Active");
            typeof(TarotNow.Domain.Entities.User).GetProperty("Role")?.SetValue(user, "User");
            typeof(TarotNow.Domain.Entities.User).GetProperty("ReaderStatus")?.SetValue(user, "Pending");
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // 1. Arrange: Update DisplayName and DateOfBirth (e.g., July 15, 1995 -> Cancer, Numerology number)
        var updateRequest = new UpdateProfileRequest
        {
            DisplayName = "Test Name",
            DateOfBirth = new DateTime(1995, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            AvatarUrl = "http://example.com/avatar.png"
        };

        // 2. Act
        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/profile", updateRequest);
        
        // 3. Assert Output is OK
        patchResponse.EnsureSuccessStatusCode();

        // 4. Act: Get Profile to verify recalculation
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();

        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal("Test Name", profile!.DisplayName);
        Assert.Equal("http://example.com/avatar.png", profile.AvatarUrl);
        
        // Cancer is July 15
        Assert.Equal("Cancer (Cự Giải)", profile.Zodiac);

        // Numerology: 1+5+0+7+1+9+9+5 = 37 -> 3+7 = 10 -> 1+0 = 1
        Assert.Equal(1, profile.Numerology);
    }
}
