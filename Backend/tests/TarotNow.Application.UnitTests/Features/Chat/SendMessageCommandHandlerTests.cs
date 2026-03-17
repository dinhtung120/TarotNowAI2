using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class SendMessageCommandHandlerTests
{
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly Mock<IChatMessageRepository> _mockMsgRepo;
    private readonly SendMessageCommandHandler _handler;

    public SendMessageCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockMsgRepo = new Mock<IChatMessageRepository>();
        _handler = new SendMessageCommandHandler(_mockConvRepo.Object, _mockMsgRepo.Object);
    }

    [Fact]
    public async Task Handle_InvalidType_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = "invalid_type", Content = "a" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_EmptyText_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NotAMember_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "a", ConversationId = "c1", SenderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidMessage_PersistsMessageAndUpdatesUnreadCount()
    {
        var senderIdStr = Guid.NewGuid().ToString();
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "Hello", ConversationId = "c1", SenderId = Guid.Parse(senderIdStr) };
        var conv = new ConversationDto { Id = "c1", UserId = senderIdStr, ReaderId = readerIdStr, Status = ConversationStatus.Pending, UnreadCountReader = 0 };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ChatMessageType.Text, result.Type);
        Assert.Equal("Hello", result.Content);
        Assert.Equal(ConversationStatus.Active, conv.Status); // Pending -> Active
        Assert.Equal(1, conv.UnreadCountReader); // User sent msg -> Reader unread count increases

        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Once);
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Conversation không tồn tại → NotFoundException.
    ///
    /// Tại sao cần test?
    /// → Phòng trường hợp ConversationId sai hoặc đã bị xóa.
    ///   Handler phải throw rõ ràng, không phải NullReferenceException.
    /// </summary>
    [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        // Arrange — conversation không tồn tại trong DB
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Hello",
            ConversationId = "non_existent",
            SenderId = Guid.NewGuid()
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default))
            .ReturnsAsync((ConversationDto)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Conversation đã kết thúc (completed) → BadRequestException.
    ///
    /// Tại sao quan trọng?
    /// → Khi conversation completed/cancelled/disputed, KHÔNG cho phép gửi tin mới.
    ///   Nếu bug: user vẫn gửi tin vào conversation đã dispute → ảnh hưởng evidence.
    ///   Đây là gate check ở handler line 48-49.
    /// </summary>
    [Fact]
    public async Task Handle_CompletedConversation_ThrowsBadRequest()
    {
        // Arrange — conversation đã completed
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Hello",
            ConversationId = "c1",
            SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Completed // Đã kết thúc
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("kết thúc", ex.Message);

        // Verify — KHÔNG persist message nào
        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Never);
    }

    /// <summary>
    /// TEST CASE: Reader gửi tin nhắn → UnreadCountUser phải tăng (không phải UnreadCountReader).
    ///
    /// Tại sao cần test riêng?
    /// → Handler dùng logic if/else dựa trên senderId == UserId hay ReaderId.
    ///   Nếu nhầm: reader gửi tin nhưng UnreadCountReader tăng → reader tự nhận unread badge.
    ///   Bug này rất khó phát hiện bằng mắt mà phải có test.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderSendsMessage_UserUnreadCountIncreases()
    {
        // Arrange — reader là người gửi
        var userIdStr = Guid.NewGuid().ToString();
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Reader reply",
            ConversationId = "c1",
            SenderId = Guid.Parse(readerIdStr) // Reader gửi
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = userIdStr,
            ReaderId = readerIdStr,
            Status = ConversationStatus.Active,
            UnreadCountUser = 0,
            UnreadCountReader = 0
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert — UnreadCountUser tăng (vì reader gửi → user chưa đọc)
        Assert.Equal(1, conv.UnreadCountUser);
        // UnreadCountReader giữ nguyên = 0 (reader là người gửi, không cần tăng)
        Assert.Equal(0, conv.UnreadCountReader);
    }
}
