using FluentAssertions;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.UnitTests.Services;

public sealed class ReadinessServiceAiProviderTests
{
    [Fact]
    public void ReadinessService_ShouldReportAiProviderUnavailable_WhenApiKeyMissing()
    {
        var (ready, message) = ReadinessService.CheckAiProviderConfiguration(new AiProviderOptions
        {
            ApiKey = "",
            Model = "gpt-test"
        });

        ready.Should().BeFalse();
        message.Should().Be("AI provider configuration is incomplete: ApiKey");
    }

    [Fact]
    public void ReadinessService_ShouldReportAiProviderUnavailable_WhenModelMissing()
    {
        var (ready, message) = ReadinessService.CheckAiProviderConfiguration(new AiProviderOptions
        {
            ApiKey = "secret-api-key",
            Model = ""
        });

        ready.Should().BeFalse();
        message.Should().Be("AI provider configuration is incomplete: Model");
    }

    [Fact]
    public void ReadinessService_ShouldNotIncludeSecretValues_InAiReadinessFailure()
    {
        const string secretValue = "secret-api-key";
        var (ready, message) = ReadinessService.CheckAiProviderConfiguration(new AiProviderOptions
        {
            ApiKey = secretValue,
            Model = ""
        });

        ready.Should().BeFalse();
        message.Should().Contain("Model");
        message.Should().NotContain(secretValue);
    }
}
