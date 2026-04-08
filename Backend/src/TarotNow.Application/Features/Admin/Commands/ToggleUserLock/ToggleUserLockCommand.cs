using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

// Command khóa hoặc mở khóa tài khoản người dùng.
public class ToggleUserLockCommand : IRequest<bool>
{
    // Định danh tài khoản cần thay đổi trạng thái khóa.
    public Guid UserId { get; set; }

    // Cờ true để khóa, false để mở khóa.
    public bool Lock { get; set; }
}
