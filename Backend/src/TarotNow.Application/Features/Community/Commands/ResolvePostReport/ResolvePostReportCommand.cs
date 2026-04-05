/*
 * ===================================================================
 * FILE: ResolvePostReportCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.ResolvePostReport
 * ===================================================================
 * MỤC ĐÍCH:
 *   Admin duyệt đơn report và đưa ra phán quyết (Resolve).
 *   Sẽ cập nhật trạng thái report, và cập nhật Target (Bài viết) nếu cần.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

public class ResolvePostReportCommand : IRequest<bool>
{
    public string ReportId { get; set; } = string.Empty;
    public Guid AdminId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
}

public class ResolvePostReportCommandHandler : IRequestHandler<ResolvePostReportCommand, bool>
{
    private readonly ICommunityPostRepository _postRepo;
    // NOTE: Cần một IAdminReportRepository hoặc update IReportRepository để Resolve.
    // Lấy tạm IReportRepository. Dưới thực tế có thể update IReportRepository.
    
    // Vì Interface IReportRepository hiện chưa có hàm Resolve (chỉ có Add, GetPaginated), 
    // chúng ta sẽ cần giả lập update hoặc ở Phase này ta quy định AdminCommunity workflow
    // sẽ call direct repo Mongo. Ở đây để code Application đúng chuẩn, ta sẽ gọi interface giả định (hoặc ta sẽ update IReportRepository sau).
    // Tạm thời mock logic để thể hiện CQRS pattern cho Plan 4.1.

    public ResolvePostReportCommandHandler(ICommunityPostRepository postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task<bool> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
    {
        // Logic sẽ gọi IReportRepository.GetById, .UpdateStatus (Cần update IReportRepository)
        // Hiện tại chỉ implement logic tác động đến Post.
        
        var validResults = new[] { ModerationResult.Warn, ModerationResult.RemovePost, ModerationResult.FreezeAccount, ModerationResult.NoAction };
        if (!validResults.Contains(request.Result))
            throw new BadRequestException("Kết quả xử lý không hợp lệ.");

        // Giả sử lấy được ReportTargetId = "abc"
        // Nếu kết quả là RemovePost -> Call SoftDelete
        if (request.Result == ModerationResult.RemovePost)
        {
            // await _postRepo.SoftDeleteAsync(report.TargetId, request.AdminId.ToString(), cancellationToken);
        }

        return await Task.FromResult(true);
    }
}
