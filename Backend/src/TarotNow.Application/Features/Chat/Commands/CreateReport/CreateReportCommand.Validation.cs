using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

public partial class CreateReportCommandHandlerRequestedDomainEventHandler
{
    private async Task<string?> ValidateTargetAndResolveConversationRefAsync(
        Guid reporterId,
        string targetType,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        var normalizedTargetId = NormalizeRequiredTargetId(targetId);
        return targetType switch
        {
            ReportTargetTypes.Message => await ResolveMessageTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                conversationRef,
                cancellationToken),
            ReportTargetTypes.Conversation => await ResolveConversationTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                cancellationToken),
            ReportTargetTypes.User => await ResolveUserTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                conversationRef,
                cancellationToken),
            _ => throw new BadRequestException("Loại đối tượng không hợp lệ.")
        };
    }

    private static string NormalizeRequiredTargetId(string targetId)
    {
        var normalizedTargetId = targetId?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(normalizedTargetId))
        {
            return normalizedTargetId;
        }

        throw new BadRequestException("TargetId là bắt buộc.");
    }

    private async Task<string> ResolveMessageTargetConversationAsync(
        Guid reporterId,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        var message = await _chatMessageRepo.GetByIdAsync(targetId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy message cần report.");
        if (IsConversationRefMismatch(conversationRef, message.ConversationId))
        {
            throw new BadRequestException("ConversationRef không khớp với message được report.");
        }

        var conversation = await _conversationRepo.GetByIdAsync(message.ConversationId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy conversation của message được report.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        return conversation.Id;
    }

    private async Task<string> ResolveConversationTargetConversationAsync(
        Guid reporterId,
        string targetId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(targetId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy conversation cần report.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        return conversation.Id;
    }

    private async Task<string?> ResolveUserTargetConversationAsync(
        Guid reporterId,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(targetId, out var targetUserId))
        {
            throw new BadRequestException("TargetId user không hợp lệ.");
        }

        var user = await _userRepo.GetByIdAsync(targetUserId, cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Không tìm thấy user cần report.");
        }

        if (string.IsNullOrWhiteSpace(conversationRef))
        {
            throw new BadRequestException("Report user bắt buộc có ConversationRef để chứng minh ngữ cảnh tương tác.");
        }

        var conversation = await _conversationRepo.GetByIdAsync(conversationRef.Trim(), cancellationToken)
            ?? throw new BadRequestException("ConversationRef không tồn tại.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        var normalizedTargetUserId = targetUserId.ToString();
        if (!string.Equals(conversation.UserId, normalizedTargetUserId, StringComparison.Ordinal)
            && !string.Equals(conversation.ReaderId, normalizedTargetUserId, StringComparison.Ordinal))
        {
            throw new BadRequestException("Target user không thuộc conversation được cung cấp.");
        }

        return conversation.Id;
    }

    private static bool IsConversationRefMismatch(string? conversationRef, string conversationId)
    {
        return !string.IsNullOrWhiteSpace(conversationRef)
               && !string.Equals(conversationRef.Trim(), conversationId, StringComparison.Ordinal);
    }

    private static void EnsureReporterHasConversationAccess(Guid reporterId, ConversationDto conversation)
    {
        var reporter = reporterId.ToString();
        if (conversation.UserId == reporter || conversation.ReaderId == reporter)
        {
            return;
        }

        throw new BadRequestException("Bạn không có quyền report đối tượng ngoài phạm vi truy cập.");
    }
}
