using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

// Handler thay đổi trạng thái khóa của tài khoản người dùng.
public class ToggleUserLockCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ToggleUserLockCommandHandlerRequestedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;

    /// <summary>
    /// Khởi tạo handler toggle lock user.
    /// Luồng xử lý: nhận user repository để tải và cập nhật trạng thái tài khoản.
    /// </summary>
    public ToggleUserLockCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
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
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId, cancellationToken);
            await _authSessionRepository.RevokeAllByUserAsync(request.UserId, cancellationToken);
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

    protected override async Task HandleDomainEventAsync(
        ToggleUserLockCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
