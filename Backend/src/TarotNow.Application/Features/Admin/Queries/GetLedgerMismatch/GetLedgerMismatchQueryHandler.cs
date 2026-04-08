using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

// Handler truy vấn các bản ghi lệch số dư để admin đối soát.
public class GetLedgerMismatchQueryHandler : IRequestHandler<GetLedgerMismatchQuery, List<MismatchRecordDto>>
{
    private readonly IAdminRepository _adminRepository;

    /// <summary>
    /// Khởi tạo handler lấy ledger mismatch.
    /// Luồng xử lý: nhận admin repository để truy xuất dữ liệu đối soát tài chính.
    /// </summary>
    public GetLedgerMismatchQueryHandler(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách lệch số dư user vs ledger.
    /// Luồng xử lý: truy vấn mismatch entities, map sang DTO trả về cho lớp API.
    /// </summary>
    public async Task<List<MismatchRecordDto>> Handle(GetLedgerMismatchQuery request, CancellationToken cancellationToken)
    {
        var entities = await _adminRepository.GetLedgerMismatchesAsync(cancellationToken);

        // Map explicit sang DTO để tránh rò rỉ schema nội bộ repository.
        return entities.Select(entity => new MismatchRecordDto
        {
            UserId = entity.UserId,
            UserGoldBalance = entity.UserGoldBalance,
            LedgerGoldBalance = entity.LedgerGoldBalance,
            UserDiamondBalance = entity.UserDiamondBalance,
            LedgerDiamondBalance = entity.LedgerDiamondBalance
        }).ToList();
    }
}
