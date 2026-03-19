/*
 * FILE: CreateReportCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler báo cáo vi phạm (Report).
 *
 *   CÁC TEST CASE:
 *   1. Handle_InvalidTargetType_ThrowsBadRequest: loại target sai (vd: "invalid") → 400
 *   2. Handle_ShortReason_ThrowsBadRequest: lý do quá ngắn → 400 (tránh spam/abuse)
 *   3. Handle_ValidRequest_CreatesPendingReport: tạo report status=pending → chờ Admin xử lý
 *
 *   QUY TẮC:
 *   → TargetType hợp lệ: "message", "user", v.v.
 *   → Reason phải đủ dài (tránh report spam 1-2 từ)
 *   → Report mới luôn status=pending → Admin queue
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

/// <summary>
/// Test report: validation (target type, reason length), pending status creation.
/// </summary>
public class CreateReportCommandHandlerTests
{
    private readonly Mock<IReportRepository> _mockReportRepo;
    private readonly CreateReportCommandHandler _handler;

    public CreateReportCommandHandlerTests()
    {
        _mockReportRepo = new Mock<IReportRepository>();
        _handler = new CreateReportCommandHandler(_mockReportRepo.Object);
    }

    /// <summary>TargetType không hợp lệ → BadRequest.</summary>
    [Fact]
    public async Task Handle_InvalidTargetType_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "invalid", Reason = "This is a valid reason" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reason quá ngắn → BadRequest (tránh spam).</summary>
    [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "message", Reason = "short" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Happy path: tạo report → status=pending, đúng target + reason.
    /// </summary>
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
        Assert.Equal("pending", result.Status); // Chờ Admin xử lý

        _mockReportRepo.Verify(x => x.AddAsync(It.Is<ReportDto>(r => r.ReporterId == reporterIdStr && r.Status == "pending"), default), Times.Once);
    }
}
