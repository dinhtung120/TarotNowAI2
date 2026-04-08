using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

// Handler ghi nhận consent pháp lý theo nguyên tắc idempotent.
public class RecordConsentCommandHandler : IRequestHandler<RecordConsentCommand, bool>
{
    private readonly IUserConsentRepository _consentRepository;

    /// <summary>
    /// Khởi tạo handler để xử lý ghi nhận consent.
    /// Luồng xử lý: nhận repository để kiểm tra consent hiện có và lưu consent mới khi cần.
    /// </summary>
    public RecordConsentCommandHandler(IUserConsentRepository consentRepository)
    {
        _consentRepository = consentRepository;
    }

    /// <summary>
    /// Ghi nhận consent cho một tài liệu pháp lý.
    /// Luồng xử lý: kiểm tra consent đã tồn tại chưa để tránh trùng lặp, sau đó tạo bản ghi mới khi chưa có.
    /// </summary>
    public async Task<bool> Handle(RecordConsentCommand request, CancellationToken cancellationToken)
    {
        var existingConsent = await _consentRepository.GetConsentAsync(
            request.UserId,
            request.DocumentType,
            request.Version,
            cancellationToken);

        if (existingConsent is not null)
        {
            // Consent đã tồn tại thì trả thành công ngay để đảm bảo idempotent khi client gửi lặp.
            return true;
        }

        var newConsent = new UserConsent(
            request.UserId,
            request.DocumentType,
            request.Version,
            request.IpAddress,
            request.UserAgent);
        // Tạo entity mới vì chưa có bản ghi phù hợp cho cùng user + document + version.

        await _consentRepository.AddAsync(newConsent, cancellationToken);
        // Ghi nhận state mới vào persistence để phục vụ kiểm tra consent ở các luồng sau.

        return true;
    }
}
