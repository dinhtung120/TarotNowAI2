using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

// Command xóa một promotion theo định danh.
public class DeletePromotionCommand : IRequest<bool>
{
    // Định danh promotion cần xóa.
    public Guid Id { get; set; }
}
