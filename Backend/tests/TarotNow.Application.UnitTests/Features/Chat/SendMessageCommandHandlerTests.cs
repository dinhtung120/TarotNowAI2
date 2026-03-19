/*
 * FILE: SendMessageCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler gửi tin nhắn trong cuộc hội thoại.
 *
 *   CÁC TEST CASE (7 scenarios):
 *   1. Handle_InvalidType_ThrowsBadRequest: loại tin nhắn sai → 400
 *   2. Handle_EmptyText_ThrowsBadRequest: nội dung trống → 400
 *   3. Handle_NotAMember_ThrowsBadRequest: User không thuộc conversation → 400
 *   4. Handle_ValidMessage_PersistsMessageAndUpdatesUnreadCount:
 *      → Happy path: lưu message + Pending→Active + tăng UnreadCountReader
 *   5. Handle_ConversationNotFound_ThrowsNotFoundException: ConversationId sai → 404
 *   6. Handle_CompletedConversation_ThrowsBadRequest: Conversation đã kết thúc → 400
 *   7. Handle_ReaderSendsMessage_UserUnreadCountIncreases:
 *      → Reader gửi → tăng UnreadCountUser (KHÔNG phải UnreadCountReader)
 *
 *   LOGIC QUAN TRỌNG:
 *   → Phân biệt sender = User hay Reader → tăng đúng unread counter
 *   → Tin nhắn đầu tiên chuyển Pending → Active (auto-activate)
 *   → Conversation Completed/Cancelled/Disputed → không cho gửi tin mới
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

/// <summary>
/// Test send message: validation, auto-activate, unread counter logic (User vs Reader).
/// </summary>
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

    /// <summary>Loại tin nhắn không hợp lệ → BadRequest.</summary>
    [Fact]
    public async Task Handle_InvalidType_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = "invalid_type", Content = "a" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Nội dung trống → BadRequest.</summary>
    [Fact]
    public async Task Handle_EmptyText_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>User không thuộc conversation → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotAMember_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "a", ConversationId = "c1", SenderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Happy path: lưu message + Pending→Active + tăng UnreadCountReader.
    /// </summary>
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
        Assert.Equal(ConversationStatus.Active, conv.Status); // Pending → Active
        Assert.Equal(1, conv.UnreadCountReader); // User gửi → Reader chưa đọc

        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Once);
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
    }

    /// <summary>ConversationId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text, Content = "Hello",
            ConversationId = "non_existent", SenderId = Guid.NewGuid()
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default)).ReturnsAsync((ConversationDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Conversation đã kết thúc → BadRequest (không cho gửi tin vào evidence).
    /// </summary>
    [Fact]
    public async Task Handle_CompletedConversation_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text, Content = "Hello",
            ConversationId = "c1", SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1", UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Completed // Kết thúc
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("kết thúc", ex.Message);
        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Never);
    }

    /// <summary>
    /// Reader gửi tin → increment UnreadCountUser (KHÔNG PHẢI UnreadCountReader).
    /// Bug tiềm ẩn: nhầm counter → reader tự thấy badge của chính mình.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderSendsMessage_UserUnreadCountIncreases()
    {
        var userIdStr = Guid.NewGuid().ToString();
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text, Content = "Reader reply",
            ConversationId = "c1", SenderId = Guid.Parse(readerIdStr) // Reader gửi
        };
        var conv = new ConversationDto
        {
            Id = "c1", UserId = userIdStr, ReaderId = readerIdStr,
            Status = ConversationStatus.Active,
            UnreadCountUser = 0, UnreadCountReader = 0
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, conv.UnreadCountUser);   // Reader gửi → User chưa đọc
        Assert.Equal(0, conv.UnreadCountReader);  // Reader gửi → giữ nguyên
    }
}
