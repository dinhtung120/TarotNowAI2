namespace TarotNow.Api.Contracts.Requests;

// Payload gửi tin nhắn trong luồng hội thoại.
public class ConversationSendMessageBody
{
    // Loại tin nhắn để backend chọn đúng nhánh xử lý (text, media...).
    public string Type { get; set; } = "text";

    // Nội dung text của tin nhắn khi loại message hỗ trợ văn bản.
    public string Content { get; set; } = string.Empty;

    // Định danh phía client để idempotency khi retry và reconcile optimistic UI.
    public string? ClientMessageId { get; set; }

    // Payload media đi kèm khi message không thuần văn bản.
    public TarotNow.Application.Common.MediaPayloadDto? MediaPayload { get; set; }
}

// Payload từ chối một đề nghị trong hội thoại.
public class ConversationRejectBody
{
    // Lý do từ chối để hiển thị minh bạch cho phía còn lại.
    public string? Reason { get; set; }
}

// Payload phản hồi cho bước hoàn tất hội thoại.
public class ConversationCompleteRespondBody
{
    // Quyết định chấp nhận hoặc từ chối yêu cầu hoàn tất.
    public bool Accept { get; set; }
}

// Payload đề nghị nạp thêm kim cương vào phiên hội thoại.
public class ConversationAddMoneyRequestBody
{
    // Số kim cương đề nghị thêm vào phiên đang chạy.
    public long AmountDiamond { get; set; }

    // Mô tả lý do nạp thêm để tăng tính minh bạch nghiệp vụ.
    public string? Description { get; set; }

    // Khóa chống tạo lệnh nạp trùng khi client retry.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// Payload phản hồi cho đề nghị nạp thêm kim cương.
public class ConversationAddMoneyRespondBody
{
    // Quyết định chấp nhận hoặc từ chối đề nghị nạp thêm.
    public bool Accept { get; set; }

    // Id message của đề nghị ban đầu để đối chiếu đúng bản ghi.
    public string OfferMessageId { get; set; } = string.Empty;

    // Lý do từ chối khi quyết định là không chấp nhận.
    public string? RejectReason { get; set; }
}

// Payload mở dispute trong ngữ cảnh hội thoại.
public class ConversationDisputeBody
{
    // Item tranh chấp cụ thể nếu dispute nhắm vào một mục riêng lẻ.
    public Guid? ItemId { get; set; }

    // Lý do dispute do người dùng cung cấp.
    public string Reason { get; set; } = string.Empty;
}
