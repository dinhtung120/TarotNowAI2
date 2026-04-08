

using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử phân quyền RBAC cho nhóm endpoint admin.
public class AdminRbacIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Factory dùng để tạo HttpClient tích hợp với môi trường test container.
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class RBAC với factory tích hợp.
    /// Luồng dùng chung factory giúp các test tái sử dụng cùng cấu hình hạ tầng.
    /// </summary>
    public AdminRbacIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Xác nhận user thường bị từ chối khi gọi endpoint admin.
    /// Luồng gắn role User vào header giả lập rồi kỳ vọng HTTP 403.
    /// </summary>
    [Fact]
    public async Task AdminRoute_ShouldReject_WhenUserIsNotAdmin()
    {
        var client = _factory.CreateClient();

        // Gắn auth test + role User để mô phỏng tài khoản không có quyền admin.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        var response = await client.GetAsync("/api/v1/admin/users");

        // Kỳ vọng middleware authorization chặn truy cập.
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    /// <summary>
    /// Xác nhận admin hợp lệ truy cập được endpoint admin.
    /// Luồng gắn role admin và kỳ vọng HTTP 200.
    /// </summary>
    [Fact]
    public async Task AdminRoute_ShouldSucceed_WhenUserIsAdmin()
    {
        var client = _factory.CreateClient();

        // Gắn auth test + role admin để mô phỏng đúng quyền truy cập.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "admin");

        var response = await client.GetAsync("/api/v1/admin/users");

        // Kỳ vọng endpoint trả thành công khi quyền phù hợp.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
