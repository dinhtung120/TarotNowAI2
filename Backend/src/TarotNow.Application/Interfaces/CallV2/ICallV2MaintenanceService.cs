namespace TarotNow.Application.Interfaces;

public interface ICallV2MaintenanceService
{
    Task ProcessTimeoutsAsync(CancellationToken cancellationToken = default);
}
