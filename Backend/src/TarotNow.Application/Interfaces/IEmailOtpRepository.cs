
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý OTP email để đảm bảo xác thực theo mã dùng một lần.
public interface IEmailOtpRepository
{
    /// <summary>
    /// Thêm OTP mới khi khởi tạo luồng xác thực qua email.
    /// Luồng xử lý: persist entity OTP vào kho dữ liệu để phục vụ bước verify.
    /// </summary>
    Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy OTP active mới nhất của người dùng theo loại nghiệp vụ để kiểm tra mã nhập vào.
    /// Luồng xử lý: lọc theo userId/type và trả bản ghi còn hiệu lực gần nhất.
    /// </summary>
    Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái OTP sau khi dùng, hết hạn hoặc bị vô hiệu hóa.
    /// Luồng xử lý: ghi thay đổi của entity OTP tương ứng vào dữ liệu bền vững.
    /// </summary>
    Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default);
}
