using MediatR;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

// Handler thực thi luồng reveal bài cho một reading session.
public partial class RevealReadingSessionCommandHandler : IRequestHandler<RevealReadingSessionCommand, RevealReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserCollectionRepository _collectionRepo;
    private readonly IUserRepository _userRepository;
    private readonly IRngService _rngService;

    // Hệ số exp mặc định cho mỗi lá bài.
    private const long ExpPerCard = 1;

    /// <summary>
    /// Khởi tạo handler reveal reading session.
    /// Luồng xử lý: nhận các repository/service để xác thực session, rút bài ngẫu nhiên, cập nhật collection/exp và lưu trạng thái phiên.
    /// </summary>
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

    /// <summary>
    /// Xử lý command reveal session.
    /// Luồng xử lý: kiểm tra session hợp lệ, trộn bộ bài và rút theo spread, cập nhật collection/exp, đánh dấu session completed và trả danh sách lá đã rút.
    /// </summary>
    public async Task<RevealReadingSessionResult> Handle(
        RevealReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await GetSessionForRevealAsync(request, cancellationToken);
        var shuffledDeck = _rngService.ShuffleDeck(78);
        var cardsToDraw = ResolveCardsToDraw(session.SpreadType);
        var drawnCards = shuffledDeck.Take(cardsToDraw).ToArray();
        var expToGrant = ResolveExpToGrant(session) * ExpPerCard;
        // Xác định số lá rút và exp theo loại spread/currency trước khi ghi dữ liệu người dùng.

        await UpdateCollectionAsync(request.UserId, drawnCards, expToGrant, cancellationToken);
        await ApplyUserExpAsync(request.UserId, drawnCards.Length * expToGrant, cancellationToken);
        // Cập nhật side-effects sở hữu thẻ và kinh nghiệm để đồng bộ tiến trình người chơi.

        var cardsJson = JsonSerializer.Serialize(drawnCards);
        session.CompleteSession(cardsJson);
        await _readingRepo.UpdateAsync(session, cancellationToken);
        // Đổi trạng thái session sang completed và lưu danh sách lá đã rút để phục vụ các bước AI tiếp theo.

        return new RevealReadingSessionResult
        {
            Cards = drawnCards
        };
    }
}
