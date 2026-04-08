

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Api.Controllers;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
// Kiểm thử luồng ghi nhận và truy vấn trạng thái consent pháp lý.
public class LegalIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Factory tích hợp dùng để truy cập service scope và tạo HTTP client.
    private readonly CustomWebApplicationFactory<Program> _factory;
    // Client test đã gắn sẵn auth scheme giả lập.
    private readonly HttpClient _client;

    /// <summary>
    /// Khởi tạo test class legal và cấu hình auth mặc định.
    /// Luồng này giảm lặp khi mọi test đều cần header xác thực.
    /// </summary>
    public LegalIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    /// <summary>
    /// Xác nhận consent được ghi nhận đầy đủ và trạng thái phản hồi đúng theo version.
    /// Luồng gửi 3 consent bắt buộc, kiểm tra fully-consented, rồi kiểm tra version mới phải chưa consent.
    /// </summary>
    [Fact]
    public async Task Consent_ShouldRecord_And_CheckStatus()
    {
        // Seed user nếu chưa tồn tại để luồng consent có đối tượng hợp lệ.
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new TarotNow.Domain.Entities.User(
                email: "legal@tarotnow.com",
                username: "legaluser",
                passwordHash: "hash",
                displayName: "Legal Test",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(TarotNow.Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Gửi đủ 3 loại document theo policy consent hiện tại.
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "TOS", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "PrivacyPolicy", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "AiDisclaimer", Version = "1.0" });

        // Kiểm tra trạng thái sau khi đã consent đầy đủ.
        var getResponse = await _client.GetAsync("/api/v1/legal/consent-status");
        getResponse.EnsureSuccessStatusCode();

        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("\"isFullyConsented\":true", content);
        Assert.Contains("[]", content);

        // Kiểm tra version mới của TOS phải yêu cầu consent lại.
        var getNewVersionResponse = await _client.GetAsync("/api/v1/legal/consent-status?documentType=TOS&version=v2.0");
        getNewVersionResponse.EnsureSuccessStatusCode();

        var newContent = await getNewVersionResponse.Content.ReadAsStringAsync();
        Assert.Contains("false", newContent);
    }
}
