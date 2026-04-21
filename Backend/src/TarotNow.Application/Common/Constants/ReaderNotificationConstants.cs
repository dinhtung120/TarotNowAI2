namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Hằng số notification cho luồng duyệt Reader.
/// </summary>
public static class ReaderNotificationTypes
{
    /// <summary>
    /// Notification khi đơn Reader được duyệt.
    /// </summary>
    public const string ReaderRequestApproved = "reader_request_approved";

    /// <summary>
    /// Notification khi đơn Reader bị từ chối.
    /// </summary>
    public const string ReaderRequestRejected = "reader_request_rejected";
}
