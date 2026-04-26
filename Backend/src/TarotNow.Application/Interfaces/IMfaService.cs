

using System.Collections.Generic;

namespace TarotNow.Application.Interfaces;

// Contract dịch vụ MFA để chuẩn hóa quy trình tạo bí mật, xác thực mã và mã dự phòng.
public interface IMfaService
{
    /// <summary>
    /// Tạo secret key mới cho MFA để khởi tạo cấu hình xác thực 2 lớp.
    /// Luồng xử lý: sinh chuỗi bí mật theo chuẩn TOTP dùng cho cả QR và verify mã.
    /// </summary>
    string GenerateSecretKey();

    /// <summary>
    /// Mã hóa secret trước khi lưu trữ để tránh lộ khóa MFA ở dạng thô.
    /// Luồng xử lý: nhận plainSecret và trả về chuỗi đã mã hóa dùng cho persistence.
    /// </summary>
    string EncryptSecret(string plainSecret);

    /// <summary>
    /// Giải mã secret khi cần xác thực mã MFA do người dùng nhập.
    /// Luồng xử lý: nhận encryptedSecret và khôi phục plainSecret trong phạm vi xử lý an toàn.
    /// </summary>
    string DecryptSecret(string encryptedSecret);

    /// <summary>
    /// Tạo URI QR theo chuẩn authenticator để người dùng quét cấu hình MFA.
    /// Luồng xử lý: ghép plainSecret với userEmail thành chuỗi otpauth dùng cho app OTP.
    /// </summary>
    string GenerateQrCodeUri(string plainSecret, string userEmail);

    /// <summary>
    /// Xác thực mã MFA người dùng nhập để quyết định cho phép đăng nhập.
    /// Luồng xử lý: dùng plainSecret tính mã TOTP tại thời điểm hiện tại và so sánh với code.
    /// </summary>
    bool VerifyCode(string plainSecret, string code);

    /// <summary>
    /// Băm backup code trước khi lưu để chống lộ mã sử dụng một lần.
    /// </summary>
    string HashBackupCode(string code);

    /// <summary>
    /// Verify backup code đầu vào với hash đã lưu.
    /// </summary>
    bool VerifyBackupCode(string code, string storedHash);

    /// <summary>
    /// Tạo danh sách mã dự phòng để người dùng dùng khi mất thiết bị OTP.
    /// Luồng xử lý: sinh ngẫu nhiên count mã backup và trả về để lưu/hash theo chính sách bảo mật.
    /// </summary>
    List<string> GenerateBackupCodes(int count = 6);
}
