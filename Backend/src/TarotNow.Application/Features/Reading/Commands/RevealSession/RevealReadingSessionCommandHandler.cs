/*
 * ===================================================================
 * FILE: RevealReadingSessionCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.RevealSession
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trái Tim Của Tính Năng Gieo Quẻ Tarot: Thuật Toán Xào Bài (Shuffle), 
 *   Phân Phối Kinh Nghiệm (Gamification EXP), Mở Khoá Bộ Sưu Tập (Collection).
 * ===================================================================
 */

using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public partial class RevealReadingSessionCommandHandler : IRequestHandler<RevealReadingSessionCommand, RevealReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserCollectionRepository _collectionRepo;
    private readonly IUserRepository _userRepository; 
    private readonly IRngService _rngService;

    // Quy định: 1 lá bài bốc được = Cày lòi ra 1 EXP cho Tướng Lĩnh User và 1 EXP cho Điểm Thông Thạo Lá Bài Đó.
    private const long EXP_PER_CARD = 1; 

    public RevealReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo,
        IUserCollectionRepository collectionRepo,
        IUserRepository userRepository, 
        IRngService rngService)
    {
        _readingRepo = readingRepo;
        _collectionRepo = collectionRepo;
        _userRepository = userRepository;
        _rngService = rngService;
    }

    public async Task<RevealReadingSessionResult> Handle(RevealReadingSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await GetSessionForRevealAsync(request, cancellationToken);
        var shuffledDeck = _rngService.ShuffleDeck(78);
        var cardsToDraw = ResolveCardsToDraw(session.SpreadType);
        var drawnCards = shuffledDeck.Take(cardsToDraw).ToArray();
        var expToGrant = ResolveExpToGrant(session);

        await UpdateCollectionAsync(request.UserId, drawnCards, expToGrant, cancellationToken);
        await ApplyUserExpAsync(request.UserId, drawnCards.Length * expToGrant, cancellationToken);

        var cardsJson = JsonSerializer.Serialize(drawnCards);
        session.CompleteSession(cardsJson);
        await _readingRepo.UpdateAsync(session, cancellationToken);
        return new RevealReadingSessionResult
        {
            Cards = drawnCards
        };
    }
}
