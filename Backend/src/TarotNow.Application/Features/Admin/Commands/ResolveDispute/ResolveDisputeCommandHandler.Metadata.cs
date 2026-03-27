using System.Text.Json;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private static string BuildResolveAuditMetadata(Guid adminId, string action, string? adminNote)
    {
        var normalizedAdminNote = string.IsNullOrWhiteSpace(adminNote)
            ? null
            : adminNote.Trim();

        if (normalizedAdminNote is { Length: > 500 })
        {
            normalizedAdminNote = normalizedAdminNote[..500];
        }

        return JsonSerializer.Serialize(new
        {
            adminId,
            action,
            adminNote = normalizedAdminNote
        });
    }
}
