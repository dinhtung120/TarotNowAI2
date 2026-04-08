using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

// Query lấy bộ sưu tập thẻ của người dùng.
public class GetUserCollectionQuery : IRequest<List<UserCollectionDto>>
{
    // Định danh user cần lấy collection.
    public Guid UserId { get; set; }
}

// DTO thông tin một lá thẻ trong bộ sưu tập người dùng.
public class UserCollectionDto
{
    // Định danh lá bài.
    public int CardId { get; set; }

    // Cấp hiện tại của lá bài trong collection.
    public int Level { get; set; }

    // Tổng exp đã tích lũy cho lá bài.
    public long ExpGained { get; set; }

    // Thời điểm gần nhất lá bài được rút.
    public DateTime LastDrawnAt { get; set; }

    // Số bản sao lá bài user đang sở hữu.
    public int Copies { get; set; }

    // Chỉ số tấn công của lá bài.
    public int Atk { get; set; }

    // Chỉ số phòng thủ của lá bài.
    public int Def { get; set; }
}
