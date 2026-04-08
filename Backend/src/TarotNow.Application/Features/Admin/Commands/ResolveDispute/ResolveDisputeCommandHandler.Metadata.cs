using System.Text.Json;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    /// <summary>
    /// Dựng JSON metadata audit cho giao dịch resolve dispute.
    /// Luồng xử lý: chuẩn hóa admin note, cắt tối đa 500 ký tự, serialize thành JSON compact.
    /// </summary>
    private static string BuildResolveAuditMetadata(Guid adminId, string action, string? adminNote)
    {
        var normalizedAdminNote = string.IsNullOrWhiteSpace(adminNote)
            ? null
            : adminNote.Trim();

        if (normalizedAdminNote is { Length: > 500 })
        {
            // Giới hạn độ dài note để tránh metadata quá lớn ở transaction records.
            normalizedAdminNote = normalizedAdminNote[..500];
        }

        // Lưu đủ admin/action/note để phục vụ audit và truy vết quyết định xử lý.
        return JsonSerializer.Serialize(new
        {
            adminId,
            action,
            adminNote = normalizedAdminNote
        });
    }
}
