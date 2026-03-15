using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public class RecordConsentCommandHandler : IRequestHandler<RecordConsentCommand, bool>
{
    private readonly IUserConsentRepository _consentRepository;
    private readonly IUserRepository _userRepository;

    public RecordConsentCommandHandler(IUserConsentRepository consentRepository, IUserRepository userRepository)
    {
        _consentRepository = consentRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(RecordConsentCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra xem user này đã đồng ý version này của document này chưa
        var existingConsent = await _consentRepository.GetConsentAsync(
            request.UserId, request.DocumentType, request.Version, cancellationToken);

        if (existingConsent != null)
        {
            return true; // Đã đồng ý rồi, bỏ qua
        }

        // 2. Tạo record mới
        var newConsent = new UserConsent(
            request.UserId,
            request.DocumentType,
            request.Version,
            request.IpAddress,
            request.UserAgent
        );

        await _consentRepository.AddAsync(newConsent, cancellationToken);

        // 3. Nếu document type là TOS, cập nhật flag HasConsented cho User
        if (request.DocumentType == "TOS")
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            // Hiện tại User entity không có SetConsented method public nên tạm thời
            // User entities in our domain only accept update through methods if properties have private setters.
            // Wait, we can't easily set HasConsented if it has a private setter and no method. 
            // We should add a MarkAsConsented method to User! I will edit the User entity.
            if (user != null && !user.HasConsented)
            {
               user.MarkAsConsented();
               await _userRepository.UpdateAsync(user);
            }
        }

        return true;
    }
}
