using MediatR;
using System;
using System.Collections.Generic;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

// Query lấy lịch sử quay gacha của user.
public class GetGachaHistoryQuery : IRequest<List<GachaHistoryItemDto>>
{
    // Định danh user cần lấy lịch sử.
    public Guid UserId { get; set; }

    // Số bản ghi tối đa trả về.
    public int Limit { get; set; } = 50;

    /// <summary>
    /// Khởi tạo query lấy lịch sử gacha.
    /// Luồng xử lý: gán UserId và Limit theo đầu vào.
    /// </summary>
    public GetGachaHistoryQuery(Guid userId, int limit = 50)
    {
        UserId = userId;
        Limit = limit;
    }
}
