using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
