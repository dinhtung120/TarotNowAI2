

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
        
        
        var existingConsent = await _consentRepository.GetConsentAsync(
            request.UserId, request.DocumentType, request.Version, cancellationToken);

        if (existingConsent != null)
        {
            return true; 
        }

        
        var newConsent = new UserConsent(
            request.UserId,
            request.DocumentType,
            request.Version,
            request.IpAddress,
            request.UserAgent
        );

        
        await _consentRepository.AddAsync(newConsent, cancellationToken);

        return true;
    }
}
