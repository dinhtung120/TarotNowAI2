using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class CreateReportCommandHandlerTests
{
    private readonly Mock<IReportRepository> _mockReportRepo;
    private readonly CreateReportCommandHandler _handler;

    public CreateReportCommandHandlerTests()
    {
        _mockReportRepo = new Mock<IReportRepository>();
        _handler = new CreateReportCommandHandler(_mockReportRepo.Object);
    }

    [Fact]
    public async Task Handle_InvalidTargetType_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "invalid", Reason = "This is a valid reason" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "message", Reason = "short" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesPendingReport()
    {
        var reporterIdStr = Guid.NewGuid().ToString();
        var command = new CreateReportCommand 
        { 
            ReporterId = Guid.Parse(reporterIdStr), 
            TargetType = "message", 
            TargetId = "msg1", 
            Reason = "This is a valid reason" 
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("message", result.TargetType);
        Assert.Equal("This is a valid reason", result.Reason);
        Assert.Equal("pending", result.Status);

        _mockReportRepo.Verify(x => x.AddAsync(It.Is<ReportDto>(r => r.ReporterId == reporterIdStr && r.Status == "pending"), default), Times.Once);
    }
}
