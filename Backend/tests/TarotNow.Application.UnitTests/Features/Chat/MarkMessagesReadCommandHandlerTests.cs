/*
 * FILE: MarkMessagesReadCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler đánh dấu tin nhắn đã đọc.
 *
 *   CÁC TEST CASE (4 scenarios):
 *   1. Handle_NotMember_ThrowsBadRequest: User không thuộc conversation → 400
 *   2. Handle_Member_ResetsUnreadCountAndMarksMessages: Reader đọc → reset UnreadCountReader + mark messages
 *   3. Handle_ConversationNotFound_ThrowsNotFoundException: ConversationId sai → 404
 *   4. Handle_UserMarksRead_ResetsUnreadCountUser:
 *      → User đọc → reset UnreadCountUser về 0, GIỮ NGUYÊN UnreadCountReader
 *
 *   LOGIC QUAN TRỌNG:
 *   → Handler phân biệt UserId vs ReaderId → reset đúng counter
 *   → Nếu bug: user mở chat nhưng reset nhầm UnreadCountReader → reader mất badge
 *   → Cần cẩn thận với SignalR reconnect gửi stale conversation ID
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

/// <summary>
/// Test mark-as-read: member check, counter reset logic (User vs Reader).
/// </summary>
public class MarkMessagesReadCommandHandlerTests
{
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly Mock<IChatMessageRepository> _mockMsgRepo;
    private readonly MarkMessagesReadCommandHandler _handler;

    public MarkMessagesReadCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockMsgRepo = new Mock<IChatMessageRepository>();
        _handler = new MarkMessagesReadCommandHandler(_mockConvRepo.Object, _mockMsgRepo.Object);
    }

    /// <summary>User không thuộc conversation → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotMember_ThrowsBadRequest()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "c1", ReaderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reader đọc tin → reset UnreadCountReader + mark messages as read.</summary>
    [Fact]
    public async Task Handle_Member_ResetsUnreadCountAndMarksMessages()
    {
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new MarkMessagesReadCommand { ConversationId = "c1", ReaderId = Guid.Parse(readerIdStr) };
        var conv = new ConversationDto { Id = "c1", UserId = Guid.NewGuid().ToString(), ReaderId = readerIdStr, UnreadCountReader = 5 };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(0, conv.UnreadCountReader); // Reset về 0
        _mockMsgRepo.Verify(x => x.MarkAsReadAsync("c1", readerIdStr, default), Times.Once);
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
    }

    /// <summary>ConversationId sai → NotFoundException (quan trọng khi SignalR reconnect).</summary>
    [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "non_existent", ReaderId = Guid.NewGuid() };
        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default)).ReturnsAsync((ConversationDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// User (không phải reader) đọc tin → reset UnreadCountUser, GIỮ NGUYÊN UnreadCountReader.
    /// Bug tiềm ẩn: nếu reset nhầm counter → reader mất notification badge.
    /// </summary>
    [Fact]
    public async Task Handle_UserMarksRead_ResetsUnreadCountUser()
    {
        var userIdStr = Guid.NewGuid().ToString();
        var command = new MarkMessagesReadCommand
        {
            ConversationId = "c1",
            ReaderId = Guid.Parse(userIdStr) // "ReaderId" trong command = người đang đọc
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = userIdStr,
            ReaderId = Guid.NewGuid().ToString(),
            UnreadCountUser = 3,   // User có 3 tin chưa đọc
            UnreadCountReader = 5  // Reader có 5 tin chưa đọc
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(0, conv.UnreadCountUser);   // Reset đúng counter
        Assert.Equal(5, conv.UnreadCountReader);  // GIỮ NGUYÊN counter Reader
    }
}
