using FluentAssertions;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Application.UnitTests.Reading;

public sealed class CompleteAiStreamSettlementPolicyTests
{
    private readonly AiStreamSettlementPolicy _policy = new();

    [Fact]
    public void CompleteAiStream_ShouldConsumeEscrow_WhenStreamCompleted()
    {
        var decision = _policy.Decide(new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.Completed
        });

        decision.ShouldConsumeEscrow.Should().BeTrue();
        decision.Reason.Should().Be("completed");
    }

    [Fact]
    public void CompleteAiStream_ShouldRefund_WhenProviderFailsBeforeFirstToken()
    {
        var decision = _policy.Decide(new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.FailedBeforeFirstToken
        });

        decision.ShouldConsumeEscrow.Should().BeFalse();
        decision.Reason.Should().Be("failed_before_first_token");
    }

    [Fact]
    public void CompleteAiStream_ShouldRefund_WhenProviderFailsAfterFirstTokenAndPolicyDoesNotAllowPartialConsume()
    {
        var decision = _policy.Decide(new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.FailedAfterFirstToken,
            IsClientDisconnect = false,
            OutputTokens = 100,
            EmittedChunkCount = 10
        });

        decision.ShouldConsumeEscrow.Should().BeFalse();
        decision.Reason.Should().Be("provider_failed_after_first_token");
    }

    [Fact]
    public void CompleteAiStream_ShouldRefund_WhenClientDisconnectsBelowBillableThreshold()
    {
        var decision = _policy.Decide(new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.FailedAfterFirstToken,
            IsClientDisconnect = true,
            OutputTokens = 20,
            EmittedChunkCount = 2
        });

        decision.ShouldConsumeEscrow.Should().BeFalse();
        decision.Reason.Should().Be("client_disconnect_below_billable_threshold");
    }

    [Fact]
    public void CompleteAiStream_ShouldConsumeOrPartiallyConsume_WhenClientDisconnectsAtOrAboveBillableThreshold()
    {
        var decision = _policy.Decide(new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.FailedAfterFirstToken,
            IsClientDisconnect = true,
            OutputTokens = 40,
            EmittedChunkCount = 1
        });

        decision.ShouldConsumeEscrow.Should().BeTrue();
        decision.Reason.Should().Be("client_disconnect_after_billable_threshold");
    }

    [Fact]
    public void CompleteAiStream_ShouldEmitSettlementReasonTelemetry()
    {
        var command = new CompleteAiStreamCommand
        {
            FinalStatus = AiStreamFinalStatuses.FailedAfterFirstToken,
            IsClientDisconnect = true,
            OutputTokens = 10,
            EmittedChunkCount = 1
        };

        var decision = _policy.Decide(command);
        command.SettlementReason = decision.Reason;

        command.SettlementReason.Should().Be("client_disconnect_below_billable_threshold");
    }
}
