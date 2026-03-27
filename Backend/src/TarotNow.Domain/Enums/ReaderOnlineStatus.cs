/*
 * ===================================================================
 * FILE: ReaderOnlineStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trạng Thái Biển Treo Cửa Của Thầy Bói (Reader).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái biển chỉ dẫn trực tuyến của Reader để UI hiển thị.
/// Gồm 3 trạng thái: Online (tự động), Offline (tự động sau 15p), Busy (thủ công).
/// </summary>
public static class ReaderOnlineStatus
{
    /// <summary>Có mặt trên web (tự động bắt từ PresenceHub).</summary>
    public const string Online = "online";

    /// <summary>Tắt ca nghỉ hoặc timeout quá 15 phút không thao tác.</summary>
    public const string Offline = "offline";

    /// <summary>Reader đang bận, chủ động chuyển sang trạng thái này báo khách báo chậm.</summary>
    public const string Busy = "busy";

    public static bool IsValid(string? status)
        => TryNormalize(status, out _);

    public static bool TryNormalize(string? status, out string normalized)
    {
        normalized = Offline;
        if (string.IsNullOrWhiteSpace(status))
        {
            return false;
        }

        var value = status.Trim().ToLowerInvariant();
        switch (value)
        {
            case Online:
            case "active":
            case "connected":
            case "available":
                normalized = Online;
                return true;
            case Offline:
            case "disconnected":
            case "invisible":
                normalized = Offline;
                return true;
            case Busy:
            case "away":
            case "idle":
            case "accepting_questions":
            case "acceptingquestions":
            case "accepting-questions":
            case "accepting":
            case "ready":
                // Map cả các trạng thái cũ sang Busy để tương thích ngược data cũ
                normalized = Busy;
                return true;
            default:
                return false;
        }
    }

    public static string NormalizeOrDefault(string? status, string defaultStatus = Offline)
        => TryNormalize(status, out var normalized) ? normalized : defaultStatus;
}
