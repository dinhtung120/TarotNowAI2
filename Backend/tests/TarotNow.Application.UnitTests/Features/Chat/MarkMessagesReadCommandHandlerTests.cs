using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

// Unit test cho handler đánh dấu tin nhắn đã đọc.
public class MarkMessagesReadCommandHandlerTests
{
    // Mock conversation repo để kiểm soát membership và unread counters.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Mock message repo để xác nhận gọi mark-as-read.
    private readonly Mock<IChatMessageRepository> _mockMsgRepo;
    // Mock domain event publisher để xác nhận enqueue outbox events.
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    // Handler cần kiểm thử.
    private readonly MarkMessagesReadCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho MarkMessagesReadCommandHandler.
    /// Luồng sử dụng mock repositories để test logic unread count độc lập DB thật.
    /// </summary>
    public MarkMessagesReadCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockMsgRepo = new Mock<IChatMessageRepository>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _handler = new MarkMessagesReadCommandHandler(
            _mockConvRepo.Object,
            _mockMsgRepo.Object,
            _mockDomainEventPublisher.Object);
    }

    /// <summary>
    /// Xác nhận người không thuộc conversation bị từ chối.
    /// Luồng này bảo vệ quyền truy cập thao tác mark read.
    /// </summary>
    [Fact]
    public async Task Handle_NotMember_ThrowsBadRequest()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "c1", ReaderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận member hợp lệ sẽ reset unread count đúng phía và gọi mark message read.
    /// Luồng này kiểm tra cả kết quả trả về và side-effect cập nhật repository.
    /// </summary>
    [Fact]
    public async Task Handle_Member_ResetsUnreadCountAndMarksMessages()
    {
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new MarkMessagesReadCommand { ConversationId = "c1", ReaderId = Guid.Parse(readerIdStr) };
        var conv = new ConversationDto { Id = "c1", UserId = Guid.NewGuid().ToString(), ReaderId = readerIdStr, UnreadCountReader = 5 };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(0, conv.UnreadCountReader);
        _mockMsgRepo.Verify(x => x.MarkAsReadAsync("c1", readerIdStr, default), Times.Once);
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(It.IsAny<UnreadCountChangedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(It.IsAny<ConversationUpdatedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận conversation không tồn tại sẽ ném NotFoundException.
    /// Luồng này bảo vệ handler khỏi thao tác trên dữ liệu không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "non_existent", ReaderId = Guid.NewGuid() };
        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default)).ReturnsAsync((ConversationDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận user đánh dấu đã đọc chỉ reset UnreadCountUser.
    /// Luồng này kiểm tra không làm thay đổi nhầm unread count phía reader.
    /// </summary>
    [Fact]
    public async Task Handle_UserMarksRead_ResetsUnreadCountUser()
    {
        var userIdStr = Guid.NewGuid().ToString();
        var command = new MarkMessagesReadCommand
        {
            ConversationId = "c1",
            ReaderId = Guid.Parse(userIdStr)
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = userIdStr,
            ReaderId = Guid.NewGuid().ToString(),
            UnreadCountUser = 3,
            UnreadCountReader = 5
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(0, conv.UnreadCountUser);
        Assert.Equal(5, conv.UnreadCountReader);
    }
}
