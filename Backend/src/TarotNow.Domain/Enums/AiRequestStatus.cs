
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái vòng đời của một yêu cầu AI.
public static class AiRequestStatus
{
    // Yêu cầu vừa được tạo và chờ xử lý.
    public const string Requested = "requested";

    // Yêu cầu đã hoàn tất thành công.
    public const string Completed = "completed";

    // Yêu cầu thất bại trước khi trả token đầu tiên.
    public const string FailedBeforeFirstToken = "failed_before_first_token";

    // Yêu cầu thất bại sau khi đã trả ít nhất một token.
    public const string FailedAfterFirstToken = "failed_after_first_token";
}
