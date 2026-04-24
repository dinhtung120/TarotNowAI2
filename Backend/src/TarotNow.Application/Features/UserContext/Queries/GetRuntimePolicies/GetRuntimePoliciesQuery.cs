using MediatR;

namespace TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;

public sealed record GetRuntimePoliciesQuery : IRequest<RuntimePoliciesDto>;
