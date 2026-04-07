

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;

public class GetMfaStatusQuery : IRequest<GetMfaStatusResult>
{
    public Guid UserId { get; set; }
}

public class GetMfaStatusResult
{
        public bool MfaEnabled { get; set; }
}

public class GetMfaStatusQueryHandler : IRequestHandler<GetMfaStatusQuery, GetMfaStatusResult>
{
    private readonly IUserRepository _userRepository;

    public GetMfaStatusQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetMfaStatusResult> Handle(GetMfaStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        
        return new GetMfaStatusResult { MfaEnabled = user?.MfaEnabled ?? false };
    }
}
