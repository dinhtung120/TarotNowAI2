using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

// Command xác thực MFA khi user đăng nhập bằng mã TOTP hoặc backup code.
public class MfaChallengeCommand : IRequest<bool>
{
    // Định danh user đang thực hiện bước challenge MFA.
    public Guid UserId { get; set; }

    // Mã MFA người dùng nhập (TOTP hoặc backup code).
    public string Code { get; set; } = string.Empty;
}

// Handler xử lý logic challenge MFA.
public class MfaChallengeCommandExecutor : ICommandExecutionExecutor<MfaChallengeCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    /// <summary>
    /// Khởi tạo handler challenge MFA.
    /// Luồng xử lý: nhận user repository để tải/cập nhật user và MFA service để verify mã.
    /// </summary>
    public MfaChallengeCommandExecutor(IUserRepository userRepo, IMfaService mfaService)
    {
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    /// <summary>
    /// Xử lý xác thực MFA cho user.
    /// Luồng xử lý: kiểm tra điều kiện MFA đã bật, ưu tiên verify TOTP, fallback sang backup code hợp lệ một lần.
    /// </summary>
    public async Task<bool> Handle(MfaChallengeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
        {
            // Edge case: tài khoản chưa hoàn tất cấu hình MFA thì không cho chạy challenge.
            throw new BadRequestException("Tài khoản chưa bật cấu hình MFA.");
        }

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        var isValid = _mfaService.VerifyCode(plainSecret, request.Code);
        // Ưu tiên xác thực theo TOTP để không tiêu hao backup code nếu không cần.

        if (isValid)
        {
            // TOTP hợp lệ thì kết thúc sớm vì không cần thay đổi trạng thái backup code.
            return true;
        }

        if (TryConsumeBackupCode(user, request.Code))
        {
            // Backup code hợp lệ phải được "đốt" ngay để đảm bảo mỗi mã chỉ dùng đúng một lần.
            await _userRepo.UpdateAsync(user, cancellationToken);
            // Cập nhật state user sau khi đã loại bỏ backup code vừa dùng.
            return true;
        }

        // Cả TOTP và backup code đều không hợp lệ thì từ chối challenge.
        throw new BadRequestException("Mã MFA không chính xác hoặc đã hết hạn.");
    }

    /// <summary>
    /// Thử tiêu thụ một backup code của user.
    /// Luồng xử lý: đọc danh sách hash backup code, so khớp mã đầu vào theo fixed-time compare, xóa mã trùng nếu tìm thấy.
    /// </summary>
    private static bool TryConsumeBackupCode(Domain.Entities.User user, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(user.MfaBackupCodesHashJson))
        {
            // Không có mã đầu vào hoặc user chưa có kho backup code thì không thể fallback.
            return false;
        }

        List<string>? backupCodeHashes;
        try
        {
            backupCodeHashes = JsonSerializer.Deserialize<List<string>>(user.MfaBackupCodesHashJson);
        }
        catch
        {
            // Edge case: dữ liệu JSON bị lỗi thì coi như không còn backup code hợp lệ.
            return false;
        }

        if (backupCodeHashes is null || backupCodeHashes.Count == 0)
        {
            // Không còn mã dự phòng khả dụng.
            return false;
        }

        var hashedInput = HashBackupCode(code);
        var matchedIndex = backupCodeHashes.FindIndex(hash => FixedTimeEquals(hash, hashedInput));
        // So khớp bằng fixed-time compare để giảm rủi ro timing attack khi đối chiếu hash.

        if (matchedIndex < 0)
        {
            // Không tìm thấy backup code trùng với input.
            return false;
        }

        backupCodeHashes.RemoveAt(matchedIndex);
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(backupCodeHashes);
        // Thay đổi state user: xóa mã đã dùng và lưu lại danh sách mới.

        return true;
    }

    /// <summary>
    /// Băm backup code bằng SHA-256 để so khớp với dữ liệu lưu trữ.
    /// Luồng xử lý: trim mã đầu vào, băm nhị phân rồi chuyển về chuỗi hex thường.
    /// </summary>
    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// So sánh hai chuỗi theo thời gian hằng để tránh lộ thông tin qua thời gian xử lý.
    /// Luồng xử lý: chuyển chuỗi sang byte và dùng CryptographicOperations.FixedTimeEquals khi độ dài khớp.
    /// </summary>
    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length &&
               CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
