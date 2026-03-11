using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class UserConsentRepository : IUserConsentRepository
{
    private readonly ApplicationDbContext _context;

    public UserConsentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserConsent?> GetConsentAsync(Guid userId, string documentType, string version, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .FirstOrDefaultAsync(c => c.UserId == userId && c.DocumentType == documentType && c.Version == version, cancellationToken);
    }

    public async Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserConsents
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserConsent consent, CancellationToken cancellationToken = default)
    {
        await _context.UserConsents.AddAsync(consent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
