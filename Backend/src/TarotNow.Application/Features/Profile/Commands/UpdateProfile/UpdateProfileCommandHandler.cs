/*
 * ===================================================================
 * FILE: UpdateProfileCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Profile.Commands.UpdateProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gắn thông tin thay đổi vào Object User và Save đè xuống Database.
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IReaderProfileRepository readerProfileRepository)
    {
        _userRepository = userRepository;
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        // Nhờ Model Domain (Entities.User) cung cấp sẵn hàm Update Đóng Gói (Encapsulation).
        // Thay vì viết user.DisplayName = request.DisplayName (vi phạm quy tắc đóng gói DDD).
        user.UpdateProfile(request.DisplayName, request.AvatarUrl, request.DateOfBirth);

        // Lưu bản ghi thay mới Postgres
        await _userRepository.UpdateAsync(user);

        // Đồng bộ hoá Name & Avatar sang Mongo Reader Profile
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile != null)
        {
            readerProfile.DisplayName = request.DisplayName;
            readerProfile.AvatarUrl = request.AvatarUrl;
            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
        }

        return true;
    }
}
