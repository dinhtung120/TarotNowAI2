/*
 * ===================================================================
 * FILE: GetMyReaderRequestQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh Trích Xuất để Frontend biết User X "Đã Nộp Đơn Xin Làm Reader Tới Đâu Rồi".
 *   Dùng để vẽ giao diện: [Đang Chờ Duyệt], [Chúc Mừng Đậu], [Rớt Môn Kèm Lời Nhắn Admin].
 * ===================================================================
 */

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;

public class GetMyReaderRequestQuery : IRequest<GetMyReaderRequestResult>
{
    public Guid UserId { get; set; }
}

public class GetMyReaderRequestResult
{
    /// <summary>True = Cha này từng có Nộp Đơn (Bất luận Đậu/Rớt/Chờ). False = Tờ Giấy Trắng, chưa viết Đơn Xin Việc.</summary>
    public bool HasRequest { get; set; }
    
    /// <summary>pending (đang ngâm) / approved (Đậu) / rejected (Tạch).</summary>
    public string? Status { get; set; }
    
    public string? IntroText { get; set; }
    
    /// <summary>Lời Phê Của Giáo Viên (Vì sao Mày Tạch).</summary>
    public string? AdminNote { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class GetMyReaderRequestQueryHandler : IRequestHandler<GetMyReaderRequestQuery, GetMyReaderRequestResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    public GetMyReaderRequestQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    public async Task<GetMyReaderRequestResult> Handle(GetMyReaderRequestQuery request, CancellationToken cancellationToken)
    {
        // Nhờ DB lôi cổ Tờ Đơn Gần Nhất (GetLatest) của Khứa này.
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(request.UserId.ToString(), cancellationToken);
        
        // Không tìm thấy Lá Đơn Nào! (Nhanh, Gọn, Không màu mè).
        if (latestRequest == null)
        {
            return new GetMyReaderRequestResult { HasRequest = false };
        }

        // Cập Nhật Cấu Trúc cho Thằng Frontend vẽ Chart Lên Giao Diện.
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
