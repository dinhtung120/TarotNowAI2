using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public class RevealReadingSessionCommandHandler : IRequestHandler<RevealReadingSessionCommand, RevealReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserCollectionRepository _collectionRepo;
    private readonly IRngService _rngService;

    // TODO: Dynamic Exp từ config. Đang hardcode EXP cho MVP.
    private const long EXP_PER_CARD = 10; 

    public RevealReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo,
        IUserCollectionRepository collectionRepo,
        IRngService rngService)
    {
        _readingRepo = readingRepo;
        _collectionRepo = collectionRepo;
        _rngService = rngService;
    }

    public async Task<RevealReadingSessionResult> Handle(RevealReadingSessionCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy phiên rút bài
        var session = await _readingRepo.GetByIdAsync(request.SessionId, cancellationToken);
        
        if (session == null) throw new NotFoundException("Session not found");
        if (session.UserId != request.UserId) throw new BadRequestException("Unauthorized access to this session");
        if (session.IsCompleted) throw new BadRequestException("This session has already been revealed");

            // 2. Kích hoạt thuật toán gieo quẻ Deterministic Shuffle (Xào bài RNG)
            // Dùng 78 lá Tarot cơ bản
            var shuffledDeck = _rngService.ShuffleDeck(78);

            // 3. Trích xuất số lượng bài tùy theo loại Spsread 
            int cardsToDraw = session.SpreadType switch
            {
                SpreadType.Daily1Card => 1,
                SpreadType.Spread3Cards => 3,
                SpreadType.Spread5Cards => 5,
                SpreadType.Spread10Cards => 10,
                _ => throw new BadRequestException($"Invalid spread type: {session.SpreadType}")
            };

            // Lấy N lá trên cùng mặt mảng bài (Bộ Deck đã bị Shuffle)
            var drawnCards = shuffledDeck.Take(cardsToDraw).ToArray();

            // 4. Update Collection (Kho Đồ) & Tích luỹ thẻ trùng (Exp/Level Up)
            foreach (var cardId in drawnCards)
            {
                await _collectionRepo.UpsertCardAsync(request.UserId, cardId, EXP_PER_CARD, cancellationToken);
            }

            // 5. Cập nhật Session hoàn tất
            var cardsJson = JsonSerializer.Serialize(drawnCards);
            session.CompleteSession(cardsJson);
            await _readingRepo.UpdateAsync(session, cancellationToken);

            // 6. Trả về kết quả ngửa bài
            return new RevealReadingSessionResult
            {
                Cards = drawnCards
            };
    }
}
