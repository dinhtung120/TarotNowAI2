using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

// Handler cập nhật hồ sơ user và đồng bộ dữ liệu reader profile khi có.
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật profile.
    /// Luồng xử lý: nhận user repository để cập nhật hồ sơ chính và reader profile repository để đồng bộ dữ liệu phụ.
    /// </summary>
    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IReaderProfileRepository readerProfileRepository)
    {
        _userRepository = userRepository;
        _readerProfileRepository = readerProfileRepository;
    }

    /// <summary>
    /// Xử lý command cập nhật profile.
    /// Luồng xử lý: cập nhật displayName + dateOfBirth của user, giữ avatar hiện tại và đồng bộ reader profile nếu có.
    /// </summary>
    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        // Avatar chỉ được cập nhật qua luồng presign/confirm chuyên biệt để bảo toàn objectKey + cleanup.
        user.UpdateProfile(request.DisplayName, user.AvatarUrl, request.DateOfBirth);
        // Áp dụng business rule cập nhật hồ sơ qua domain method để giữ invariant của entity User.

        await _userRepository.UpdateAsync(user);
        // Persist thay đổi profile chính trước để các luồng đọc user nhận dữ liệu mới ngay.

        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile is not null)
        {
            readerProfile.DisplayName = request.DisplayName;
            readerProfile.AvatarUrl = user.AvatarUrl;
            // Đồng bộ dữ liệu hiển thị cho reader profile để tránh lệch tên/avatar giữa hai nguồn dữ liệu.

            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
            // Cập nhật state của reader profile sau khi đã map xong thông tin mới.
        }
        // Edge case: user không có reader profile thì bỏ qua bước đồng bộ phụ.

        return true;
    }
}
