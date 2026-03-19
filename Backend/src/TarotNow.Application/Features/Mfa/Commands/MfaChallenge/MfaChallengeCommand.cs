/*
 * ===================================================================
 * FILE: MfaChallengeCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Mfa.Commands.MfaChallenge
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Xác Nhập OTP (Google Authenticator) dành cho Chốt Chặn Giao Dịch Quan Trọng.
 *
 * NOTE BẢO MẬT:
 *   Khác với lúc Login (Cứ Mật khẩu đúng là cho vào). 
 *   Các thao tác Xé Ra Tiền (VD: Reader rút tiền vào ngân hàng, Đổi Mật Khẩu, Đổi Email) 
 *   BẮT BUỘC phải xì ra được Mã Code MFA (6 số) thay đổi mỗi 30s.
 *   
 * TÍNH NĂNG CỨU RỖI (BACKUP CODES):
 *   Nếu lỡ làm rớt điện thoại xuống bồn cầu -> Nhập 1 trong 10 mã sơ cua (Backup Code).
 *   Mỗi mã sơ cua chỉ Mua được đúng 1 mạng. Xài xong là bốc hơi luôn.
 * ===================================================================
 */

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

/// <summary>
/// Thẻ Căn Cước Nhất Thời (Hỏi Passcode 6 Số)
/// </summary>
public class MfaChallengeCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
    /// <summary>Vứt vào 6 cục số lấy từ App (Google Auth/Authy), Hoặc 1 chuỗi ký tự Backup cứu hộ do hệ thống từng cấp.</summary>
    public string Code { get; set; } = string.Empty;
}

public class MfaChallengeCommandHandler : IRequestHandler<MfaChallengeCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaChallengeCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<bool> Handle(MfaChallengeCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        // Bọn rèm pha cấu hình CÒN CHƯA BẬT BẢO MẬT mà đã gõ cửa đòi Challenge? Get out.
        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
            throw new BadRequestException("Tài khoản chưa bật cấu hình MFA.");

        // 1. Phá giáp thuật toán băm nội bộ: Lấy Mật Ký (Secret Key) dạng thô từ Database (Đang lưu bị xáo trộn mã hoá AES hầm bà lằng nằng).
        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        
        // 2. Chấm Điểm 6 Con Số của Google Authenticator. (MfaService đã tính toán bù LAG thời gian chuẩn xác +- 30 giây).
        var isValid = _mfaService.VerifyCode(plainSecret, req.Code);
        
        // Pass ngon lành -> Qua Ải!
        if (isValid) return true;

        // 3. TIẾN TRÌNH RÀ SOÁT CỨU THƯƠNG: 
        // Lỡ Mảng Code Authenticator 6 Số ở trên Sai (Do mất điện thoại/ Hư máy), 
        // thì ta cho quét kiểm tra xem cục CHUỖI NHẬP KIA có phải là Mã Hỗ Trợ 1 Lần (Backup code) hay không!
        if (TryConsumeBackupCode(user, req.Code))
        {
            // THÀNH CÔNG RỒI => Phải lưu DB ngay lập tức để huỷ mạng Mã đó mãi mãi.  (Không cho xài đúp).
            await _userRepo.UpdateAsync(user, ct);
            return true;
        }

        // Đuổi khách.
        throw new BadRequestException("Mã MFA không chính xác hoặc đã hết hạn.");
    }

    /// <summary>
    /// Thuật Toán Tiêu Cháy Độc Đắc Nhất Lần (Burn-After-Read Backup Codes).
    /// </summary>
    private static bool TryConsumeBackupCode(Domain.Entities.User user, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(user.MfaBackupCodesHashJson))
            return false;

        List<string>? backupCodeHashes;
        try
        {
            // Hồ sơ Bệnh án: Hệ thống không Bao Giờ lưu nguyên si chữ Backup Code (Ngừa Hacker). 
            // Mà nó lưu mã Băm HASH của mã đó (Giống như Password thui nè).
            backupCodeHashes = JsonSerializer.Deserialize<List<string>>(user.MfaBackupCodesHashJson);
        }
        catch
        {
            return false;
        }

        if (backupCodeHashes == null || backupCodeHashes.Count == 0)
            return false;

        // Băm xào mật khẩu người mới Nhập (Để lát cầm Cân nén với Cục Hash trong DB).
        var hashedInput = HashBackupCode(code);
        
        // Dùng đòn soi gương thời gian (Ngửa bài tấn công Timing Attack).
        var matchedIndex = backupCodeHashes.FindIndex(hash => FixedTimeEquals(hash, hashedInput));
        if (matchedIndex < 0) return false;

        // TÌM RA THỦ PHẠM - SÚNG BÓP CÒ RỒI -> BỤP -> Gỡ Bỏ (Remove) cục hash đấy ra khỏi Danh Sách Túi Khôn.
        backupCodeHashes.RemoveAt(matchedIndex);
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        
        return true;
    }

    /// <summary>
    /// Dao thái thịt SHA-256 (Tương đương 1 tầng cửa an ninh ngân hàng cùi bắp, đủ dùng cho Backup Code ngẫu nhiên 16 Char).
    /// </summary>
    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// THUẬT TOÁN ĐỈNH CAO CHỐNG TIMING ATTACKS (Rò rỉ bằng cách bấm đồng hồ soi mili-giây xử lý của CPU).
    /// So sánh theo FixedTimeEquals giúp Server luôn mỉm cười tốn một thời lượng tĩnh y sì dập khuôn để quét xong Chuỗi A vs B. Không khai báo "Ngưng Cơm" sớm khi mới check chữ cái đầu.
    /// </summary>
    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
            && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
