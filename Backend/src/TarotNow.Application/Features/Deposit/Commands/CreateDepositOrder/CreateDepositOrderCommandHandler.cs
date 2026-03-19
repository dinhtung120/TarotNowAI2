/*
 * ===================================================================
 * FILE: CreateDepositOrderCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trung tâm tính toán Tỉ Giá Nạp Tiền và Khuyến Mãi (Promotions).
 *
 * LUỒNG TÍNH TOÁN:
 *   1. Chuyển đổi Vnd -> Diamond (Tỉ giá cứng: 1,000 VNĐ = 1 Diamond)
 *   2. Thuật toán quét Khuyến mãi (Auto-apply Promotion):
 *      Nếu mốc nạp đạt chỉ tiêu tối thiểu của sự kiện, hệ thống tự động
 *      cộng thêm Diamond Bonus (Tạo mồi nhử Gamification ép nạp nhiều hơn).
 *   3. Ghi lại Hoá Đơn Trạng thái Pending vào Database PostgreSQL.
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Constants;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

/// <summary>
/// Handler xử lý nghiệp vụ Tạo Hoá Đơn nạp tiền.
/// </summary>
public class CreateDepositOrderCommandHandler : IRequestHandler<CreateDepositOrderCommand, CreateDepositOrderResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IDepositPromotionRepository _promotionRepository;

    public CreateDepositOrderCommandHandler(IDepositOrderRepository depositOrderRepository, IDepositPromotionRepository promotionRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _promotionRepository = promotionRepository;
    }

    public async Task<CreateDepositOrderResponse> Handle(CreateDepositOrderCommand request, CancellationToken cancellationToken)
    {
        // --------------------------------------------------------------------
        // BƯỚC 1: QUY ĐỔI TIỀN SỞ TẠI (VNĐ) SANG TIỀN NỀN TẢNG (DIAMOND)
        // VD: 100,000 VNĐ / 1,000 = 100 Diamond Gốc
        // --------------------------------------------------------------------
        long baseDiamondAmount = request.AmountVnd / EconomyConstants.VndPerDiamond;
        
        // --------------------------------------------------------------------
        // BƯỚC 2: AUTOMATIC PROMOTION (P1-PROMO-BE-1.2)
        // Hệ thống sẽ lấy tất cả Khuyến mãi Đang diễn ra (Active) từ CSDL.
        // Danh sách này ĐÃ ĐƯỢC CHUẨN HOÁ SORT DESCENDING (Mức nạp cao nhất đứng đầu).
        // --------------------------------------------------------------------
        var activePromotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        
        long bonusDiamond = 0;
        foreach (var promo in activePromotions)
        {
            // Do đã sắp xếp từ Lớn tới bé, nên Khuyến Mãi đầu tiên nào thoả điều kiện 
            // chính là khuyến mãi Cao Nhất và Tốt Nhất cho khách hàng.
            if (request.AmountVnd >= promo.MinAmountVnd)
            {
                bonusDiamond = promo.BonusDiamond;
                break; // Tìm thấy thì dừng vòng lặp (Tham lam - Greedy Algorithm).
            }
        }

        long totalDiamondAmount = baseDiamondAmount + bonusDiamond;

        // --------------------------------------------------------------------
        // BƯỚC 3: IN HOÁ ĐƠN (LƯU DATABASE)
        // Trạng thái tự động sinh ra trong Class DepositOrder là "Pending".
        // Tiền CHƯA được cộng vào ví ngay lúc này. Phải chờ Webhook của VNPay gọi lại.
        // --------------------------------------------------------------------
        var order = new DepositOrder(request.UserId, request.AmountVnd, totalDiamondAmount);

        await _depositOrderRepository.AddAsync(order, cancellationToken);

        return new CreateDepositOrderResponse
        {
            OrderId = order.Id,
            AmountVnd = order.AmountVnd,
            DiamondAmount = order.DiamondAmount
        };
    }
}
