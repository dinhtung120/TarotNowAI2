using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;

// Query lấy trạng thái MFA hiện tại của user.
public class GetMfaStatusQuery : IRequest<GetMfaStatusResult>
{
    // Định danh user cần kiểm tra trạng thái MFA.
    public Guid UserId { get; set; }
}

// DTO trạng thái MFA trả về cho client.
public class GetMfaStatusResult
{
    // Cờ cho biết user đã bật MFA hay chưa.
    public bool MfaEnabled { get; set; }
}

// Handler truy vấn trạng thái MFA.
public class GetMfaStatusQueryHandler : IRequestHandler<GetMfaStatusQuery, GetMfaStatusResult>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler lấy trạng thái MFA.
    /// Luồng xử lý: nhận user repository để tra cứu cờ MFA theo user id.
    /// </summary>
    public GetMfaStatusQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query lấy trạng thái MFA.
    /// Luồng xử lý: đọc user theo id và trả false khi không tồn tại để caller xử lý an toàn ở tầng trên.
    /// </summary>
    public async Task<GetMfaStatusResult> Handle(GetMfaStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        return new GetMfaStatusResult
        {
            // Edge case: user không tồn tại thì coi như chưa bật MFA.
            MfaEnabled = user?.MfaEnabled ?? false
        };
    }
}
