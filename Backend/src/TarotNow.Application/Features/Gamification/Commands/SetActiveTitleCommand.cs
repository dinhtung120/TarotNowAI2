using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

// Command đặt title đang hiển thị cho user.
public record SetActiveTitleCommand(Guid UserId, string TitleCode) : IRequest<bool>;

// Handler xử lý đặt active title.
public class SetActiveTitleCommandExecutor : ICommandExecutionExecutor<SetActiveTitleCommand, bool>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Khởi tạo handler set active title.
    /// Luồng xử lý: nhận title repo để kiểm tra ownership và user repo để cập nhật profile.
    /// </summary>
    public SetActiveTitleCommandExecutor(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Xử lý command set active title.
    /// Luồng xử lý: kiểm tra user có sở hữu title (trừ trường hợp bỏ title), tải user và cập nhật active title rồi lưu.
    /// </summary>
    public async Task<bool> Handle(SetActiveTitleCommand request, CancellationToken cancellationToken)
    {
        var owns = await _titleRepo.OwnsTitleAsync(request.UserId, request.TitleCode, cancellationToken);
        if (!owns && !string.IsNullOrEmpty(request.TitleCode))
        {
            // Không sở hữu title yêu cầu thì từ chối cập nhật.
            return false;
        }

        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            // User không tồn tại nên không thể cập nhật active title.
            return false;
        }

        // TitleCode rỗng nghĩa là user muốn bỏ active title hiện tại.
        user.SetActiveTitle(string.IsNullOrEmpty(request.TitleCode) ? null : request.TitleCode);
        await _userRepo.UpdateAsync(user, cancellationToken);
        return true;
    }
}
