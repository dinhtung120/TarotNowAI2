using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

// Handler thay đổi trạng thái khóa của tài khoản người dùng.
public class ToggleUserLockCommandHandler : IRequestHandler<ToggleUserLockCommand, bool>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler toggle lock user.
    /// Luồng xử lý: nhận user repository để tải và cập nhật trạng thái tài khoản.
    /// </summary>
    public ToggleUserLockCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý command khóa/mở khóa người dùng.
    /// Luồng xử lý: tải user theo id, rẽ nhánh lock/unlock, rồi lưu lại thay đổi.
    /// </summary>
    public async Task<bool> Handle(ToggleUserLockCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User not found.");

        if (request.Lock)
        {
            // Nhánh lock: chuyển tài khoản sang trạng thái bị khóa.
            user.Lock();
        }
        else
        {
            // Nhánh unlock: mở lại tài khoản để user hoạt động bình thường.
            user.Unlock();
        }

        // Persist thay đổi trạng thái khóa sau khi áp dụng rule.
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
