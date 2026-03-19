/*
 * ===================================================================
 * FILE: DeletePromotionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.DeletePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xóa bỏ một Sự kiện Khuyến Mãi ra khỏi Hệ Thống. (Hành vi của Admin).
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommand : IRequest<bool>
{
    /// <summary>Truyền sang cái ID của Gói Khuyến Mãi lỡ tạo sai để Trảm nó.</summary>
    public Guid Id { get; set; }
}
