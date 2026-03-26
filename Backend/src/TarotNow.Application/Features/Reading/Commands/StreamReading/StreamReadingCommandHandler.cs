using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Services;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler : IRequestHandler<StreamReadingCommand, StreamReadingResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IAiProvider _aiProvider;
    private readonly ICacheService _cacheService;
    private readonly FollowupPricingService _pricingService;
    private readonly int _dailyAiQuota;
    private readonly int _inFlightAiCap;
    private readonly int _readingRateLimitSeconds;

    public StreamReadingCommandHandler(
        IReadingSessionRepository readingRepo,
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        IAiProvider aiProvider,
        ICacheService cacheService,
        FollowupPricingService pricingService,
        ISystemConfigSettings systemConfigSettings)
    {
        _readingRepo = readingRepo;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _aiProvider = aiProvider;
        _cacheService = cacheService;
        _pricingService = pricingService;
        _dailyAiQuota = systemConfigSettings.DailyAiQuota;
        _inFlightAiCap = systemConfigSettings.InFlightAiCap;
        _readingRateLimitSeconds = systemConfigSettings.ReadingRateLimitSeconds;
    }

    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        var session = await ValidateSessionAsync(request, cancellationToken);
        await EnsureQuotaAsync(request.UserId, cancellationToken);
        await EnsureRateLimitAsync(request.UserId, cancellationToken);

        var calculatedCost = await CalculateCostAsync(request, session, cancellationToken);
        var aiRequest = await CreateAiRequestAsync(request, session, calculatedCost, cancellationToken);

        await FreezeEscrowAsync(request, aiRequest, calculatedCost, cancellationToken);

        var prompts = StreamReadingPromptFactory.Build(session, request.FollowupQuestion, request.Language);
        var stream = _aiProvider.StreamChatAsync(prompts.SystemPrompt, prompts.UserPrompt, cancellationToken);

        return new StreamReadingResult
        {
            Stream = stream,
            AiRequestId = aiRequest.Id
        };
    }
}
