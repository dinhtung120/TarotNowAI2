using System;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

// Tập trạng thái kết thúc hợp lệ cho luồng hoàn tất AI stream.
public static class AiStreamFinalStatuses
{
    // Trạng thái stream hoàn thành thành công.
    public static readonly string Completed = AiRequestStatus.Completed;

    // Trạng thái stream lỗi trước khi trả token đầu tiên.
    public static readonly string FailedBeforeFirstToken = AiRequestStatus.FailedBeforeFirstToken;

    // Trạng thái stream lỗi sau khi đã trả token đầu tiên.
    public static readonly string FailedAfterFirstToken = AiRequestStatus.FailedAfterFirstToken;

    /// <summary>
    /// Kiểm tra trạng thái kết thúc có thuộc tập hỗ trợ hay không.
    /// Luồng xử lý: so sánh chính xác status đầu vào với ba trạng thái chuẩn để khóa chặt ngữ nghĩa thanh toán.
    /// </summary>
    public static bool IsSupported(string? status)
    {
        return string.Equals(status, Completed, StringComparison.Ordinal) ||
               string.Equals(status, FailedBeforeFirstToken, StringComparison.Ordinal) ||
               string.Equals(status, FailedAfterFirstToken, StringComparison.Ordinal);
    }
}
