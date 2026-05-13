using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Services.Ai;

namespace TarotNow.Infrastructure.UnitTests.Services;

public sealed class OpenAiProviderTelemetryTests
{
    [Fact]
    public async Task OpenAiProviderTelemetry_ShouldNotFailProviderCall_WhenTelemetryWriteThrows()
    {
        var provider = CreateProvider(out _, out _);

        var exception = await Record.ExceptionAsync(() => provider.LogRequestAsync(CreateLogEntry()));

        Assert.Null(exception);
    }

    [Fact]
    public async Task OpenAiProviderTelemetry_ShouldEmitFailureSignal_WhenTelemetryWriteThrows()
    {
        var provider = CreateProvider(out _, out var logger);

        await provider.LogRequestAsync(CreateLogEntry());

        Assert.Contains(logger.Messages, message => message.Contains("ai.telemetry.write_failed", StringComparison.Ordinal));
    }

    [Fact]
    public async Task OpenAiProviderTelemetry_ShouldNotLogPromptOrSecret_WhenTelemetryWriteThrows()
    {
        const string secret = "secret-api-key";
        var provider = CreateProvider(out _, out var logger, secret);

        await provider.LogRequestAsync(CreateLogEntry());

        var combined = string.Join('\n', logger.Messages);
        Assert.DoesNotContain(secret, combined, StringComparison.Ordinal);
        Assert.DoesNotContain("sensitive prompt", combined, StringComparison.OrdinalIgnoreCase);
    }

    private static OpenAiProvider CreateProvider(
        out Mock<IAiProviderLogRepository> logRepository,
        out CapturingLogger<OpenAiProvider> logger,
        string apiKey = "test-api-key")
    {
        logRepository = new Mock<IAiProviderLogRepository>();
        logRepository
            .Setup(x => x.CreateAsync(It.IsAny<AiProviderLogCreateDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("telemetry store unavailable sensitive prompt"));

        var settings = new Mock<ISystemConfigSettings>();
        settings.SetupGet(x => x.OperationalAiTimeoutSeconds).Returns(30);
        settings.SetupGet(x => x.OperationalAiMaxRetries).Returns(2);
        settings.SetupGet(x => x.OperationalAiStreamingRetryBaseDelayMs).Returns(200);
        settings.SetupGet(x => x.OperationalAiStreamingTemperature).Returns(0.7);
        settings.SetupGet(x => x.OperationalAiPromptVersion).Returns("prompt-v1");

        logger = new CapturingLogger<OpenAiProvider>();
        return new OpenAiProvider(
            new HttpClient(),
            Microsoft.Extensions.Options.Options.Create(new AiProviderOptions
            {
                ApiKey = apiKey,
                BaseUrl = "https://example.test/v1/",
                Model = "gpt-test"
            }),
            settings.Object,
            logRepository.Object,
            logger);
    }

    private static AiProviderRequestLog CreateLogEntry()
    {
        return new AiProviderRequestLog
        {
            UserId = Guid.NewGuid(),
            RequestId = "request-1",
            Status = "failed",
            ErrorCode = "provider_error",
            PromptVersion = "prompt-v1"
        };
    }

    private sealed class CapturingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
