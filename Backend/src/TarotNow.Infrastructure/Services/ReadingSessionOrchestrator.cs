using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Services;

public sealed partial class ReadingSessionOrchestrator : IReadingSessionOrchestrator
{
    private readonly record struct DebitInstruction(
        Guid UserId,
        string SpreadType,
        string Currency,
        long Amount,
        string IdempotencyKey,
        CancellationToken CancellationToken);

    private readonly IReadingSessionRepository _readingSessionRepository;
    private readonly IWalletRepository _walletRepository;

    public ReadingSessionOrchestrator(
        IReadingSessionRepository readingSessionRepository,
        IWalletRepository walletRepository)
    {
        _readingSessionRepository = readingSessionRepository;
        _walletRepository = walletRepository;
    }

    public async Task<(bool Success, string ErrorMessage)> StartPaidSessionAsync(
        StartPaidSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var idempotencyKey = $"read_{request.Session.Id}";
        var goldDebited = await DebitAsync(new DebitInstruction(
            request.UserId,
            request.SpreadType,
            CurrencyType.Gold,
            request.CostGold,
            idempotencyKey,
            cancellationToken));
        var diamondDebited = await DebitAsync(new DebitInstruction(
            request.UserId,
            request.SpreadType,
            CurrencyType.Diamond,
            request.CostDiamond,
            idempotencyKey,
            cancellationToken));

        try
        {
            await _readingSessionRepository.CreateAsync(request.Session, cancellationToken);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            await RollbackDebitsAsync(new RollbackContext(
                request.UserId,
                request.Session,
                request.CostGold,
                request.CostDiamond,
                goldDebited,
                diamondDebited,
                ex));
            return (false, "start_paid_session_failed");
        }
    }

    private async Task<bool> DebitAsync(DebitInstruction instruction)
    {
        if (instruction.Amount <= 0)
        {
            return false;
        }

        var type = instruction.Currency == CurrencyType.Gold
            ? TransactionType.ReadingCostGold
            : TransactionType.ReadingCostDiamond;

        var debitResult = await _walletRepository.DebitWithResultAsync(
            instruction.UserId,
            instruction.Currency,
            type,
            instruction.Amount,
            "Reading",
            $"Tarot_{instruction.SpreadType}",
            $"Phiên rút Tarot {instruction.SpreadType}",
            metadataJson: null,
            idempotencyKey: instruction.IdempotencyKey,
            cancellationToken: instruction.CancellationToken);

        return debitResult.Executed;
    }
}
