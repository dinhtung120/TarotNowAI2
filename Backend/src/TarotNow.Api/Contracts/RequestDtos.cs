/*
 * ===================================================================
 * FILE: RequestDtos.cs
 * NAMESPACE: TarotNow.Api.Contracts
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   File này chứa các "DTO" (Data Transfer Object) - tức là các lớp
 *   dùng để NHẬN DỮ LIỆU từ phía client (trình duyệt, app điện thoại)
 *   khi client gửi request đến server API.
 *
 * GIẢI THÍCH ĐƠN GIẢN:
 *   Khi người dùng ấn nút "Cập nhật hồ sơ" trên giao diện, trình duyệt
 *   sẽ gửi một "gói dữ liệu" (JSON) lên server. Server cần biết gói đó
 *   có những trường nào, kiểu dữ liệu gì → các class bên dưới chính là
 *   "khuôn mẫu" để server hiểu và đọc dữ liệu đó.
 *
 * TẠI SAO TÁCH FILE RIÊNG?
 *   Theo nguyên tắc Clean Architecture, tầng API (Presentation) không nên
 *   trộn lẫn logic xử lý với định nghĩa dữ liệu. Tách DTO ra file riêng
 *   giúp dễ bảo trì, dễ tìm kiếm, và nhiều controller có thể cùng dùng.
 * ===================================================================
 */

// Khai báo namespace - tức là "địa chỉ thư mục logic" của file này
// Mọi class bên dưới đều thuộc về nhóm "TarotNow.Api.Contracts"
// giúp phân biệt với các class cùng tên ở nơi khác trong dự án.
namespace TarotNow.Api.Contracts;

/// <summary>
/// DTO (Data Transfer Object) cho yêu cầu CẬP NHẬT HỒ SƠ người dùng.
/// Khi user gửi request PUT/PATCH đến endpoint "/api/profile",
/// server sẽ ánh xạ (map) JSON body sang đối tượng này.
/// </summary>
public class UpdateProfileRequest
{
    /*
     * DisplayName: Tên hiển thị của người dùng.
     * - Kiểu "string" (chuỗi ký tự) vì tên là văn bản.
     * - "get; set;" nghĩa là cho phép ĐỌC và GHI giá trị (thuộc tính công khai).
     * - "= string.Empty" nghĩa là giá trị mặc định là chuỗi rỗng "",
     *   tránh lỗi null (không có giá trị) khi client quên gửi trường này.
     */
    public string DisplayName { get; set; } = string.Empty;

    /*
     * AvatarUrl: Đường dẫn ảnh đại diện của người dùng.
     * - Kiểu "string?" (có dấu "?") nghĩa là trường này CÓ THỂ KHÔNG CÓ GIÁ TRỊ (nullable).
     * - Lý do cho phép null: không phải ai cũng muốn đổi ảnh đại diện mỗi lần cập nhật,
     *   nên nếu client không gửi trường này, server hiểu là "giữ nguyên ảnh cũ".
     */
    public string? AvatarUrl { get; set; }

    /*
     * DateOfBirth: Ngày tháng năm sinh của người dùng.
     * - Kiểu "DateTime" là kiểu dữ liệu chuyên dùng cho ngày/giờ trong C#.
     * - Không có dấu "?" nên trường này BẮT BUỘC phải có giá trị.
     * - Dùng để tính tuổi, hiển thị cung hoàng đạo, v.v. trong ứng dụng tarot.
     */
    public DateTime DateOfBirth { get; set; }
}

/// <summary>
/// DTO cho yêu cầu TẠO ĐƠN NẠP TIỀN.
/// Khi user muốn nạp tiền vào ví, client gửi request POST đến endpoint "/api/deposit"
/// kèm theo số tiền muốn nạp. Server nhận thông tin qua class này.
/// </summary>
public class CreateDepositOrderRequest
{
    /*
     * AmountVnd: Số tiền nạp, tính bằng đồng Việt Nam (VND).
     * - Kiểu "long" (số nguyên lớn, tối đa ~9.2 x 10^18) thay vì "int" (tối đa ~2.1 tỷ)
     *   vì số tiền VND có thể rất lớn (ví dụ 10.000.000 VND).
     * - Dùng số nguyên thay vì số thập phân (decimal/float) vì VND không có đơn vị lẻ
     *   (không có xu), tránh sai số do phép tính số thực.
     */
    public long AmountVnd { get; set; }
}

/// <summary>
/// DTO cho yêu cầu GHI NHẬN SỰ ĐỒNG Ý pháp lý (consent).
/// Ví dụ: khi user tích vào ô "Tôi đồng ý với Điều khoản sử dụng",
/// client gửi request lên server để lưu lại sự đồng ý đó.
/// Điều này rất quan trọng về mặt pháp lý (GDPR, luật bảo vệ dữ liệu cá nhân).
/// </summary>
public class RecordConsentRequest
{
    /*
     * DocumentType: Loại văn bản pháp lý mà user đồng ý.
     * - Ví dụ: "tos" (Terms of Service - Điều khoản dịch vụ),
     *           "privacy" (Privacy Policy - Chính sách bảo mật),
     *           "ai-disclaimer" (Miễn trừ trách nhiệm AI).
     * - Dùng string thay vì enum để linh hoạt thêm loại mới mà không cần sửa code.
     */
    public string DocumentType { get; set; } = string.Empty;

    /*
     * Version: Phiên bản của văn bản pháp lý.
     * - Ví dụ: "1.0", "2.3"
     * - Mỗi khi cập nhật nội dung văn bản, phiên bản tăng lên,
     *   và user cần đồng ý lại phiên bản mới.
     * - Giúp theo dõi chính xác user đã đồng ý phiên bản nào.
     */
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// DTO cho yêu cầu CẬP NHẬT CHƯƠNG TRÌNH KHUYẾN MÃI.
/// Admin (quản trị viên) dùng endpoint này để sửa đổi thông tin
/// một chương trình khuyến mãi nạp tiền đã tồn tại.
/// </summary>
public class UpdatePromotionRequest
{
    /*
     * MinAmountVnd: Số tiền nạp TỐI THIỂU (VND) để được hưởng khuyến mãi.
     * - Ví dụ: nạp từ 100.000 VND trở lên mới được tặng diamond.
     * - Kiểu "long" vì lý do tương tự AmountVnd ở trên.
     */
    public long MinAmountVnd { get; set; }

    /*
     * BonusDiamond: Số diamond (kim cương - đơn vị tiền ảo trong app) được tặng thêm.
     * - Ví dụ: nạp 100.000 VND được tặng thêm 50 diamond.
     * - Đây là phần thưởng khuyến khích user nạp tiền.
     */
    public long BonusDiamond { get; set; }

    /*
     * IsActive: Chương trình khuyến mãi có đang hoạt động hay không.
     * - true = đang hoạt động, user có thể hưởng khuyến mãi.
     * - false = đã tắt, không áp dụng cho giao dịch mới.
     * - Admin có thể bật/tắt mà không cần xóa dữ liệu khuyến mãi.
     */
    public bool IsActive { get; set; }
}
