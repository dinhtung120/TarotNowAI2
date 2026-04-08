using MediatR;
using System;
using System.Collections.Generic;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements;

/// <summary>
/// Query lấy toàn bộ số dư entitlement hiện tại của user.
/// Luồng xử lý: handler dùng UserId để truy vấn entitlement service và trả danh sách balance theo từng entitlement key.
/// </summary>
/// <param name="UserId">Định danh user cần lấy entitlement balance.</param>
public record GetMyEntitlementsQuery(Guid UserId) : IRequest<List<EntitlementBalanceDto>>;
