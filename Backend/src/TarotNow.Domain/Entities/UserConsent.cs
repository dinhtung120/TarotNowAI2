

using System;

namespace TarotNow.Domain.Entities;

public class UserConsent
{
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
        public string DocumentType { get; private set; } = string.Empty;
    
        public string Version { get; private set; } = string.Empty;
    
        public DateTime ConsentedAt { get; private set; }
    
    
    public string IpAddress { get; private set; } = string.Empty;
    
    public string UserAgent { get; private set; } = string.Empty;

    
    public User User { get; private set; } = null!;

    protected UserConsent() { } 

        public UserConsent(Guid userId, string documentType, string version, string ipAddress, string userAgent)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        DocumentType = documentType;
        Version = version;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        ConsentedAt = DateTime.UtcNow;
    }
}
