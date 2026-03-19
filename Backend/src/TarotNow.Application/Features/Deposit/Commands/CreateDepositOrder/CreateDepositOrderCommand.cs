/*
 * ===================================================================
 * FILE: CreateDepositOrderCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh kích hoạt yêu cầu "Nạp Tiền Vào Tài Khoản" (Deposit).
 *   Hệ thống TarotNow dùng đơn vị tiền ảo (Diamond) để thanh toán dịch vụ.
 *
 * QUY TRÌNH NẠP:
 *   User nhập số tiền VNĐ -> API tính toán số Diamond nhận được (Bao gồm khuyến mãi) 
 *   -> Sinh ra dòng OrderId chờ thanh toán qua cổng VNPay/MoMo.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

/// <summary>
/// Yêu cầu cung cấp số tiền VNĐ muốn nạp.
/// </summary>
public class CreateDepositOrderCommand : IRequest<CreateDepositOrderResponse>
{
    public Guid UserId { get; set; }
    
    /// <summary>Số lượng tiền Việt Nam Đồng mà User muốn thanh toán.</summary>
    public long AmountVnd { get; set; }
}

/// <summary>
/// DTO chứa hoá đơn thanh toán vừa tạo.
/// Front-end sẽ dùng OrderId này chèn vào Link VNPay để chuyển trang sang Ngân hàng.
/// </summary>
public class CreateDepositOrderResponse
{
    /// <summary>Mã số phiếu thu (Cần thiết cho quá trình đối soát sau này).</summary>
    public Guid OrderId { get; set; }
    
    public long AmountVnd { get; set; }
    
    /// <summary>Số lượng Kim Cương (Diamond) mà User sẽ nhận được sau khi thanh toán Thành Công.</summary>
    public long DiamondAmount { get; set; }
}
