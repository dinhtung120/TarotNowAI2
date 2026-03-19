/*
 * ===================================================================
 * FILE: QuestionItemType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Ngăn Giá Cước Từng Câu Chưởi Đắt Rẻ Kiểu Này.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Mẫu Tem Phân Định Loại Câu Bốc Xỉn Tạp.
/// Dồn Tiền Vô Cốc Nào Ở Tạp Trí (Ví Lợi Do Cây Gộp Ngắn Hay Hỏi Đúp Chéo Già Rẻ Bào Thầy Chém Nhắn Nhanh Đeo Bám Follow-up).
/// </summary>
public static class QuestionItemType
{
    // Quả Bài Gốc Thu Vào Câu Rút Chi Phí Chát Đầu Ngồi Nói Với Nhau Lần 1 Thảo Thuận Xa Xôi.
    public const string MainQuestion = "main_question";
    
    // Câu Bồi Hỏi Kì Kèo Cho Hiểu Rõ Lá Này Xíu Thầy Bói Ăn Trừ Tiền Kín 1 Méo (Hỏi Thêm Phụ Đòi Follow-up Càng Ít Tiền).
    public const string AddQuestion = "add_question";
}
