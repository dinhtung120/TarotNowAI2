/*
 * FILE: LegalIntegrationTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra luồng pháp lý (consent/đồng thuận) end-to-end.
 *
 *   QUY TẮC KINH DOANH:
 *   → User phải đồng ý 3 loại tài liệu: TOS, PrivacyPolicy, AiDisclaimer
 *   → Mỗi loại có version → khi version mới release → User phải đồng ý lại
 *   → isFullyConsented=true chỉ khi TẤT CẢ document types đều đã consent
 *
 *   TEST CASE:
 *   Consent_ShouldRecord_And_CheckStatus:
 *   1. Seed User
 *   2. POST consent cho 3 document types (TOS, Privacy, AI)
 *   3. GET consent-status → isFullyConsented=true, pendingDocuments=[]
 *   4. GET consent-status?version=v2.0 → hasConsented=false (version mới chưa đồng ý)
 *
 *   KIỂM TRA: version tracking (v1.0 → v2.0 cần consent lại).
 */

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

/// <summary>
/// Test luồng pháp lý: consent tracking + version management.
/// </summary>
[Collection("IntegrationTests")]
public class LegalIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LegalIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    /// <summary>
    /// Luồng đầy đủ: consent 3 docs → check status → test version upgrade.
    /// Verify: isFullyConsented=true sau khi consent tất cả, false khi có version mới.
    /// </summary>
    [Fact]
    public async Task Consent_ShouldRecord_And_CheckStatus()
    {
        // Seed User
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

        // Consent cho cả 3 document types (v1.0)
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "TOS", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "PrivacyPolicy", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "AiDisclaimer", Version = "1.0" });

        // Kiểm tra: phải fully consented
        var getResponse = await _client.GetAsync("/api/v1/legal/consent-status");
        getResponse.EnsureSuccessStatusCode();

        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("\"isFullyConsented\":true", content); 
        Assert.Contains("[]", content); // Không còn document nào pending

        // Kiểm tra version mới: TOS v2.0 chưa consent → false
        var getNewVersionResponse = await _client.GetAsync("/api/v1/legal/consent-status?documentType=TOS&version=v2.0");
        getNewVersionResponse.EnsureSuccessStatusCode();

        var newContent = await getNewVersionResponse.Content.ReadAsStringAsync();
        Assert.Contains("false", newContent); // Chưa consent version mới
    }
}
