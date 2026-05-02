

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Api.Contracts;
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

        // Chuẩn bị avatar hiện tại để xác nhận PATCH profile không được ghi đè avatar trực tiếp.
        var existingUser = await db.Users.FirstAsync(u => u.Id == userId);
        existingUser.ApplyManagedAvatar("https://media.example.com/avatars/original.webp", "avatars/original.webp");
        await db.SaveChangesAsync();

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
        Assert.Equal("https://media.example.com/avatars/original.webp", profile.AvatarUrl);

        // DOB 15/07 kỳ vọng thuộc cung Cự Giải theo rule hiện tại.
        Assert.Equal("Cancer (Cự Giải)", profile.Zodiac);

        // Kiểm tra numerology số 1 theo dữ liệu ngày sinh test.
        Assert.Equal(1, profile.Numerology);
    }

    [Fact]
    public async Task UpdateProfile_ShouldAcceptDateOnlyAndPayoutBank_ForReader()
    {
        await EnsureTestUserAsync();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var user = await db.Users.FirstAsync(u => u.Id == userId);
            user.ApproveAsReader();
            await db.SaveChangesAsync();
        }

        var payload = new
        {
            displayName = "Reader Profile",
            dateOfBirth = "1994-11-03",
            payoutBankBin = "970436",
            payoutBankName = "Vietcombank",
            payoutBankAccountNumber = "1019030535",
            payoutBankAccountHolder = "DINH SON TUNG"
        };

        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/profile", payload);
        patchResponse.EnsureSuccessStatusCode();

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var verifyUser = await verifyDb.Users.FirstAsync(u => u.Id == Guid.Parse("00000000-0000-0000-0000-000000000001"));

        Assert.Equal("Reader Profile", verifyUser.DisplayName);
        Assert.Equal("970436", verifyUser.PayoutBankBin);
        Assert.Equal("Vietcombank", verifyUser.PayoutBankName);
        Assert.Equal("1019030535", verifyUser.PayoutBankAccountNumber);
        Assert.Equal("DINH SON TUNG", verifyUser.PayoutBankAccountHolder);
    }

    /// <summary>
    /// Xác nhận upload avatar theo luồng mới presign + confirm và phản ánh lại trong profile.
    /// </summary>
    [Fact]
    public async Task UploadAvatar_ShouldPresignThenConfirmAndUpdateProfile()
    {
        await EnsureTestUserAsync();

        var presignResponse = await _client.PostAsJsonAsync("/api/v1/profile/avatar/presign", new
        {
            contentType = "image/webp",
            sizeBytes = 120_000L,
        });
        presignResponse.EnsureSuccessStatusCode();
        var presignPayload = await presignResponse.Content.ReadFromJsonAsync<PresignedUploadResponsePayload>();

        Assert.NotNull(presignPayload);
        Assert.StartsWith("avatars/", presignPayload!.ObjectKey);
        Assert.EndsWith("/" + presignPayload.ObjectKey, presignPayload.PublicUrl);
        Assert.StartsWith("http", presignPayload.PublicUrl);
        Assert.False(string.IsNullOrWhiteSpace(presignPayload.UploadUrl));
        Assert.False(string.IsNullOrWhiteSpace(presignPayload.UploadToken));

        var confirmResponse = await _client.PostAsJsonAsync("/api/v1/profile/avatar/confirm", new
        {
            objectKey = presignPayload.ObjectKey,
            publicUrl = presignPayload.PublicUrl,
            uploadToken = presignPayload.UploadToken,
        });
        confirmResponse.EnsureSuccessStatusCode();
        var confirmPayload = await confirmResponse.Content.ReadFromJsonAsync<AvatarConfirmResponsePayload>();

        Assert.NotNull(confirmPayload);
        Assert.True(confirmPayload!.Success);
        Assert.Equal(presignPayload.PublicUrl, confirmPayload.AvatarUrl);
        Assert.Equal(presignPayload.ObjectKey, confirmPayload.ObjectKey);

        // Đọc profile và xác nhận avatar đã được cập nhật đúng URL mới.
        var getResponse = await _client.GetAsync("/api/v1/profile");
        getResponse.EnsureSuccessStatusCode();
        var profile = await getResponse.Content.ReadFromJsonAsync<Application.Features.Profile.Queries.GetProfile.ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal(presignPayload.PublicUrl, profile!.AvatarUrl);
    }

    private async Task EnsureTestUserAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (await db.Users.AnyAsync(x => x.Id == userId))
        {
            return;
        }

        var user = new TarotNow.Domain.Entities.User(
            email: "profile@tarotnow.com",
            username: "profileuser",
            passwordHash: "hash",
            displayName: "Seed User",
            dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(TarotNow.Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);
        user.Activate();
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    private sealed class PresignedUploadResponsePayload
    {
        public string UploadUrl { get; set; } = string.Empty;
        public string ObjectKey { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public string UploadToken { get; set; } = string.Empty;
    }

    private sealed class AvatarConfirmResponsePayload
    {
        public bool Success { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string ObjectKey { get; set; } = string.Empty;
    }
}
