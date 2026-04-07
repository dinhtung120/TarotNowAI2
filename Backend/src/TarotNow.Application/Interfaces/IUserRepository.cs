

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IUserRepository
{
    
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

    
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    
    
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    
    
    Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);

    
    Task<IEnumerable<User>> SearchUsersByUsernameAsync(string usernamePart, CancellationToken cancellationToken = default);
    
    
    Task<Dictionary<Guid, string>> GetUsernameMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);

    
    Task<Dictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)>> GetUserBasicInfoMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
}
