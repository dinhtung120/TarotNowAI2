using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed class WithdrawalDataEncryptionBackfillHostedService : BackgroundService
{
    private const int BatchSize = 200;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<SecurityOptions> _securityOptions;
    private readonly ILogger<WithdrawalDataEncryptionBackfillHostedService> _logger;

    public WithdrawalDataEncryptionBackfillHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<SecurityOptions> securityOptions,
        ILogger<WithdrawalDataEncryptionBackfillHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _securityOptions = securityOptions;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_securityOptions.Value.EnableWithdrawalEncryptionBackfill == false)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var processed = await ProcessOneBatchAsync(stoppingToken);
            if (processed == 0)
            {
                break;
            }
        }
    }

    private async Task<int> ProcessOneBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var candidateIds = await LoadUnencryptedCandidateIdsAsync(dbContext, cancellationToken);
        if (candidateIds.Count == 0)
        {
            return 0;
        }

        var records = await dbContext.WithdrawalRequests
            .Where(x => candidateIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        foreach (var record in records)
        {
            TouchSensitiveFieldsForEncryption(dbContext, record);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("[WithdrawalBackfill] Encrypted {Count} withdrawal records.", records.Count);
        return records.Count;
    }

    private static Task<List<Guid>> LoadUnencryptedCandidateIdsAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return dbContext.Database.SqlQuery<Guid>(
            $"""
             SELECT id
             FROM withdrawal_requests
             WHERE bank_account_name IS NOT NULL
               AND bank_account_number IS NOT NULL
               AND (bank_account_name NOT LIKE 'enc.v1:%' OR bank_account_number NOT LIKE 'enc.v1:%')
             ORDER BY created_at
             LIMIT {BatchSize}
             """).ToListAsync(cancellationToken);
    }

    private static void TouchSensitiveFieldsForEncryption(ApplicationDbContext dbContext, Domain.Entities.WithdrawalRequest record)
    {
        record.BankAccountName = record.BankAccountName?.Trim() ?? string.Empty;
        record.BankAccountNumber = record.BankAccountNumber?.Trim() ?? string.Empty;
        var entry = dbContext.Entry(record);
        entry.Property(x => x.BankAccountName).IsModified = true;
        entry.Property(x => x.BankAccountNumber).IsModified = true;
    }
}
