

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

// Unit test cho handler tạo report trong module chat.
public class CreateReportCommandHandlerRequestedDomainEventHandlerTests
{
    // Mock report repo để xác minh side-effect lưu report.
    private readonly Mock<IReportRepository> _mockReportRepo;
    private readonly Mock<IConversationRepository> _mockConversationRepo;
    private readonly Mock<IChatMessageRepository> _mockChatMessageRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Handler cần kiểm thử.
    private readonly CreateReportCommandHandlerRequestedDomainEventHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho CreateReportCommandHandlerRequestedDomainEventHandler.
    /// Luồng dùng mock repository để kiểm thử logic validate và tạo report.
    /// </summary>
    public CreateReportCommandHandlerRequestedDomainEventHandlerTests()
    {
        _mockReportRepo = new Mock<IReportRepository>();
        _mockConversationRepo = new Mock<IConversationRepository>();
        _mockChatMessageRepo = new Mock<IChatMessageRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _handler = new CreateReportCommandHandlerRequestedDomainEventHandler(
            _mockReportRepo.Object,
            _mockConversationRepo.Object,
            _mockChatMessageRepo.Object,
            _mockUserRepo.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());
    }

    /// <summary>
    /// Xác nhận target type không hợp lệ bị từ chối.
    /// Luồng này bảo vệ danh mục loại đối tượng có thể report.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidTargetType_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "invalid", Reason = "This is a valid reason" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận lý do report quá ngắn bị từ chối.
    /// Luồng này đảm bảo report có đủ thông tin để vận hành xử lý.
    /// </summary>
    [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new CreateReportCommand { TargetType = "message", Reason = "short" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận request hợp lệ tạo report trạng thái pending.
    /// Luồng này kiểm tra mapping trường cơ bản và thao tác AddAsync vào repository.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesPendingReport()
    {
        var reporterIdStr = Guid.NewGuid().ToString();
        var command = new CreateReportCommand
        {
            ReporterId = Guid.Parse(reporterIdStr),
            TargetType = ReportTargetTypes.Message,
            TargetId = "msg1",
            Reason = "This is a valid reason"
        };
        _mockChatMessageRepo
            .Setup(x => x.GetByIdAsync("msg1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatMessageDto
            {
                Id = "msg1",
                ConversationId = "conv1",
                SenderId = Guid.NewGuid().ToString()
            });
        _mockConversationRepo
            .Setup(x => x.GetByIdAsync("conv1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConversationDto
            {
                Id = "conv1",
                UserId = reporterIdStr,
                ReaderId = Guid.NewGuid().ToString(),
                Status = ConversationStatus.Ongoing
            });

        // Thực thi handler và kiểm tra dữ liệu report đầu ra.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ReportTargetTypes.Message, result.TargetType);
        Assert.Equal("This is a valid reason", result.Reason);
        Assert.Equal("pending", result.Status);
        Assert.Equal("conv1", result.ConversationRef);

        // Xác nhận repo nhận report với reporterId và status pending đúng kỳ vọng.
        _mockReportRepo.Verify(x => x.AddAsync(It.Is<ReportDto>(r => r.ReporterId == reporterIdStr && r.Status == "pending"), default), Times.Once);
    }
}
