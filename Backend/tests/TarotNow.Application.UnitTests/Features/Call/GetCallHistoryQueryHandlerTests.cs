using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Queries.GetCallHistory;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

// Unit test cho query lấy lịch sử cuộc gọi theo hội thoại.
public class GetCallHistoryQueryHandlerTests
{
    // Mock call repository để kiểm soát dữ liệu lịch sử trả về.
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    // Mock conversation repository để kiểm tra quyền participant.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Handler cần kiểm thử.
    private readonly GetCallHistoryQueryHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho GetCallHistoryQueryHandler.
    /// Luồng dùng mock repo để cô lập handler khỏi hạ tầng dữ liệu.
    /// </summary>
    public GetCallHistoryQueryHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new GetCallHistoryQueryHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ trả về danh sách lịch sử phân trang đúng.
    /// Luồng setup conversation hợp lệ và mock tổng số bản ghi để kiểm tra mapping kết quả.
    /// </summary>
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

        // Gọi handler và kiểm tra totalCount/items khớp dữ liệu từ repository.
        var query = new GetCallHistoryQuery { ConversationId = conversationId, ParticipantId = participantId, Page = 1, PageSize = 20 };
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
    }

    /// <summary>
    /// Xác nhận người không thuộc hội thoại bị chặn bằng ForbiddenException.
    /// Luồng này bảo vệ quyền riêng tư lịch sử cuộc gọi theo participant.
    /// </summary>
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
