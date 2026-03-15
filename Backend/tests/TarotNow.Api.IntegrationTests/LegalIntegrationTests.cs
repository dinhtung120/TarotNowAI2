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

    [Fact]
    public async Task Consent_ShouldRecord_And_CheckStatus()
    {
        // 0. Seed User
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
            
            // Set Id via reflection since it's private set
            typeof(TarotNow.Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);
            
            user.Activate(); // Set status Active

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // 1. Check initial status (might be false or true depending on DB state, so let's record directly)
        // 2. Act: Record Consent for all required documents
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "TOS", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "PrivacyPolicy", Version = "1.0" });
        await _client.PostAsJsonAsync("/api/v1/legal/consent", new { DocumentType = "AiDisclaimer", Version = "1.0" });

        // 3. Act: Check Consent Status
        var getResponse = await _client.GetAsync("/api/v1/legal/consent-status");
        getResponse.EnsureSuccessStatusCode();

        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("\"isFullyConsented\":true", content); 
        Assert.Contains("[]", content); // PendingDocuments should be empty

        // 4. Act: Check Consent for a newer version (simulate version change requirement)
        var getNewVersionResponse = await _client.GetAsync("/api/v1/legal/consent-status?documentType=TOS&version=v2.0");
        getNewVersionResponse.EnsureSuccessStatusCode();

        var newContent = await getNewVersionResponse.Content.ReadAsStringAsync();
        Assert.Contains("false", newContent); // HasConsented should be false for a new un-consented version
    }
}
