

namespace TarotNow.Application.Features.Auth.Commands.Register;

// DTO phản hồi sau khi đăng ký thành công.
public class RegisterResponse
{
    // Định danh user vừa được tạo.
    public Guid UserId { get; set; }

    // Thông điệp phản hồi cho client.
    public string Message { get; set; } = string.Empty;
}
