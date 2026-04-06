using MediatR;
using System;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

/// <summary>
/// Cú nháy fetch toàn bộ metadata quan trọng cho DashBoard (Batching Request).
/// </summary>
public record GetInitialMetadataQuery(Guid UserId) : IRequest<UserMetadataDto>;
