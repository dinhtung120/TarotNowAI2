/*
 * ===================================================================
 * FILE: UpdateUserCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.UpdateUser
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command + Handler cho admin SỬA TOÀN DIỆN THÔNG TIN USER (Role, Status, Balances)
 *   Thay vì 2 lệnh riêng lẻ để cộng tiền và khóa, tạo 1 form gộp.
 * 
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

/// <summary>
/// Payload cập nhật User từ Admin. Gồm cả ví và chức vụ.
/// </summary>
public class UpdateUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
    // Role (admin, tarot_reader, user)
    public string Role { get; set; } = string.Empty;

    // Status (active, locked)
    public string Status { get; set; } = string.Empty;

    // Số dư Tuyệt Đối (Absolute Balance)
    public long DiamondBalance { get; set; }
    public long GoldBalance { get; set; }

    // Chống double click
    public string IdempotencyKey { get; set; } = string.Empty;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
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
        // 1. Kiểm tra User
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Người dùng không tồn tại.");

        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác sửa user.");

        // 2. Cập nhật Role
        string normalizedRole = request.Role?.Trim().ToLowerInvariant() ?? UserRole.User;
        if (user.Role != normalizedRole)
        {
            user.UpdateRole(normalizedRole);
        }

        // 3. Cập nhật Status
        string normalizedStatus = request.Status?.Trim().ToLowerInvariant() ?? UserStatus.Active;
        bool isActivelyLocked = user.Status == UserStatus.Locked;
        bool wantToLock = normalizedStatus == UserStatus.Locked;

        if (wantToLock && !isActivelyLocked)
        {
            user.Lock();
        }
        else if (!wantToLock && isActivelyLocked)
        {
            user.Activate(); 
        }

        // Lưu thông tin bảng Users
        await _userRepository.UpdateAsync(user);

        // 4. Xử lý Chênh Lệch Kim Cương (Delta calculations for Ledger strictly)
        long currentDiamond = user.DiamondBalance;
        long deltaDiamond = request.DiamondBalance - currentDiamond;
        if (deltaDiamond > 0)
        {
            await _walletRepository.CreditAsync(
                userId: user.Id,
                currency: CurrencyType.Diamond,
                type: TransactionType.AdminTopup, 
                amount: deltaDiamond,
                referenceSource: "Admin_Update_User",
                referenceId: request.IdempotencyKey,
                description: $"Admin adjusted diamond balance (+{deltaDiamond})",
                idempotencyKey: $"admin_update_d_credit_{request.IdempotencyKey}",
                cancellationToken: cancellationToken
            );
        }
        else if (deltaDiamond < 0)
        {
            await _walletRepository.DebitAsync(
                userId: user.Id,
                currency: CurrencyType.Diamond,
                type: TransactionType.AdminTopup, // Mượn Topup cho Adjustment
                amount: Math.Abs(deltaDiamond),
                referenceSource: "Admin_Update_User",
                referenceId: request.IdempotencyKey,
                description: $"Admin adjusted diamond balance ({deltaDiamond})",
                idempotencyKey: $"admin_update_d_debit_{request.IdempotencyKey}",
                cancellationToken: cancellationToken
            );
        }

        // 5. Xử lý Chênh Lệch Vàng (Delta)
        long currentGold = user.GoldBalance;
        long deltaGold = request.GoldBalance - currentGold;
        if (deltaGold > 0)
        {
            await _walletRepository.CreditAsync(
                userId: user.Id,
                currency: CurrencyType.Gold,
                type: TransactionType.AdminTopup,
                amount: deltaGold,
                referenceSource: "Admin_Update_User",
                referenceId: request.IdempotencyKey,
                description: $"Admin adjusted gold balance (+{deltaGold})",
                idempotencyKey: $"admin_update_g_credit_{request.IdempotencyKey}",
                cancellationToken: cancellationToken
            );
        }
        else if (deltaGold < 0)
        {
            await _walletRepository.DebitAsync(
                userId: user.Id,
                currency: CurrencyType.Gold,
                type: TransactionType.AdminTopup,
                amount: Math.Abs(deltaGold),
                referenceSource: "Admin_Update_User",
                referenceId: request.IdempotencyKey,
                description: $"Admin adjusted gold balance ({deltaGold})",
                idempotencyKey: $"admin_update_g_debit_{request.IdempotencyKey}",
                cancellationToken: cancellationToken
            );
        }

        return true;
    }
}
