using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

// Command cập nhật role/status và số dư đích của người dùng từ màn quản trị.
public class UpdateUserCommand : IRequest<bool>
{
    // Định danh người dùng cần cập nhật.
    public Guid UserId { get; set; }

    // Vai trò mong muốn sau cập nhật.
    public string Role { get; set; } = string.Empty;

    // Trạng thái mong muốn sau cập nhật (active/locked...).
    public string Status { get; set; } = string.Empty;

    // Số dư kim cương đích sau cập nhật.
    public long DiamondBalance { get; set; }

    // Số dư vàng đích sau cập nhật.
    public long GoldBalance { get; set; }

    // Khóa idempotency bắt buộc để chống ghi đè số dư lặp.
    public string IdempotencyKey { get; set; } = string.Empty;
}
