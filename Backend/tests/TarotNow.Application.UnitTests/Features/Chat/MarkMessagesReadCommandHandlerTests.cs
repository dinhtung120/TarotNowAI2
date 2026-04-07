

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

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

        [Fact]
    public async Task Handle_NotMember_ThrowsBadRequest()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "c1", ReaderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };
        
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

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
    }

        [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        var command = new MarkMessagesReadCommand { ConversationId = "non_existent", ReaderId = Guid.NewGuid() };
        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default)).ReturnsAsync((ConversationDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

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
