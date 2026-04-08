using MediatR;
using System;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

/// <summary>
/// Command mua gói subscription cho người dùng.
/// Luồng xử lý: handler dùng dữ liệu command để kiểm tra gói/user, trừ ví diamond, tạo subscription và cấp entitlement bucket.
/// </summary>
/// <param name="UserId">Định danh user mua gói.</param>
/// <param name="PlanId">Định danh gói subscription cần mua.</param>
/// <param name="IdempotencyKey">Khóa idempotency để chống tạo giao dịch/mua gói trùng khi retry.</param>
public record SubscribeCommand(Guid UserId, Guid PlanId, string IdempotencyKey) : IRequest<Guid>;
