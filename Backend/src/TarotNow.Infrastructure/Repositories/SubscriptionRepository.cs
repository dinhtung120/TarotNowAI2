using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

// Repository điều phối truy cập dữ liệu subscription và entitlement.
public partial class SubscriptionRepository : ISubscriptionRepository
{
    // DbContext dùng chung cho toàn bộ partial repository để đảm bảo cùng unit-of-work.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository với DbContext hiện tại.
    /// Luồng dependency injection này giúp các thao tác dữ liệu chia sẻ cùng transaction scope.
    /// </summary>
    public SubscriptionRepository(ApplicationDbContext context)
    {
        // Lưu context để các partial methods cùng thao tác trên một phiên làm việc dữ liệu.
        _context = context;
    }
}
