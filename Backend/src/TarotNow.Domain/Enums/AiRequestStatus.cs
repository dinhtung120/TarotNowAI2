/*
 * ===================================================================
 * FILE: AiRequestStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Sử Dụng Text Constants Cố Định Mô Tả Tình Trạng Phiên Cắn Tiền Đi Gọi AI Của Khách.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Mappings Trạng Thái Luồng Gọi AI (Thay Cờ Lập Trình Cho Enum Để DB Postgres SQL Lưu Bằng Chữ Đọc Hiểu Luôn Không Quá Lỗi Reflection Lên Json).
/// DB Sẽ Bấm Đo Xem Phiên Stream Này Gọi Cứng 1 Lần Chết Trước Hai Không Hoàn Tiền Code Lỗi Lấy Gì Mà Phạt AI.
/// </summary>
public static class AiRequestStatus
{
    // Bóp Nút Đã Nộp Đơn Chờ Cổng API Mở Ra Nhả Chữ.
    public const string Requested = "requested";
    // Loa Đọc Xong Hoàn Thành Mãn Nguyện Cầm Code Cho Khách Rõ Ràng.
    public const string Completed = "completed";
    
    // Tạch Lỗi DB Ngay Lập Tức Hoặc AI Ban Account Từ Api Không Phun Nổi Giọt Chữ (Thất Bại Chết Lỗi Sạch Sẽ Được Refund Kím Tiền Lại Lên User).
    public const string FailedBeforeFirstToken = "failed_before_first_token";
    // Mạng Bị Rớt Đoạn Stream Chữ Khúc Cuối Đang Ra Giữa Chừng Bật Ho Chết Hút API (Thua Tịt - Tùy Policy Có Refund Không Do Lỗi Timeout Khủng DB).
    public const string FailedAfterFirstToken = "failed_after_first_token";
}
