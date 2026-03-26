using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public partial class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository, IWalletRepository walletRepository)
    {
        _userRepository = userRepository;
        _walletRepository = walletRepository;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác sửa user.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Người dùng không tồn tại.");

        UpdateRoleAndStatus(user, request);
        await _userRepository.UpdateAsync(user, cancellationToken);

        await AdjustBalancesAsync(user, request, cancellationToken);
        return true;
    }
}
