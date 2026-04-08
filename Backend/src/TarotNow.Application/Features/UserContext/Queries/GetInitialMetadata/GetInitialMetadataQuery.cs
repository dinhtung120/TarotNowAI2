using MediatR;
using System;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

/// <summary>
/// Query lấy metadata khởi tạo cho màn hình chính của user.
/// Luồng xử lý: handler dùng UserId để tổng hợp wallet, streak, thông báo và hội thoại hoạt động trong một response.
/// </summary>
/// <param name="UserId">Định danh user cần lấy metadata.</param>
public record GetInitialMetadataQuery(Guid UserId) : IRequest<UserMetadataDto>;
