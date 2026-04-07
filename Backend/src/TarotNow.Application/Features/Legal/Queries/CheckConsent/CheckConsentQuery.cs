

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

public class CheckConsentQuery : IRequest<CheckConsentResponse>
{
    public Guid UserId { get; set; }
    
    
    public string? DocumentType { get; set; }
    public string? Version { get; set; }
}

public class CheckConsentResponse
{
        public bool IsFullyConsented { get; set; }
    
        public List<string> PendingDocuments { get; set; } = new();
}
