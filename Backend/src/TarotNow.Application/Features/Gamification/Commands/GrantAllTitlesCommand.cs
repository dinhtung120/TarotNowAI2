using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

// Command cấp toàn bộ title hiện có cho user.
public record GrantAllTitlesCommand(Guid UserId) : IRequest<bool>;

// Handler cấp toàn bộ title cho user.
public class GrantAllTitlesCommandExecutor : ICommandExecutionExecutor<GrantAllTitlesCommand, bool>
{
    private readonly ITitleRepository _titleRepository;

    /// <summary>
    /// Khởi tạo handler grant all titles.
    /// Luồng xử lý: nhận title repository để liệt kê title và cấp cho user chưa sở hữu.
    /// </summary>
    public GrantAllTitlesCommandExecutor(ITitleRepository titleRepository)
    {
        _titleRepository = titleRepository;
    }

    /// <summary>
    /// Xử lý command cấp tất cả title.
    /// Luồng xử lý: tải toàn bộ title, kiểm tra ownership từng title và cấp title còn thiếu cho user.
    /// </summary>
    public async Task<bool> Handle(GrantAllTitlesCommand request, CancellationToken cancellationToken)
    {
        var titles = await _titleRepository.GetAllTitlesAsync(cancellationToken);
        foreach (var title in titles)
        {
            if (!await _titleRepository.OwnsTitleAsync(request.UserId, title.Code, cancellationToken))
            {
                // Chỉ grant title khi user chưa sở hữu để tránh thao tác thừa.
                await _titleRepository.GrantTitleAsync(request.UserId, title.Code, cancellationToken);
            }
        }

        return true;
    }
}
