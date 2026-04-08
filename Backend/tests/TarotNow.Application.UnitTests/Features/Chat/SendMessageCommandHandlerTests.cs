

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

// Unit test cho handler gửi tin nhắn chat (text/image/voice) và luồng tài chính liên quan.
public class SendMessageCommandHandlerTests
{
    // Mock conversation repo để điều khiển membership và trạng thái conversation.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Mock message repo để kiểm tra thao tác lưu message.
    private readonly Mock<IChatMessageRepository> _mockMsgRepo;
    // Mock finance repo để mô phỏng session/item tài chính chat.
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    // Mock wallet repo để xác nhận freeze ví khi gửi câu hỏi.
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    // Mock reader profile repo để lấy trạng thái reader và pricing.
    private readonly Mock<IReaderProfileRepository> _mockReaderProfileRepo;
    // Mock transaction coordinator để chạy transactional flow trong test.
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    // Mock media processor để tránh phụ thuộc xử lý media thật.
    private readonly Mock<IMediaProcessorService> _mockMediaProcessorService;
    // Mock wallet push service cho side-effect realtime.
    private readonly Mock<IWalletPushService> _mockWalletPushService;
    // Handler cần kiểm thử.
    private readonly SendMessageCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho SendMessageCommandHandler.
    /// Luồng setup transaction/media mock để mọi test chạy deterministic và không phụ thuộc hạ tầng ngoài.
    /// </summary>
    public SendMessageCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockMsgRepo = new Mock<IChatMessageRepository>();
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockReaderProfileRepo = new Mock<IReaderProfileRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken token) => action(token));
        _mockMediaProcessorService = new Mock<IMediaProcessorService>();
        _mockMediaProcessorService
            .Setup(x => x.ProcessAndCompressVoiceAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[] data, string ext, CancellationToken _) => (data, "audio/webm"));

        _mockWalletPushService = new Mock<IWalletPushService>();

        _handler = new SendMessageCommandHandler(
            _mockConvRepo.Object,
            _mockMsgRepo.Object,
            _mockFinanceRepo.Object,
            _mockWalletRepo.Object,
            _mockReaderProfileRepo.Object,
            _mockTransactionCoordinator.Object,
            _mockMediaProcessorService.Object,
            _mockWalletPushService.Object);
    }

    /// <summary>
    /// Xác nhận type không hợp lệ bị từ chối.
    /// Luồng này bảo vệ danh mục loại message được phép gửi.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidType_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = "invalid_type", Content = "a" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận text message rỗng bị từ chối.
    /// Luồng này đảm bảo chất lượng dữ liệu nội dung trước khi persist.
    /// </summary>
    [Fact]
    public async Task Handle_EmptyText_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận sender không thuộc conversation bị chặn.
    /// Luồng này bảo vệ quyền gửi tin nhắn theo membership hội thoại.
    /// </summary>
    [Fact]
    public async Task Handle_NotAMember_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "a", ConversationId = "c1", SenderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận tin nhắn hợp lệ được lưu và unread count được cập nhật đúng phía.
    /// Luồng này đồng thời kiểm tra nhánh pending -> awaiting acceptance và freeze ví theo pricing reader.
    /// </summary>
    [Fact]
    public async Task Handle_ValidMessage_PersistsMessageAndUpdatesUnreadCount()
    {
        var senderIdStr = Guid.NewGuid().ToString();
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "Hello", ConversationId = "c1", SenderId = Guid.Parse(senderIdStr) };
        var conv = new ConversationDto { Id = "c1", UserId = senderIdStr, ReaderId = readerIdStr, Status = ConversationStatus.Pending, UnreadCountReader = 0 };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);
        _mockReaderProfileRepo
            .Setup(x => x.GetByUserIdAsync(readerIdStr, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReaderProfileDto
            {
                UserId = readerIdStr,
                Status = ReaderOnlineStatus.Online,
                DiamondPerQuestion = 10
            });
        _mockFinanceRepo
            .Setup(x => x.GetItemByIdempotencyKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatQuestionItem?)null);
        _mockFinanceRepo
            .Setup(x => x.GetSessionByConversationRefAsync("c1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatFinanceSession?)null);

        // Thực thi luồng gửi tin nhắn text trong conversation pending.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ChatMessageType.Text, result.Type);
        Assert.Equal("Hello", result.Content);
        Assert.Equal(ConversationStatus.AwaitingAcceptance, conv.Status);
        Assert.Equal(1, conv.UnreadCountReader);

        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
        _mockWalletRepo.Verify(
            x => x.FreezeAsync(
                It.IsAny<Guid>(),
                10,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận conversation không tồn tại trả NotFoundException.
    /// Luồng này chặn persist message vào hội thoại không hợp lệ.
    /// </summary>
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
    /// Xác nhận conversation đã completed không cho gửi thêm message.
    /// Luồng này bảo toàn trạng thái đóng của hội thoại.
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
            Status = ConversationStatus.Completed
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("kết thúc", ex.Message);
        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Never);
    }

    /// <summary>
    /// Xác nhận reader gửi message sẽ tăng UnreadCountUser thay vì UnreadCountReader.
    /// Luồng này kiểm tra hướng tăng unread theo vai trò sender.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderSendsMessage_UserUnreadCountIncreases()
    {
        var userIdStr = Guid.NewGuid().ToString();
        var readerIdStr = Guid.NewGuid().ToString();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text, Content = "Reader reply",
            ConversationId = "c1", SenderId = Guid.Parse(readerIdStr)
        };
        var conv = new ConversationDto
        {
            Id = "c1", UserId = userIdStr, ReaderId = readerIdStr,
            Status = ConversationStatus.Ongoing,
            UnreadCountUser = 0, UnreadCountReader = 0
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, conv.UnreadCountUser);
        Assert.Equal(0, conv.UnreadCountReader);
    }

    /// <summary>
    /// Xác nhận image URL chuẩn được chấp nhận và có media payload fallback phù hợp.
    /// Luồng này kiểm tra nhánh xử lý ảnh không yêu cầu upload binary.
    /// </summary>
    [Fact]
    public async Task Handle_ImageMessageWithUrl_FallbackPayloadAccepted()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Image,
            Content = "https://cdn.example.com/card.png",
            ConversationId = "c1",
            SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ChatMessageType.Image, result.Type);
        Assert.NotNull(result.MediaPayload);
        Assert.Equal("image/png", result.MediaPayload!.MimeType);
    }

    /// <summary>
    /// Xác nhận voice message vượt giới hạn duration bị từ chối.
    /// Luồng này bảo vệ business rule giới hạn thời lượng voice.
    /// </summary>
    [Fact]
    public async Task Handle_VoiceMessageDurationTooLong_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Voice,
            Content = "voice",
            MediaPayload = new MediaPayloadDto
            {
                Url = "https://cdn.example.com/voice.webm",
                MimeType = "audio/webm",
                DurationMs = 700_000
            },
            ConversationId = "c1",
            SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận ảnh định dạng AVIF được chấp nhận.
    /// Luồng này kiểm tra whitelist MIME/image extension mới.
    /// </summary>
    [Fact]
    public async Task Handle_ImageMessageAvif_IsAccepted()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Image,
            Content = "https://cdn.example.com/card.avif",
            ConversationId = "c1",
            SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ChatMessageType.Image, result.Type);
        Assert.NotNull(result.MediaPayload);
        Assert.Equal("image/avif", result.MediaPayload!.MimeType);
    }

    /// <summary>
    /// Xác nhận voice MIME có suffix codec vẫn được normalize và chấp nhận.
    /// Luồng này kiểm tra nhánh xử lý mime type phức tạp dễ gây hiểu nhầm.
    /// </summary>
    [Fact]
    public async Task Handle_VoiceMessageWithCodecSuffix_IsAccepted()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Voice,
            Content = "voice",
            MediaPayload = new MediaPayloadDto
            {
                Url = "data:audio/webm;codecs=opus;base64,ZmFrZQ==",
                MimeType = "audio/webm;codecs=opus",
                DurationMs = 1_500
            },
            ConversationId = "c1",
            SenderId = senderId
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ChatMessageType.Voice, result.Type);
        Assert.NotNull(result.MediaPayload);
        Assert.Equal("audio/webm", result.MediaPayload!.MimeType);
    }
}
