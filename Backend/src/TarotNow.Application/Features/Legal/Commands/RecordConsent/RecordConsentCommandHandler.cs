/*
 * ===================================================================
 * FILE: RecordConsentCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Legal.Commands.RecordConsent
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thi hành ghi Sổ Sinh Nhai (PostgreSQL SQL Database) Lời Cam Kết Của User.
 *   
 * NOTE AN TOÀN:
 *   Hệ thống thiết kế theo cơ chế "Lưu Trữ Chống Đúp" (Idempotency thủ công).
 *   Nếu Khách hàng đã ấn đồng ý File Version này rồi lỡ xóa App cài lại, 
 *   bấm đồng ý lần 2 -> Server bỏ qua (Return True luôn), Không Tốn ổ cứng rác DB.
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public class RecordConsentCommandHandler : IRequestHandler<RecordConsentCommand, bool>
{
    private readonly IUserConsentRepository _consentRepository;

    public RecordConsentCommandHandler(IUserConsentRepository consentRepository)
    {
        _consentRepository = consentRepository;
    }

    public async Task<bool> Handle(RecordConsentCommand request, CancellationToken cancellationToken)
    {
        // 1. Quét Bụi Tờ Giấy Cũ: Kiểm tra xem User này đã từng Lấy Vân Tay vào Version này chưa?
        //    Ví dụ: TOS 1.2 đã Ký hôm qua. Hôm nay login trình duyệt Rác nó đòi Ký Lại -> Ta Chặn luôn!
        var existingConsent = await _consentRepository.GetConsentAsync(
            request.UserId, request.DocumentType, request.Version, cancellationToken);

        if (existingConsent != null)
        {
            return true; // Đã đồng ý rồi, bỏ qua, Ẩn mình đi (Silent Drop). Tránh Đúp Bộ Nhớ.
        }

        // 2. Lôi Sổ Đỏ ra Lập Kỷ Lục Mới.
        var newConsent = new UserConsent(
            request.UserId,
            request.DocumentType,
            request.Version,
            request.IpAddress,
            request.UserAgent
        );

        // Ném thẳng vào CSDL Kế toán của Server.
        await _consentRepository.AddAsync(newConsent, cancellationToken);

        return true;
    }
}
