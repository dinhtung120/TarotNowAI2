using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

// Handler cập nhật thông tin tài khoản và cân bằng ví theo giá trị mục tiêu admin nhập.
public partial class UpdateUserCommandExecutor : ICommandExecutionExecutor<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler update user.
    /// Luồng xử lý: nhận user repository để cập nhật hồ sơ và wallet repository để điều chỉnh số dư.
    /// </summary>
    public UpdateUserCommandExecutor(
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command cập nhật user.
    /// Luồng xử lý: kiểm tra idempotency key, tải user, cập nhật role/status, lưu user, rồi điều chỉnh số dư.
    /// </summary>
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            // Rule bắt buộc idempotency cho thao tác chỉnh số dư để ngăn replay ngoài ý muốn.
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác sửa user.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Người dùng không tồn tại.");

        // Cập nhật role/status trước để trạng thái tài khoản phản ánh đúng quyết định admin.
        UpdateRoleAndStatus(user, request);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Điều chỉnh số dư sau khi user đã được persist để đảm bảo luồng audit nhất quán.
        await AdjustBalancesAsync(user, request, cancellationToken);
        return true;
    }
}
