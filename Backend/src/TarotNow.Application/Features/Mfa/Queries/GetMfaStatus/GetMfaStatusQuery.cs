/*
 * ===================================================================
 * FILE: GetMfaStatusQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Mfa.Queries.GetMfaStatus
 * ===================================================================
 * MỤC ĐÍCH:
 *   API cực kỳ đơn giản để Frontend (App/Web) hỏi xem "Chủ nhân đã bật Khoá 2 lớp chưa?".
 *   Từ đó UI (Giao diện) sẽ vẽ ra nút [ Bật MFA ] hay nút [ Tắt MFA ] cho phù hợp.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;

/// <summary>
/// Gói Lệnh: Đòi xem Trạng Thái Cửa Nẻo MFA của Nhà mình.
/// </summary>
public class GetMfaStatusQuery : IRequest<GetMfaStatusResult>
{
    public Guid UserId { get; set; }
}

public class GetMfaStatusResult
{
    /// <summary>True = Đã Bật (Bất khả xâm phạm), False = Trống Huơ Trống Hoác.</summary>
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
        
        // Trả Về Đúng Trạng Thái Đang Lưu Khắc Trong SQL.
        return new GetMfaStatusResult { MfaEnabled = user?.MfaEnabled ?? false };
    }
}
