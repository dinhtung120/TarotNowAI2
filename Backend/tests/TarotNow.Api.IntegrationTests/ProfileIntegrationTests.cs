/*
 * FILE: ProfileIntegrationTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra luồng cập nhật + lấy profile end-to-end.
 *
 *   QUY TẮC KINH DOANH:
 *   → Khi User cập nhật DateOfBirth → hệ thống tự tính:
 *     - Zodiac (cung hoàng đạo): dựa trên tháng + ngày sinh
 *     - Numerology (thần số học): cộng tất cả chữ số ngày sinh → rút gọn về 1 chữ số
 *   → Ví dụ: 15/07/1995 → Cancer (Cự Giải), Numerology = 1+5+0+7+1+9+9+5 = 37 → 3+7 = 10 → 1+0 = 1
 *
 *   TEST CASE:
 *   UpdateProfile_ShouldComputeZodiacAndNumerology_Correctly:
 *   1. Seed User
 *   2. PATCH /api/v1/profile: cập nhật DisplayName + DateOfBirth + AvatarUrl
 *   3. GET /api/v1/profile: verify Zodiac="Cancer (Cự Giải)", Numerology=1
 *
 *   KIỂM TRA: domain logic tính Zodiac + Numerology là đúng.
 */

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

/// <summary>
/// Test profile: update → auto-compute Zodiac + Numerology → verify.
/// </summary>
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

    /// <summary>
    /// Cập nhật profile → hệ thống tự tính Zodiac + Numerology.
    /// Input: DateOfBirth = 15/07/1995
    /// Expected: Zodiac = "Cancer (Cự Giải)", Numerology = 1
    /// </summary>
    [Fact]
    public async Task UpdateProfile_ShouldComputeZodiacAndNumerology_Correctly()
    {
        // Seed User
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

        // Cập nhật profile: ngày sinh 15/07/1995 → Cancer, Numerology = 1
        var updateRequest = new UpdateProfileRequest
        {
            DisplayName = "Test Name",
            DateOfBirth = new DateTime(1995, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            AvatarUrl = "http://example.com/avatar.png"
        };

        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/profile", updateRequest);
        patchResponse.EnsureSuccessStatusCode();

        // GET profile → verify computed fields
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();

        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal("Test Name", profile!.DisplayName);
        Assert.Equal("http://example.com/avatar.png", profile.AvatarUrl);
        
        // Zodiac: July 15 → Cancer (Cự Giải)
        Assert.Equal("Cancer (Cự Giải)", profile.Zodiac);

        // Numerology: 1+5+0+7+1+9+9+5 = 37 → 3+7 = 10 → 1+0 = 1
        Assert.Equal(1, profile.Numerology);
    }
}
