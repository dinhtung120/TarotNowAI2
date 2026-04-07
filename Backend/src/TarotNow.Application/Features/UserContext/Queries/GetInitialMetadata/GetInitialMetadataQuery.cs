using MediatR;
using System;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

public record GetInitialMetadataQuery(Guid UserId) : IRequest<UserMetadataDto>;
