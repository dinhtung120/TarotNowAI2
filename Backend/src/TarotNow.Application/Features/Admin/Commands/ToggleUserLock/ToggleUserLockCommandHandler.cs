/*
 * ===================================================================
 * FILE: ToggleUserLockCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ToggleUserLock
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý logic khóa/mở khóa tài khoản user.
 *   Logic TỐI GIẢN: tìm user → gọi domain method → lưu.
 *
 * DOMAIN-DRIVEN DESIGN:
 *   Handler KHÔNG trực tiếp thay đổi user.IsLocked = true.
 *   Thay vào đó gọi user.Lock() / user.Unlock() (domain methods).
 *   Domain entity quản lý business rules:
 *   - Không thể khóa admin
 *   - Ghi nhận thời điểm khóa
 *   - Có thể trigger domain events
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

/// <summary>Handler xử lý khóa/mở khóa tài khoản.</summary>
public class ToggleUserLockCommandHandler : IRequestHandler<ToggleUserLockCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ToggleUserLockCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ToggleUserLockCommand request, CancellationToken cancellationToken)
    {
        // Tìm user — throw NotFoundException nếu không tìm thấy
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User not found.");

        /*
         * Gọi domain method thay vì set property trực tiếp.
         * user.Lock(): đánh dấu IsLocked = true, ghi LockedAt = now
         * user.Unlock(): đánh dấu IsLocked = false, xóa LockedAt
         * 
         * Tại sao dùng domain method?
         * 1. Encapsulation: logic nằm trong entity, không rải rác
         * 2. Invariants: entity kiểm tra rules (VD: không khóa admin)
         * 3. Side effects: có thể trigger domain events
         */
        if (request.Lock)
            user.Lock();
        else
            user.Unlock();
        
        // Lưu thay đổi vào PostgreSQL
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
