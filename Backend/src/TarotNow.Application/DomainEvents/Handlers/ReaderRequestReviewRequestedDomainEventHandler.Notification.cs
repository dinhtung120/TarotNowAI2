using TarotNow.Application.Common;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
{
    private const string NotificationMetadataRequestId = "requestId";
    private const string NotificationMetadataStatus = "status";
    private const string NotificationMetadataAction = "action";

    private async Task PublishReviewNotificationAsync(
        ReaderRequestDto readerRequest,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        string resultingStatus,
        CancellationToken cancellationToken)
    {
        var userId = ParseUserId(readerRequest.UserId);
        var notification = BuildNotification(userId, readerRequest.Id, domainEvent, resultingStatus);

        await _notificationRepository.CreateAsync(notification, cancellationToken);
        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Notifications,
            RealtimeEventNames.NotificationNew,
            new
            {
                userId = notification.UserId.ToString(),
                notification.TitleVi,
                notification.TitleEn,
                notification.TitleZh,
                notification.BodyVi,
                notification.BodyEn,
                notification.BodyZh,
                notification.Type,
                createdAt = DateTime.UtcNow
            },
            cancellationToken);
    }

    private static NotificationCreateDto BuildNotification(
        Guid userId,
        string requestId,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        string resultingStatus)
    {
        return resultingStatus == ReaderApprovalStatus.Approved
            ? BuildApprovedNotification(userId, requestId, domainEvent)
            : BuildRejectedNotification(userId, requestId, domainEvent);
    }

    private static NotificationCreateDto BuildApprovedNotification(
        Guid userId,
        string requestId,
        ReaderRequestReviewRequestedDomainEvent domainEvent)
    {
        return new NotificationCreateDto
        {
            UserId = userId,
            Type = ReaderNotificationTypes.ReaderRequestApproved,
            TitleVi = "Đơn Reader đã được phê duyệt",
            TitleEn = "Reader application approved",
            TitleZh = "占卜师申请已获批准",
            BodyVi = BuildApprovedBodyVi(domainEvent.AdminNote),
            BodyEn = BuildApprovedBodyEn(domainEvent.AdminNote),
            BodyZh = BuildApprovedBodyZh(domainEvent.AdminNote),
            Metadata = BuildNotificationMetadata(requestId, ReaderApprovalStatus.Approved, domainEvent.Action)
        };
    }

    private static NotificationCreateDto BuildRejectedNotification(
        Guid userId,
        string requestId,
        ReaderRequestReviewRequestedDomainEvent domainEvent)
    {
        return new NotificationCreateDto
        {
            UserId = userId,
            Type = ReaderNotificationTypes.ReaderRequestRejected,
            TitleVi = "Đơn Reader chưa được phê duyệt",
            TitleEn = "Reader application rejected",
            TitleZh = "占卜师申请未通过",
            BodyVi = BuildRejectedBodyVi(domainEvent.AdminNote),
            BodyEn = BuildRejectedBodyEn(domainEvent.AdminNote),
            BodyZh = BuildRejectedBodyZh(domainEvent.AdminNote),
            Metadata = BuildNotificationMetadata(requestId, ReaderApprovalStatus.Rejected, domainEvent.Action)
        };
    }

    private static Dictionary<string, string> BuildNotificationMetadata(
        string requestId,
        string status,
        string action)
    {
        return new Dictionary<string, string>
        {
            [NotificationMetadataRequestId] = requestId,
            [NotificationMetadataStatus] = status,
            [NotificationMetadataAction] = action
        };
    }

    private static string BuildApprovedBodyVi(string? adminNote)
    {
        var body = "Hồ sơ Reader của bạn đã được phê duyệt. Vui lòng đăng nhập lại để cập nhật quyền mới.";
        return AppendAdminNote(body, adminNote, " Ghi chú: ");
    }

    private static string BuildApprovedBodyEn(string? adminNote)
    {
        var body = "Your reader application has been approved. Please sign in again to refresh your new role.";
        return AppendAdminNote(body, adminNote, " Note: ");
    }

    private static string BuildApprovedBodyZh(string? adminNote)
    {
        var body = "您的占卜师申请已获批准。请重新登录以更新新权限。";
        return AppendAdminNote(body, adminNote, " 备注：");
    }

    private static string BuildRejectedBodyVi(string? adminNote)
    {
        var body = "Đơn đăng ký Reader của bạn chưa được phê duyệt. Bạn có thể cập nhật hồ sơ và nộp lại.";
        return AppendAdminNote(body, adminNote, " Ghi chú: ");
    }

    private static string BuildRejectedBodyEn(string? adminNote)
    {
        var body = "Your reader application was not approved. You can update your profile and submit again.";
        return AppendAdminNote(body, adminNote, " Note: ");
    }

    private static string BuildRejectedBodyZh(string? adminNote)
    {
        var body = "您的占卜师申请未通过。您可以完善资料后重新提交。";
        return AppendAdminNote(body, adminNote, " 备注：");
    }

    private static string AppendAdminNote(string body, string? adminNote, string prefix)
    {
        if (string.IsNullOrWhiteSpace(adminNote))
        {
            return body;
        }

        return string.Concat(body, prefix, adminNote.Trim());
    }
}
