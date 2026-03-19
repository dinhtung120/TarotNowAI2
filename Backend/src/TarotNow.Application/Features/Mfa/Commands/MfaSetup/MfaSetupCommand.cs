/*
 * ===================================================================
 * FILE: MfaSetupCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Mfa.Commands.MfaSetup
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Khởi tạo Chìa Khóa Bảo Mật 2 Lớp (MFA Step 1).
 *
 * LUỒNG NGHIỆP VỤ:
 *   1. User bấm nút "Bật MFA" trên giao diện.
 *   2. Server Sinh ra 1 Mã Bí Mật Độc Nhất (Secret Key) + Link tạo mã QR + 10 Mã Dự Phòng.
 *   3. LƯU Ý: Lúc này MFA vẫn CHƯA ĐƯỢC BẬT (MfaEnabled = False). 
 *      Nó chỉ mới lưu nháp Chìa Khóa vào Database thui.
 *   4. Đợi User lấy Điện Thoại quẹt mã QR, nhập đúng 6 số lên Web (sang lệnh Verify) thì mới Chính Thức Khóa Cửa.
 * ===================================================================
 */

using MediatR;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

/// <summary>
/// Xin Lệnh Cấp Chìa Khóa Két Sắt (Setup 2FA).
/// </summary>
public class MfaSetupCommand : IRequest<MfaSetupResult>
{
    public Guid UserId { get; set; }
}

public class MfaSetupResult
{
    /// <summary>Truyền về cục String URL Định dạng `otpauth://totp/...` để Frontend dùng thư viện vẽ ra Hình Vuông QR Code cho Nhâm nhi Quét mã.</summary>
    public string QrCodeUri { get; set; } = string.Empty;
    
    /// <summary>Chữ thô để user Copy/Paste phòng trường hợp Camera điện thoại bị hỏng không quét QR được.</summary>
    public string SecretDisplay { get; set; } = string.Empty;
    
    /// <summary>Danh sách 10 mã phao cứu sinh (Backup Codes) đưa cho User bắt chép ra Sổ Tay.</summary>
    public List<string> BackupCodes { get; set; } = new();
}

public class MfaSetupCommandHandler : IRequestHandler<MfaSetupCommand, MfaSetupResult>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public MfaSetupCommandHandler(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<MfaSetupResult> Handle(MfaSetupCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("User not found.");

        // Khóa cửa 2 lần là Rớt Bản Lề - Bắt buộc tắt đi rồi mới được tạo lại mới.
        if (user.MfaEnabled)
            throw new BadRequestException("MFA đã được bật. Nếu muốn thiết lập lại, vui lòng tắt MFA trước.");

        // ==========================================
        //  XƯỞNG ĐÚC VÀ RÈN VŨ KHÍ BẢO MẬT
        // ==========================================
        // 1. Phôi gốc 16 Ký tự Base32 (Ví Dụ: JBSWY3DPEHPK3PXP).
        var plainSecret = _mfaService.GenerateSecretKey();
        
        // 2. Nhét phôi vô tủ bọc Kẽm gai AES256 (Phòng khi Hacker chôm Database cũng không rặn ra được Pass của Khách).
        var encryptedSecret = _mfaService.EncryptSecret(plainSecret);
        
        // 3. Đắp tên User vào để App Google Nhận Diện ra Tiệm TarotNow (TarotNow: email@gmail.com).
        var qrUri = _mfaService.GenerateQrCodeUri(plainSecret, user.Email);
        
        // 4. Sinh Sinh Sôi Nảy Nở 10 Ký Tự Dự Phòng.
        var backupCodes = _mfaService.GenerateBackupCodes();
        
        // 5. Đem băm vằm 10 Ký tự này thành Bột Hash Ngay Và Luôn (Để lát lưu DB, Không Save Bản Text).
        var backupCodeHashes = backupCodes.Select(HashBackupCode).ToList();

        // Ép xác vào kho Khách Hàng. (Ghi chú: Giữ y MfaEnabled=False để Cửa Vẫn Chưa Sập lại).
        user.MfaSecretEncrypted = encryptedSecret;
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        
        await _userRepo.UpdateAsync(user, ct);

        // Ném Thí điểm về UI cho Frontend trang trí.
        return new MfaSetupResult
        {
            QrCodeUri = qrUri,
            SecretDisplay = plainSecret,
            BackupCodes = backupCodes
        };
    }

    /// <summary>
    /// Dao Tẩu Hoả Mã Hoá SHA-256 (Chống mất bò mới lo làm chuồng).
    /// </summary>
    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
