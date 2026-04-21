using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;

/// <summary>
/// Query lấy trạng thái đơn đăng ký Reader gần nhất của user hiện tại.
/// </summary>
public class GetMyReaderRequestQuery : IRequest<GetMyReaderRequestResult>
{
    /// <summary>
    /// Định danh user cần kiểm tra đơn Reader.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// DTO trạng thái đơn Reader gần nhất.
/// </summary>
public class GetMyReaderRequestResult
{
    /// <summary>
    /// Cờ cho biết user đã từng gửi đơn Reader hay chưa.
    /// </summary>
    public bool HasRequest { get; set; }

    /// <summary>
    /// Trạng thái đơn gần nhất (pending/approved/rejected).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Nội dung giới thiệu trong đơn.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Danh sách chuyên môn đã đăng ký.
    /// </summary>
    public List<string> Specialties { get; set; } = [];

    /// <summary>
    /// Số năm kinh nghiệm đã đăng ký.
    /// </summary>
    public int? YearsOfExperience { get; set; }

    /// <summary>
    /// Link Facebook đã đăng ký.
    /// </summary>
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Link Instagram đã đăng ký.
    /// </summary>
    public string? InstagramUrl { get; set; }

    /// <summary>
    /// Link TikTok đã đăng ký.
    /// </summary>
    public string? TikTokUrl { get; set; }

    /// <summary>
    /// Giá dịch vụ đã đăng ký.
    /// </summary>
    public long? DiamondPerQuestion { get; set; }

    /// <summary>
    /// Ghi chú từ admin khi duyệt/từ chối đơn.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Thời điểm tạo đơn.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm admin review đơn.
    /// </summary>
    public DateTime? ReviewedAt { get; set; }
}

/// <summary>
/// Handler lấy thông tin đơn Reader gần nhất.
/// </summary>
public class GetMyReaderRequestQueryHandler : IRequestHandler<GetMyReaderRequestQuery, GetMyReaderRequestResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    /// <summary>
    /// Khởi tạo handler truy vấn đơn reader gần nhất.
    /// </summary>
    public GetMyReaderRequestQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    /// <summary>
    /// Xử lý query lấy đơn Reader gần nhất.
    /// </summary>
    public async Task<GetMyReaderRequestResult> Handle(GetMyReaderRequestQuery request, CancellationToken cancellationToken)
    {
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(request.UserId.ToString(), cancellationToken);
        if (latestRequest is null)
        {
            return new GetMyReaderRequestResult
            {
                HasRequest = false
            };
        }

        return new GetMyReaderRequestResult
        {
            HasRequest = true,
            Status = latestRequest.Status,
            Bio = latestRequest.Bio,
            Specialties = latestRequest.Specialties,
            YearsOfExperience = latestRequest.YearsOfExperience,
            FacebookUrl = latestRequest.FacebookUrl,
            InstagramUrl = latestRequest.InstagramUrl,
            TikTokUrl = latestRequest.TikTokUrl,
            DiamondPerQuestion = latestRequest.DiamondPerQuestion,
            AdminNote = latestRequest.AdminNote,
            CreatedAt = latestRequest.CreatedAt,
            ReviewedAt = latestRequest.ReviewedAt
        };
    }
}
