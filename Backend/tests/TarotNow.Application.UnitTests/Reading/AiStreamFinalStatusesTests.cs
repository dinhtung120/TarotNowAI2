using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Reading;

public class AiStreamFinalStatusesTests
{
    [Fact]
    public void Values_ShouldMatchDomainAiRequestStatusConstants()
    {
        Assert.Equal(AiRequestStatus.Completed, AiStreamFinalStatuses.Completed);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, AiStreamFinalStatuses.FailedBeforeFirstToken);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, AiStreamFinalStatuses.FailedAfterFirstToken);
    }

    [Theory]
    [InlineData("completed", true)]
    [InlineData("failed_before_first_token", true)]
    [InlineData("failed_after_first_token", true)]
    [InlineData("failed", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsSupported_ShouldValidateKnownValues(string? status, bool expected)
    {
        Assert.Equal(expected, AiStreamFinalStatuses.IsSupported(status));
    }
}
