

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
// Kiểm thử endpoint hồ sơ: cập nhật thông tin và upload avatar.
public class ProfileIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Factory tích hợp dùng cho dữ liệu và DI scope.
    private readonly CustomWebApplicationFactory<Program> _factory;
    // HTTP client đã cấu hình auth test scheme.
    private readonly HttpClient _client;

    /// <summary>
    /// Khởi tạo test class profile với client có sẵn xác thực.
    /// Luồng này giúp các test tập trung vào nghiệp vụ thay vì setup auth lặp lại.
    /// </summary>
    public ProfileIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    /// <summary>
    /// Xác nhận cập nhật profile tính đúng Zodiac và Numerology.
    /// Luồng seed user, gọi PATCH profile, rồi GET profile để kiểm tra các trường tính toán.
    /// </summary>
    [Fact]
    public async Task UpdateProfile_ShouldComputeZodiacAndNumerology_Correctly()
    {
        // Seed user nếu chưa có để test luôn chạy độc lập.
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

        // Chuẩn bị payload cập nhật có DOB để hệ thống tính Zodiac/Numerology.
        var updateRequest = new UpdateProfileRequest
        {
            DisplayName = "Test Name",
            DateOfBirth = new DateTime(1995, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            AvatarUrl = "http://example.com/avatar.png"
        };

        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/profile", updateRequest);
        patchResponse.EnsureSuccessStatusCode();

        // Đọc lại profile để xác minh các trường đã lưu và trường tính toán.
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();

        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal("Test Name", profile!.DisplayName);
        Assert.Equal("http://example.com/avatar.png", profile.AvatarUrl);

        // DOB 15/07 kỳ vọng thuộc cung Cự Giải theo rule hiện tại.
        Assert.Equal("Cancer (Cự Giải)", profile.Zodiac);

        // Kiểm tra numerology số 1 theo dữ liệu ngày sinh test.
        Assert.Equal(1, profile.Numerology);
    }

    /// <summary>
    /// Xác nhận upload avatar được nén/lưu và phản ánh lại trong profile.
    /// Luồng upload ảnh giả lập, đọc URL trả về, rồi GET profile để đối chiếu.
    /// </summary>
    [Fact]
    public async Task UploadAvatar_ShouldCompressAndSaveFile()
    {
        // Dùng ảnh mẫu cực nhỏ dạng base64 để test upload nhanh và ổn định.
        var imageBytes = Convert.FromBase64String("/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAP//////////////////////////////////////////////////////////////////////////////////////wgALCAABAAEBAREA/8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPxA=");
        using var stream = new MemoryStream(imageBytes);
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(stream);
        streamContent.Headers.Add("Content-Type", "image/jpeg");
        content.Add(streamContent, "file", "test.jpg");

        // Gọi endpoint upload avatar.
        var response = await _client.PostAsync("/api/v1/profile/avatar", content);
        response.EnsureSuccessStatusCode();

        // Kiểm tra payload response có thông tin upload thành công.
        var resultText = await response.Content.ReadAsStringAsync();
        Assert.Contains("success\":true", resultText);
        // Với R2, avatarUrl sẽ là URL đầy đủ bắt đầu bằng http, publicId khớp với key prefix avatars/
        Assert.Contains("avatarUrl\":\"http", resultText);
        Assert.Contains("publicId\":\"avatars/", resultText);

        // Parse avatarUrl để đối chiếu với dữ liệu profile sau khi lưu.
        using var jsonDoc = System.Text.Json.JsonDocument.Parse(resultText);
        var avatarUrl = jsonDoc.RootElement.GetProperty("avatarUrl").GetString();

        // Đọc profile và xác nhận avatar đã được cập nhật đúng URL mới.
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();
        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal(avatarUrl, profile!.AvatarUrl);
    }
}
