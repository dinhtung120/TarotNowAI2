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

    public UpdateProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        // Nhờ Model Domain (Entities.User) cung cấp sẵn hàm Update Đóng Gói (Encapsulation).
        // Thay vì viết user.DisplayName = request.DisplayName (vi phạm quy tắc đóng gói DDD).
        user.UpdateProfile(request.DisplayName, request.AvatarUrl, request.DateOfBirth);

        // Lưu bản ghi thay mới.
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
