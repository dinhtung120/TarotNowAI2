using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class DepositOrderRepository
{
    private const string ClientRequestKeyUniqueConstraintName = "ix_deposit_orders_client_request_key";

    /// <summary>
    /// Tạo đơn nạp mới hoặc trả về đơn đã tồn tại theo client request key khi đụng race idempotency.
    /// </summary>
    public async Task<DepositOrder> AddOrGetExistingByClientRequestKeyAsync(
        DepositOrder order,
        string clientRequestKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (string.IsNullOrWhiteSpace(clientRequestKey))
        {
            throw new ArgumentException("Client request key is required.", nameof(clientRequestKey));
        }

        try
        {
            await AddAsync(order, cancellationToken);
            return order;
        }
        catch (DbUpdateException exception) when (IsClientRequestKeyConflict(exception))
        {
            return await ResolveIdempotentCollisionAsync(order, clientRequestKey, cancellationToken);
        }
        catch (InvalidOperationException exception) when (IsSerializationFailure(exception))
        {
            return await ResolveIdempotentCollisionAsync(order, clientRequestKey, cancellationToken);
        }
    }

    private async Task<DepositOrder> ResolveIdempotentCollisionAsync(
        DepositOrder incomingOrder,
        string clientRequestKey,
        CancellationToken cancellationToken)
    {
        var existingOrder = await GetByClientRequestKeyUsingFreshContextAsync(clientRequestKey, cancellationToken);
        if (existingOrder == null)
        {
            throw new InvalidOperationException("Cannot resolve idempotent deposit order collision.");
        }

        DetachIfTracked(incomingOrder);
        return existingOrder;
    }

    private static bool IsClientRequestKeyUniqueViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   ClientRequestKeyUniqueConstraintName,
                   StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsSerializationFailure(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.SerializationFailure;
    }

    private static bool IsSerializationFailure(InvalidOperationException exception)
    {
        return exception.InnerException is DbUpdateException dbUpdateException
               && IsSerializationFailure(dbUpdateException);
    }

    private static bool IsClientRequestKeyConflict(DbUpdateException exception)
    {
        return IsClientRequestKeyUniqueViolation(exception) || IsSerializationFailure(exception);
    }

    private async Task<DepositOrder?> GetByClientRequestKeyUsingFreshContextAsync(
        string clientRequestKey,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var freshContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await freshContext.DepositOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.ClientRequestKey == clientRequestKey, cancellationToken);
    }

    private void DetachIfTracked(DepositOrder order)
    {
        var entry = _context.Entry(order);
        if (entry.State != EntityState.Detached)
        {
            entry.State = EntityState.Detached;
        }
    }
}
