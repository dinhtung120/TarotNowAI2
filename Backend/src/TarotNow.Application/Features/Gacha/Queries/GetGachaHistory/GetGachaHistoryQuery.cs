using MediatR;
using System;
using System.Collections.Generic;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

public class GetGachaHistoryQuery : IRequest<List<GachaHistoryItemDto>>
{
    public Guid UserId { get; set; }
    public int Limit { get; set; } = 50;

    public GetGachaHistoryQuery(Guid userId, int limit = 50)
    {
        UserId = userId;
        Limit = limit;
    }
}
