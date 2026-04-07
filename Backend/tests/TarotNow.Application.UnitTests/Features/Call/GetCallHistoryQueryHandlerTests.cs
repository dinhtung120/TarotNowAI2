using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Queries.GetCallHistory;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

public class GetCallHistoryQueryHandlerTests
{
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly GetCallHistoryQueryHandler _handler;

    public GetCallHistoryQueryHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new GetCallHistoryQueryHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPaginatedHistory()
    {
        var conversationId = "conv-1";
        var participantId = Guid.NewGuid();
        
        var conv = new ConversationDto { Id = conversationId, UserId = participantId.ToString() };

        var mockItems = new List<CallSessionDto> { new CallSessionDto(), new CallSessionDto() };
        
        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(r => r.GetByConversationIdPaginatedAsync(conversationId, 1, 20, default))
                     .ReturnsAsync((mockItems, 2));

        var query = new GetCallHistoryQuery { ConversationId = conversationId, ParticipantId = participantId, Page = 1, PageSize = 20 };
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
    }

    [Fact]
    public async Task Handle_NonParticipant_ThrowsForbidden()
    {
        var conversationId = "conv-1";
        var participantId = Guid.NewGuid();
        
        var conv = new ConversationDto { Id = conversationId, UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);

        var query = new GetCallHistoryQuery { ConversationId = conversationId, ParticipantId = participantId };
        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
