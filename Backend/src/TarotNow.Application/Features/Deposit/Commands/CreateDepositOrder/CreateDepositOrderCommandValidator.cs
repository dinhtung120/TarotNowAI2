/*
 * ===================================================================
 * FILE: CreateDepositOrderCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder
 * ===================================================================
 * MỤC ĐÍCH:
 *   Chặn rác dữ liệu ở đầu vào. Bảo vệ an toàn toán học cho tính năng Nạp Tiền.
 *   
 * QUY TẮC BIZ:
 *   1. Không được nạp âm (-100,000).
 *   2. Không được nạp lẻ tẻ (phải tròn trục nghìn vì tỉ giá sàn là 1k VNĐ = 1 Diamond).
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

/// <summary>
/// Validator cho Request Nạp mệnh giá vào Nền tảng.
/// </summary>
public class CreateDepositOrderCommandValidator : AbstractValidator<CreateDepositOrderCommand>
{
    public CreateDepositOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.AmountVnd)
            // Ngăn chặn Hacker gửi gói API -1,000,000 VND để làm crash hệ thống ví.
            .GreaterThan(0).WithMessage("AmountVnd must be greater than 0.")
            
            // Ép ràng buộc bội số nghìn: Tránh tình trạng nạp 1,234 VNĐ -> Không chia hết cho 1,000 -> Đẻ ra số thập phân.
            .Must(amount => amount % 1000 == 0).WithMessage("AmountVnd must be a multiple of 1,000.");
    }
}
