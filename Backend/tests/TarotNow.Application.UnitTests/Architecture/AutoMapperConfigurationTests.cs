using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TarotNow.Application;

namespace TarotNow.Application.UnitTests.Architecture;

public sealed class AutoMapperConfigurationTests
{
    [Fact]
    public void AutoMapperProfiles_ShouldBeValid()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
        services.AddApplicationServices();
        using var provider = services.BuildServiceProvider();

        var mapper = provider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
