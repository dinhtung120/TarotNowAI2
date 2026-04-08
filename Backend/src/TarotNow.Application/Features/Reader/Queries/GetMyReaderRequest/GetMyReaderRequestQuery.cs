using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;

// Query lấy trạng thái đơn đăng ký Reader gần nhất của user hiện tại.
public class GetMyReaderRequestQuery : IRequest<GetMyReaderRequestResult>
{
    // Định danh user cần kiểm tra đơn Reader.
    public Guid UserId { get; set; }
}

// DTO trạng thái đơn Reader gần nhất.
public class GetMyReaderRequestResult
{
    // Cờ cho biết user đã từng gửi đơn Reader hay chưa.
    public bool HasRequest { get; set; }

    // Trạng thái đơn gần nhất (pending/approved/rejected).
    public string? Status { get; set; }

    // Nội dung giới thiệu trong đơn.
    public string? IntroText { get; set; }

    // Ghi chú từ admin khi duyệt/từ chối đơn.
    public string? AdminNote { get; set; }

    // Thời điểm tạo đơn.
    public DateTime? CreatedAt { get; set; }

    // Thời điểm admin review đơn.
    public DateTime? ReviewedAt { get; set; }
}

// Handler lấy thông tin đơn Reader gần nhất.
public class GetMyReaderRequestQueryHandler : IRequestHandler<GetMyReaderRequestQuery, GetMyReaderRequestResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    /// <summary>
    /// Khởi tạo handler truy vấn đơn reader gần nhất.
    /// Luồng xử lý: nhận reader request repository để lấy bản ghi mới nhất theo user.
    /// </summary>
    public GetMyReaderRequestQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    /// <summary>
    /// Xử lý query lấy đơn reader gần nhất.
    /// Luồng xử lý: tải bản ghi mới nhất theo user, trả cờ HasRequest=false nếu chưa từng gửi, ngược lại map dữ liệu chi tiết.
    /// </summary>
    public async Task<GetMyReaderRequestResult> Handle(
        GetMyReaderRequestQuery request,
        CancellationToken cancellationToken)
    {
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(
            request.UserId.ToString(),
            cancellationToken);

        if (latestRequest is null)
        {
            // Edge case: user chưa gửi đơn reader lần nào.
            return new GetMyReaderRequestResult
            {
                HasRequest = false
            };
        }

        return new GetMyReaderRequestResult
        {
            HasRequest = true,
            Status = latestRequest.Status,
            IntroText = latestRequest.IntroText,
            AdminNote = latestRequest.AdminNote,
            CreatedAt = latestRequest.CreatedAt,
            ReviewedAt = latestRequest.ReviewedAt
        };
    }
}
