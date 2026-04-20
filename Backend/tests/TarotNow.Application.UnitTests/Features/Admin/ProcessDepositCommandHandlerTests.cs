using TarotNow.Application.Features.Admin.Commands.ProcessDeposit;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Admin;

// Unit test xác nhận manual process deposit đã bị vô hiệu hóa.
public class ProcessDepositCommandHandlerTests
{
    /// <summary>
    /// Handler luôn trả false để chặn luồng duyệt tay.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenManualProcessIsDisabled()
    {
        var handler = new ProcessDepositCommandHandler();
        var command = new ProcessDepositCommand
        {
            DepositId = Guid.NewGuid(),
            Action = "approve",
            TransactionId = "manual-txn"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result);
    }
}
