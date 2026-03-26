/*
 * ===================================================================
 * FILE: LoginCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Login
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler trung tâm xử lý quy trình Đăng Nhập an toàn.
 *   Xác minh danh tính -> Kiểm tra trạng thái rào cản -> Cấp vé Token.
 *
 * LUỒNG HOẠT ĐỘNG (WORKFLOW):
 *   1. Tìm kiếm User: Hỗ trợ tìm bằng cả Email hoặc Username.
 *   2. Xác minh Password: Gọi Service băm mật khẩu (Argon2 / BCrypt).
 *   3. Kiểm tra User Status: Chặn ngay nếu Pending, Locked, hoặc Banned.
 *   4. Tạo Access Token: Ký phát JWT Token (ngắn hạn).
 *   5. Tạo Refresh Token: Sinh chuỗi ký tự ngẫu nhiên, lưu database (dài hạn).
 *   6. Trả kết quả: Build AuthResponse + RefreshToken string.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Core Handler cho Use-Case. Nhận vào LoginCommand, trả về tuple chứa response JWT và refresh token nguyên thuỷ.
/// </summary>
public partial class LoginCommandHandler : IRequestHandler<LoginCommand, (AuthResponse Response, string RefreshToken)>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;
    
    // Repository lưu trữ RefreshToken thay vì lưu hết vào bảng User 
    // Giúp hỗ trợ một tài khoản đăng nhập nhiều thiết bị (Multi-Device Sessions).
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService, 
        IJwtTokenSettings jwtTokenSettings,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<(AuthResponse Response, string RefreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = EnsureValidCredentials(
            await GetUserByIdentityAsync(request.EmailOrUsername, cancellationToken),
            request.Password);
        EnsureUserCanLogin(user);
        await RehashPasswordIfNeededAsync(user, request.Password, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenString = await CreateRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, accessToken);
        return (response, refreshTokenString);
    }
}
