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
        // 1. Lọc Cửa Vào (Security Gate): 
        // Lấy phòng bốc bài ra kiểm duyệt (Có phải của thằng này tạo không? Bốc mẹ rồi bốc nữa chi?)
        var session = await _readingRepo.GetByIdAsync(request.SessionId, cancellationToken);
        
        if (session == null) throw new NotFoundException("Session not found");
        if (session.UserId != request.UserId.ToString())
            throw new UnauthorizedAccessException("Reading session not found or access denied");
        if (session.IsCompleted) throw new BadRequestException("This session has already been revealed");

        // 2. Kích hoạt thuật toán gieo quẻ Bằng Hàm Random Thần Thánh (Deterministic Shuffle RNG).
        // Trộn Văng Khắp Nơi Nguyên Bộ 78 Lá Của Tarot (0-77).
        var shuffledDeck = _rngService.ShuffleDeck(78);

        // 3. Trích xuất số lượng bài tùy theo Bảng Giá Gói Dịch Vụ Của Cái Phòng Này (Vd Mua gói 3 lá thì cho 3 lá).
        int cardsToDraw = session.SpreadType switch
        {
            SpreadType.Daily1Card => 1,
            SpreadType.Spread3Cards => 3,
            SpreadType.Spread5Cards => 5,
            SpreadType.Spread10Cards => 10,
            _ => throw new BadRequestException($"Invalid spread type: {session.SpreadType}")
        };

        // Bốc N ngẫu nhiên mảng ID Lá Bài trên cùng của bộ xào (Top Deck Draw).
        var drawnCards = shuffledDeck.Take(cardsToDraw).ToArray();

        // 4. Update Tủ Kính Kho Đồ (Collection) & Tính Cơ Chế Quay Gacha 
        // Bốc Trúng Lá Mới Cấp 1, Lá Cũ Trùng Thì Nhồi Exp Lên Cấp.
        // HỆ SỐ: Kim cương x2 EXP (+2), Vàng x1 EXP (+1).
        // YÊU CẦU: Daily 1 Card luôn là 1 EXP. Các loại khác dùng Diamond thì x2.
        long expToGrant = (session.SpreadType != SpreadType.Daily1Card && 
                          string.Equals(session.CurrencyUsed, CurrencyType.Diamond, StringComparison.OrdinalIgnoreCase)) 
                          ? 2 : 1;

        foreach (var cardId in drawnCards)
        {
            await _collectionRepo.UpsertCardAsync(request.UserId, cardId, expToGrant, cancellationToken);
        }

        // 5. Cộng EXP Vào Hồ Sơ Cá Nhân (Gamification)
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user != null)
        {
            // Tụ 10 lá Diamond Được 20 Exp Sướng Run Người.
            user.AddExp(drawnCards.Length * expToGrant);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // 6. Ghi Nhận Lên SQL Dấu Chấm Hết Mã Hóa JSON (Chống Lật Đi Lật Lại 1 Phòng).
        var cardsJson = JsonSerializer.Serialize(drawnCards);
        session.CompleteSession(cardsJson);
        await _readingRepo.UpdateAsync(session, cancellationToken);

        // 7. Ném mảng Index cho Thằng JavaScript Vẽ Ra 3D Bằng Three.js
        return new RevealReadingSessionResult
        {
            Cards = drawnCards
        };
    }
}
