using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandler : IRequestHandler<CompleteAiStreamCommand, bool>
{
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IAiProvider _aiProvider;
    private readonly IReadingSessionRepository _readingRepo;

    public CompleteAiStreamCommandHandler(
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator,
        IAiProvider aiProvider,
        IReadingSessionRepository readingRepo)
    {
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
        _aiProvider = aiProvider;
        _readingRepo = readingRepo;
    }

    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        if (!AiStreamFinalStatuses.IsSupported(request.FinalStatus))
        {
            return false;
        }

        var context = new CompletionContext();
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => ProcessCompletionAsync(request, context, transactionCt),
            cancellationToken);

        if (!context.Processed || string.IsNullOrWhiteSpace(context.RequestId))
        {
            return context.Processed;
        }

        await LogTelemetrySafeAsync(request, context);
        return true;
    }

    private sealed class CompletionContext
    {
        public bool Processed { get; set; }

        public string? RequestId { get; set; }

        public string? SessionRef { get; set; }

        public string TelemetryStatus { get; set; } = "failed";

        public string? TelemetryErrorCode { get; set; }
    }
}
