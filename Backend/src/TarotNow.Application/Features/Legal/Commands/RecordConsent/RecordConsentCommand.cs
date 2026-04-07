

using MediatR;
using System;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public class RecordConsentCommand : IRequest<bool>
{
        public Guid UserId { get; set; }
    
        public string DocumentType { get; set; } = string.Empty;
    
        public string Version { get; set; } = string.Empty;
    
    
    
    
    
        public string IpAddress { get; set; } = string.Empty;
    
        public string UserAgent { get; set; } = string.Empty;
}
