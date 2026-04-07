

using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
public class AdminRbacIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AdminRbacIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

        [Fact]
    public async Task AdminRoute_ShouldReject_WhenUserIsNotAdmin()
    {
        var client = _factory.CreateClient();

        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        var response = await client.GetAsync("/api/v1/admin/users");

        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

        [Fact]
    public async Task AdminRoute_ShouldSucceed_WhenUserIsAdmin()
    {
        var client = _factory.CreateClient();

        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "admin");

        var response = await client.GetAsync("/api/v1/admin/users");

        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
