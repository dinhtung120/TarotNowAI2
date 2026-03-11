namespace TarotNow.Domain.Enums;

/// <summary>
/// Mappings trạng thái luồng xử lý AI Request (Thay cho Enum để tránh lỗi Reflection).
/// </summary>
public static class AiRequestStatus
{
    public const string Requested = "requested";
    public const string FirstTokenReceived = "first_token_received";
    public const string Completed = "completed";
    public const string FailedBeforeFirstToken = "failed_before_first_token";
    public const string FailedAfterFirstToken = "failed_after_first_token";
}
