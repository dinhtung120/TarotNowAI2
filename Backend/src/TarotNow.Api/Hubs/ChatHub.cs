/*
 * ===================================================================
 * FILE: ChatHub.cs
 * NAMESPACE: TarotNow.Api.Hubs
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   SignalR Hub cho CHAT THỜI GIAN THỰC (Realtime Messaging) giữa
 *   User và Reader. Đây là "bộ não" xử lý giao tiếp 2 chiều tức thì.
 *
 * SIGNALR LÀ GÌ?
 *   Thư viện của Microsoft giúp server và client giao tiếp 2 chiều (bidirectional).
 *   Giải thích đơn giản:
 *   - HTTP bình thường: Client hỏi → Server trả lời (1 chiều, mỗi lần hỏi)
 *   - SignalR: Client VÀ Server có thể gửi tin cho nhau BẤT CỨ LÚC NÀO
 *     mà không cần hỏi trước.
 *   
 *   Dùng WebSocket làm transport chính (nhanh nhất).
 *   Tự động fallback sang SSE hoặc Long Polling nếu WebSocket không khả dụng.
 *
 * KHÁC GÌ VỚI ConversationController?
 *   Controller = REST API: dùng cho tạo conversation, lấy lịch sử tin nhắn
 *   Hub = WebSocket: dùng cho gửi/nhận tin nhắn NGAY LẬP TỨC
 *
 * GROUP PATTERN:
 *   Mỗi conversation = 1 group trong SignalR.
 *   Khi user gửi tin nhắn → broadcast cho TẤT CẢ members trong group.
 *   Members = chỉ 2 người: user và reader (chat 1-1).
 *
 * XÁC THỰC:
 *   JWT token được gửi qua query string (?access_token=xxx)
 *   vì WebSocket KHÔNG hỗ trợ custom headers (khác với HTTP).
 *
 * LUỒNG CHAT:
 *   1. Client kết nối: new HubConnectionBuilder().withUrl("/api/v1/chat?access_token=xxx")
 *   2. Client gọi JoinConversation(conversationId) → vào nhóm
 *   3. Client gọi SendMessage(conversationId, content) → lưu DB + broadcast
 *   4. Client nhận event "ReceiveMessage" → hiển thị tin nhắn mới
 *   5. Client gọi MarkRead(conversationId) → đánh dấu đã đọc
 *   6. Khi ngắt kết nối → tự động rời tất cả nhóm
 * ===================================================================
 */

using MediatR;                    // Gửi commands qua MediatR
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền
using Microsoft.AspNetCore.SignalR;       // Nền tảng SignalR Hub
using System.Security.Claims;             // Đọc JWT claims

// Import các Command cho chat
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

// Import exceptions và interfaces
using TarotNow.Application.Exceptions;   // BadRequestException, NotFoundException
using TarotNow.Application.Common;       // Các class dùng chung

namespace TarotNow.Api.Hubs;

/*
 * [Authorize]: TẤT CẢ method trong Hub đều yêu cầu xác thực.
 *   Client phải gửi JWT token qua query string khi kết nối.
 *   
 * Hub (kế thừa): Class base của SignalR, cung cấp:
 *   - Context: thông tin connection hiện tại (ConnectionId, User)
 *   - Clients: gửi message đến client(s) khác
 *   - Groups: quản lý nhóm (thêm/xóa connection)
 */
[Authorize]
public class ChatHub : Hub
{
    /*
     * _mediator: gửi commands để lưu tin nhắn vào DB.
     *   Đồng thời dùng query ValidateConversationAccess để kiểm tra quyền join conversation.
     * _logger: ghi log kết nối, gửi tin, lỗi.
     */
    private readonly IMediator _mediator;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(
        IMediator mediator,
        ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lấy UserId từ JWT claims.
    /// Context.User: thông tin user từ JWT token đã được xác thực.
    /// Expression body (=>) cho hàm 1 dòng.
    /// </summary>
    private string? GetUserId() =>
        Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // ======================================================================
    // VÒNG ĐỜI KẾT NỐI (CONNECTION LIFECYCLE)
    // SignalR tự động gọi các method này khi client kết nối/ngắt kết nối.
    // ======================================================================

    /// <summary>
    /// Được gọi TỰ ĐỘNG khi client kết nối thành công vào Hub.
    ///
    /// Context.ConnectionId: ID duy nhất cho mỗi kết nối.
    ///   Mỗi tab trình duyệt = 1 connection riêng biệt.
    ///   Cùng 1 user có thể có nhiều connection (nhiều tab, nhiều thiết bị).
    ///
    /// MÌNH DÙNG ĐỂ:
    ///   - Ghi log: biết ai kết nối, lúc nào
    ///   - Có thể mở rộng: track trạng thái online, thông báo cho reader
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        _logger.LogInformation("[ChatHub] User {UserId} connected. ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
        
        // Gọi base method để SignalR hoàn tất xử lý nội bộ
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Được gọi TỰ ĐỘNG khi client ngắt kết nối.
    ///
    /// NGUYÊN NHÂN:
    ///   - User đóng tab/trình duyệt
    ///   - Mất mạng Internet
    ///   - Server restart
    ///
    /// SignalR TỰ ĐỘNG remove connection khỏi tất cả groups.
    ///   Không cần gọi RemoveFromGroupAsync thủ công.
    ///
    /// exception: null = ngắt bình thường, có giá trị = bị lỗi
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        _logger.LogInformation("[ChatHub] User {UserId} disconnected. Reason: {Reason}", 
            userId, exception?.Message ?? "normal");
        
        await base.OnDisconnectedAsync(exception);
    }

    // ======================================================================
    // CÁC METHOD MÀ CLIENT GỌI được qua connection.invoke("MethodName", args)
    // ======================================================================

    /// <summary>
    /// PHƯƠNG THỨC: JoinConversation(conversationId)
    /// MỤC ĐÍCH: Client tham gia vào nhóm chat → bắt đầu nhận tin nhắn realtime.
    ///
    /// CLIENT GỌI: connection.invoke("JoinConversation", "abc-conversation-id")
    ///
    /// KIỂM TRA BẢO MẬT:
    ///   1. User đã xác thực (Authorize)
    ///   2. Conversation tồn tại
    ///   3. User là thành viên của conversation (user hoặc reader)
    ///   → Nếu sai: gửi event "Error" cho client
    ///
    /// SAU KHI JOIN:
    ///   Connection được thêm vào SignalR group (group name = conversationId).
    ///   Mọi tin nhắn broadcast cho group → connection này cũng nhận được.
    /// </summary>
    public async Task JoinConversation(string conversationId)
    {
        var userId = GetUserId();

        // Kiểm tra đã xác thực
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            // Gửi event "Error" chỉ cho client gọi (Caller)
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        // Kiểm tra conversationId không rỗng
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            await Clients.Caller.SendAsync("Error", "ConversationId is required");
            return;
        }

        var accessStatus = await _mediator.Send(new ValidateConversationAccessQuery
        {
            ConversationId = conversationId,
            RequesterId = userGuid
        });

        if (accessStatus == ConversationAccessStatus.NotFound)
        {
            await Clients.Caller.SendAsync("Error", "Conversation not found");
            return;
        }

        if (accessStatus == ConversationAccessStatus.Forbidden)
        {
            await Clients.Caller.SendAsync("Error", "Forbidden");
            return;
        }

        /*
         * Thêm connection hiện tại vào SignalR group.
         * Group name = conversationId → dễ quản lý.
         * Từ giờ, mọi tin nhắn gửi đến group này → connection này nhận được.
         */
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        // Thông báo cho TẤT CẢ members trong group rằng có người vừa join
        // (dùng cho tính năng "online indicator" ở client)
        await Clients.Group(conversationId).SendAsync("UserJoined", new
        {
            userId,
            conversationId,
            joinedAt = DateTime.UtcNow // Thời điểm join (UTC)
        });

        _logger.LogInformation("[ChatHub] User {UserId} joined conversation {ConversationId}", userId, conversationId);
    }

    /// <summary>
    /// PHƯƠNG THỨC: LeaveConversation(conversationId)
    /// MỤC ĐÍCH: Client rời nhóm chat → ngừng nhận tin nhắn realtime.
    ///
    /// KHI NÀO GỌI?
    ///   - Client chuyển sang màn hình khác (navigate away from chat)
    ///   - Client muốn tắt notification cho conversation này
    ///
    /// LƯU Ý: Khi client ngắt kết nối (đóng app), SignalR TỰ ĐỘNG remove
    /// khỏi tất cả groups → không cần gọi LeaveConversation.
    /// Method này chỉ dùng khi client vẫn online nhưng muốn rời group.
    /// </summary>
    public async Task LeaveConversation(string conversationId)
    {
        var userId = GetUserId();
        
        // Xóa connection khỏi group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        
        _logger.LogInformation("[ChatHub] User {UserId} left conversation {ConversationId}", userId, conversationId);
    }

    /// <summary>
    /// PHƯƠNG THỨC: SendMessage(conversationId, content, type)
    /// MỤC ĐÍCH: Gửi tin nhắn → lưu vào MongoDB → broadcast cho nhóm.
    ///
    /// CLIENT GỌI: connection.invoke("SendMessage", "conv-id", "Hello!", "text")
    /// GROUP NHẬN: event "ReceiveMessage" với ChatMessageDto
    ///
    /// PARAMETER "type": loại tin nhắn
    ///   - "text": tin nhắn văn bản bình thường
    ///   - "image": ảnh
    ///   - "proposal": đề nghị giá từ reader
    ///   - "system": tin nhắn hệ thống (auto-generated)
    ///
    /// LUỒNG XỬ LÝ:
    ///   1. Validate user + dữ liệu
    ///   2. Gửi SendMessageCommand qua MediatR → Handler lưu vào MongoDB
    ///   3. Handler trả về ChatMessageDto (tin nhắn đã lưu, có timestamp)
    ///   4. Broadcast ChatMessageDto cho tất cả members trong group
    ///   5. Cả 2 bên client nhận được tin nhắn gần như tức thì
    /// </summary>
    public async Task SendMessage(string conversationId, string content, string type = "text")
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        try
        {
            /*
             * Gửi command qua MediatR → handler thực hiện:
             * 1. Kiểm tra user thuộc conversation
             * 2. Kiểm tra conversation chưa bị đóng
             * 3. Lưu tin nhắn vào MongoDB collection "messages"
             * 4. Cập nhật lastMessageAt của conversation
             * 5. Tăng unreadCount cho bên kia
             * 6. Trả về ChatMessageDto (tin nhắn vừa lưu)
             */
            var command = new SendMessageCommand
            {
                ConversationId = conversationId,
                SenderId = userGuid,
                Type = type,
                Content = content
            };

            var message = await _mediator.Send(command);

            /*
             * Broadcast tin nhắn cho TẤT CẢ members trong group.
             * Clients.Group(groupName): chọn nhóm để gửi
             * SendAsync("ReceiveMessage", message): gửi event "ReceiveMessage" kèm data
             * 
             * Phía client lắng nghe:
             *   connection.on("ReceiveMessage", (msg) => { displayMessage(msg); });
             */
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", message);
        }
        catch (BadRequestException ex)
        {
            // Lỗi do dữ liệu không hợp lệ → thông báo cho client gọi
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch (NotFoundException ex)
        {
            // Conversation không tìm thấy
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch (Exception ex)
        {
            // Lỗi không xác định → log + thông báo chung cho client
            _logger.LogError(ex,
                "[ChatHub] SendMessage failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await Clients.Caller.SendAsync("Error", "Unable to send message. Please try again.");
        }
    }

    /// <summary>
    /// PHƯƠNG THỨC: MarkRead(conversationId)
    /// MỤC ĐÍCH: Đánh dấu tin nhắn đã đọc → reset bộ đếm chưa đọc (unread count).
    ///
    /// KHI NÀO GỌI?
    ///   - Khi client mở conversation (đọc tin nhắn cũ)
    ///   - Khi đang ở trong conversation và nhận tin mới
    ///
    /// HIỆU ỨNG:
    ///   - Cập nhật unreadCount = 0 cho user này trong conversation
    ///   - Broadcast event "MessagesRead" cho group → bên kia thấy "đã đọc" ✓✓
    ///     (giống tick xanh WhatsApp)
    /// </summary>
    public async Task MarkRead(string conversationId)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        try
        {
            // Gửi command đánh dấu đã đọc qua MediatR
            var command = new MarkMessagesReadCommand
            {
                ConversationId = conversationId,
                ReaderId = userGuid // "ReaderId" ở đây nghĩa là "người đọc tin nhắn" (reader of messages)
            };

            await _mediator.Send(command);

            // Broadcast "MessagesRead" cho group → bên kia biết tin nhắn đã được đọc
            await Clients.Group(conversationId).SendAsync("MessagesRead", new
            {
                userId,
                conversationId,
                readAt = DateTime.UtcNow // Thời điểm đọc
            });
        }
        catch (BadRequestException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch (NotFoundException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[ChatHub] MarkRead failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await Clients.Caller.SendAsync("Error", "Unable to mark messages as read. Please try again.");
        }
    }
}
