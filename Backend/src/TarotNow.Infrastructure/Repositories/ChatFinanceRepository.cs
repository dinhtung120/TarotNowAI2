using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

// Repository chính cho module chat finance, tách theo partial theo nhóm nghiệp vụ.
public partial class ChatFinanceRepository : IChatFinanceRepository
{
    // DbContext dùng cho tất cả thao tác session/item/dispute.
    private readonly ApplicationDbContext _db;

    /// <summary>
    /// Khởi tạo ChatFinanceRepository.
    /// Luồng xử lý: nhận DbContext từ DI để thao tác dữ liệu tài chính hội thoại.
    /// </summary>
    public ChatFinanceRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Persist toàn bộ thay đổi đang theo dõi trong DbContext.
    /// Luồng xử lý: gọi SaveChangesAsync để commit các thao tác add/update đã thực hiện ở partial khác.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }
}
