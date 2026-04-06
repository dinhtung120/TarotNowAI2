/*
 * ===================================================================
 * FILE: SubscribeCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.Subscribe
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh (Command) mang thông tin Khách muốn Mua một Lớp Đặc Quyền (Subscription Plan).
 * ===================================================================
 */

using System;
using MediatR;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

/// <summary>
/// Thư Lệnh Yêu Cầu Mở Khóa Gói Cước Mới.
/// Trả về ID của Biên lai Nhận Đăng Ký (UserSubscriptionId).
/// </summary>
public record SubscribeCommand(Guid UserId, Guid PlanId, string IdempotencyKey) : IRequest<Guid>;
