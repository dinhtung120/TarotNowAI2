/*
 * FILE: AdminRbacIntegrationTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra RBAC (Role-Based Access Control) cho Admin endpoints.
 *   Đảm bảo chỉ User có role "admin" mới truy cập được các API quản trị.
 *
 *   CÁC TEST CASE:
 *   1. AdminRoute_ShouldReject_WhenUserIsNotAdmin:
 *      → User thường (role = "User") gọi /api/v1/admin/users → 403 Forbidden
 *   2. AdminRoute_ShouldSucceed_WhenUserIsAdmin:
 *      → Admin (role = "admin") gọi /api/v1/admin/users → 200 OK
 *
 *   CƠ CHẾ TEST:
 *   → Dùng TestAuthHandler (mock authentication) thay vì JWT thật.
 *   → Header X-Test-Role: truyền role cho TestAuthHandler → không cần login thật.
 *   → IClassFixture<CustomWebApplicationFactory>: chia sẻ Docker containers giữa các test.
 *   → [Collection("Testcontainers")]: đảm bảo collection tests chạy tuần tự (tránh port conflict).
 */

using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Test RBAC: Admin-only endpoints phải từ chối User thường và cho phép Admin.
/// </summary>
[Collection("Testcontainers")]
public class AdminRbacIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AdminRbacIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// User thường (role = "User") gọi Admin API → phải bị từ chối (403 Forbidden).
    /// Kiểm tra policy-based authorization hoạt động đúng.
    /// </summary>
    [Fact]
    public async Task AdminRoute_ShouldReject_WhenUserIsNotAdmin()
    {
        var client = _factory.CreateClient();

        // Setup mock auth: role = "User" (không phải admin)
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        var response = await client.GetAsync("/api/v1/admin/users");

        // Kỳ vọng: 403 Forbidden (có auth nhưng không đủ quyền)
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    /// <summary>
    /// Admin (role = "admin") gọi Admin API → phải thành công (200 OK).
    /// </summary>
    [Fact]
    public async Task AdminRoute_ShouldSucceed_WhenUserIsAdmin()
    {
        var client = _factory.CreateClient();

        // Setup mock auth: role = "admin"
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "admin");

        var response = await client.GetAsync("/api/v1/admin/users");

        // Kỳ vọng: 200 OK (Admin có đủ quyền)
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
