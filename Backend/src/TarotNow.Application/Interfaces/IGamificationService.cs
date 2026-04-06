using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IGamificationService
{
    Task OnReadingCompletedAsync(Guid userId, CancellationToken ct);
    Task OnCheckInAsync(Guid userId, int currentStreak, CancellationToken ct);
    Task OnPostCreatedAsync(Guid userId, CancellationToken ct);
}
