using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Services;
using TarotNow.Application.Exceptions;

namespace TarotNow.Api.IntegrationTests;

public sealed class MiddlewareSecurityBehaviorTests
{
    [Fact]
    public void ControllerProblemDetailsExtensions_ShouldReturnCanonicalErrorCodeShape()
    {
        var controller = new TestController();

        var result = Assert.IsType<ObjectResult>(controller.ApiProblem(
            StatusCodes.Status400BadRequest,
            "TEST_ERROR",
            "Test detail."));
        var problem = Assert.IsType<ProblemDetails>(result.Value);

        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
        Assert.Equal("Bad Request", problem.Title);
        Assert.Equal("TEST_ERROR", problem.Extensions["errorCode"]);
        Assert.Equal("Test detail.", problem.Detail);
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldReturnProblemDetails_ForValidationException()
    {
        var context = CreateHttpContext();
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            context,
            new ValidationException(new Dictionary<string, string[]> { ["Name"] = ["Required"] }),
            CancellationToken.None);

        var problem = await ReadProblemDetailsAsync(context);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.StartsWith("application/", context.Response.ContentType, StringComparison.Ordinal);
        Assert.Equal("Validation Failed", problem.RootElement.GetProperty("title").GetString());
        Assert.True(problem.RootElement.TryGetProperty("errorCode", out _));
        Assert.True(problem.RootElement.TryGetProperty("errors", out _));
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldReturnProblemDetails_ForUnauthorizedAccessException()
    {
        var context = CreateHttpContext();
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            context,
            new UnauthorizedAccessException("Invalid authenticated user context."),
            CancellationToken.None);

        var problem = await ReadProblemDetailsAsync(context);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.StartsWith("application/", context.Response.ContentType, StringComparison.Ordinal);
        Assert.Equal("Unauthorized", problem.RootElement.GetProperty("title").GetString());
        Assert.True(problem.RootElement.TryGetProperty("errorCode", out _));
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldReturnProblemDetails_ForBadRequestException()
    {
        var context = CreateHttpContext();
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            context,
            new BadRequestException("Invalid request."),
            CancellationToken.None);

        var problem = await ReadProblemDetailsAsync(context);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.StartsWith("application/", context.Response.ContentType, StringComparison.Ordinal);
        Assert.Equal("Bad Request", problem.RootElement.GetProperty("title").GetString());
        Assert.True(problem.RootElement.TryGetProperty("errorCode", out _));
    }

    [Fact]
    public async Task CorrelationIdMiddleware_ShouldPropagateValidCorrelationId()
    {
        const string correlationId = "trace-123";
        var context = CreateHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = correlationId;
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, NullLogger<CorrelationIdMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.Equal(correlationId, context.TraceIdentifier);
        Assert.Equal(correlationId, context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString());
    }

    [Fact]
    public async Task CorrelationIdMiddleware_ShouldSanitizeOversizedCorrelationId()
    {
        var context = CreateHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = new string('a', 65);
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, NullLogger<CorrelationIdMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.NotEqual(new string('a', 65), context.TraceIdentifier);
        Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString()));
    }

    [Fact]
    public async Task CorrelationIdMiddleware_ShouldSanitizeInvalidCorrelationId()
    {
        var context = CreateHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "bad id with spaces";
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, NullLogger<CorrelationIdMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.NotEqual("bad id with spaces", context.TraceIdentifier);
        Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString()));
    }

    [Fact]
    public async Task CorrelationIdMiddleware_ShouldCreateCorrelationId_WhenMissing()
    {
        var context = CreateHttpContext();
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, NullLogger<CorrelationIdMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.False(string.IsNullOrWhiteSpace(context.TraceIdentifier));
        Assert.Equal(context.TraceIdentifier, context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString());
    }

    [Fact]
    public async Task ChatFeatureGateMiddleware_ShouldAllowRequest_WhenFeatureEnabled()
    {
        var context = CreateHttpContext("/api/v1/conversations");
        var called = false;
        var middleware = new ChatFeatureGateMiddleware(_ =>
        {
            called = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(context, new StubFeatureManagerSnapshot(true));

        Assert.True(called);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task ChatFeatureGateMiddleware_ShouldReturnNotFound_WhenFeatureDisabled()
    {
        var context = CreateHttpContext("/api/v1/conversations");
        var middleware = new ChatFeatureGateMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(context, new StubFeatureManagerSnapshot(false));

        var problem = await ReadProblemDetailsAsync(context);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Equal("CHAT_V2_DISABLED", problem.RootElement.GetProperty("errorCode").GetString());
    }

    [Fact]
    public async Task ChatFeatureGateMiddleware_ShouldIgnoreNonChatRoutes()
    {
        var context = CreateHttpContext("/api/v1/profile");
        var called = false;
        var middleware = new ChatFeatureGateMiddleware(_ =>
        {
            called = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(context, new StubFeatureManagerSnapshot(false));

        Assert.True(called);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public void AuthCookieService_ShouldApplyForwardedHeaders_FromTrustedProxy()
    {
        var context = CreateHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.10");
        context.Request.Scheme = "http";
        context.Request.Host = new HostString("api.internal");
        context.Request.Headers["x-forwarded-proto"] = "https";
        context.Request.Headers["x-forwarded-host"] = "example.com";
        var service = CreateAuthCookieService(cookieDomain: "example.com");

        service.SetAccessToken(context.Request, context.Response, "access-token", 60);

        var setCookie = context.Response.Headers.SetCookie.ToString();
        Assert.Contains("secure", setCookie, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("domain=example.com", setCookie, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AuthCookieService_ShouldIgnoreForwardedHeaders_FromUntrustedRemote()
    {
        var context = CreateHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Parse("203.0.113.10");
        context.Request.Scheme = "http";
        context.Request.Host = new HostString("api.internal");
        context.Request.Headers["x-forwarded-proto"] = "https";
        context.Request.Headers["x-forwarded-host"] = "example.com";
        var service = CreateAuthCookieService(cookieDomain: "example.com");

        service.SetAccessToken(context.Request, context.Response, "access-token", 60);

        var setCookie = context.Response.Headers.SetCookie.ToString();
        Assert.DoesNotContain("secure", setCookie, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("domain=example.com", setCookie, StringComparison.OrdinalIgnoreCase);
    }

    private static DefaultHttpContext CreateHttpContext(string path = "/")
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<JsonDocument> ReadProblemDetailsAsync(HttpContext context)
    {
        context.Response.Body.Position = 0;
        return await JsonDocument.ParseAsync(context.Response.Body);
    }

    private static AuthCookieService CreateAuthCookieService(string? cookieDomain = null)
    {
        var configurationValues = new Dictionary<string, string?>
        {
            ["Auth:CookieDomain"] = cookieDomain,
            ["Auth:CookieSecure"] = null,
            ["ForwardedHeaders:Enabled"] = "true"
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();

        var services = new ServiceCollection();
        services.Configure<Microsoft.AspNetCore.Builder.ForwardedHeadersOptions>(options =>
        {
            options.KnownProxies.Add(IPAddress.Parse("10.0.0.10"));
        });
        var serviceProvider = services.BuildServiceProvider();
        var trustEvaluator = new ForwardedHeaderTrustEvaluator(
            configuration,
            serviceProvider.GetRequiredService<IOptions<Microsoft.AspNetCore.Builder.ForwardedHeadersOptions>>());

        return new AuthCookieService(configuration, new StubHostEnvironment(), trustEvaluator);
    }

    private sealed class TestController : ControllerBase
    {
    }

    private sealed class StubFeatureManagerSnapshot : IFeatureManagerSnapshot
    {
        private readonly bool _enabled;

        public StubFeatureManagerSnapshot(bool enabled)
        {
            _enabled = enabled;
        }

        public IAsyncEnumerable<string> GetFeatureNamesAsync()
        {
            return AsyncEnumerable.Empty<string>();
        }

        public Task<bool> IsEnabledAsync(string feature)
        {
            Assert.Equal(FeatureFlags.ChatV2Enabled, feature);
            return Task.FromResult(_enabled);
        }

        public Task<bool> IsEnabledAsync<TContext>(string feature, TContext context)
        {
            Assert.Equal(FeatureFlags.ChatV2Enabled, feature);
            return Task.FromResult(_enabled);
        }
    }

    private sealed class StubHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "TarotNow.Api.IntegrationTests";
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
