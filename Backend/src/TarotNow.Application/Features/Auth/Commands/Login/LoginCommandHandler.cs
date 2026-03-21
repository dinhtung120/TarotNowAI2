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
public class LoginCommandHandler : IRequestHandler<LoginCommand, (AuthResponse Response, string RefreshToken)>
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
        // -------------------------------------------------------------
        // BƯỚC 1: Tìm kiếm định danh User linh hoạt
        // Nếu chuỗi chứa kí tự '@', hệ thống cho đó là Email. Nếu không → username.
        // -------------------------------------------------------------
        User? user = null;
        if (request.EmailOrUsername.Contains('@'))
        {
            user = await _userRepository.GetByEmailAsync(request.EmailOrUsername, cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(request.EmailOrUsername, cancellationToken);
        }

        // BẢO MẬT KHÔNG NHẬN CỤ THỂ LỖI (Obfuscation):
        // Nếu sai user hoặc sai mật khẩu, ta quăng CHUNG 1 thông báo "Tài khoản hoặc mật khẩu không đúng".
        // Tránh tình trạng lộ thông tin: "Email này có trên DB nhưng sai pass".
        if (user == null)
        {
            throw new BusinessRuleException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        // -------------------------------------------------------------
        // BƯỚC 2: So khớp Password Hash
        // Gọi thư viện Hash (Argon2id/BCrypt) để xác minh mật khẩu trơn gửi lên 
        // có khớp với bản băm đang lưu trên Postgres không.
        // -------------------------------------------------------------
        if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new BusinessRuleException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        // -------------------------------------------------------------
        // BƯỚC 3: Rào cản Trạng thái tài khoản (Status Gateway)
        // -------------------------------------------------------------
        if (user.Status == UserStatus.Pending)
        {
            // Pending: User chưa click link xác thực đăng ký gửi vào Email.
            throw new BusinessRuleException("USER_PENDING", "Please verify your email address to log in.");
        }
        else if (user.Status == UserStatus.Banned || user.Status == UserStatus.Locked)
        {
            // Banned/Locked: User bị quản trị viên khóa, từ chối cấp token.
            throw new BusinessRuleException("USER_BLOCKED", "Your account is temporarily locked or banned.");
        }

        // -------------------------------------------------------------
        // BƯỚC 4: Tạo JWT Access Token
        // Token này sẽ chứa {userId, Role, Tên} được ký bí mật HMAC.
        // -------------------------------------------------------------
        var accessToken = _tokenService.GenerateAccessToken(user);

        // -------------------------------------------------------------
        // BƯỚC 5: Xử lý Refresh Token (Vé lên tàu dài hạn)
        // Lưu xuống Database cấu trúc RefreshToken Entity (Id, TokenString, Hạn chót, IP).
        // Khi JWT chết, Frontend gửi TokenString này để xin JWT mới.
        // -------------------------------------------------------------
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = _jwtTokenSettings.RefreshTokenExpiryDays;
        var refreshTokenEntity = new TarotNow.Domain.Entities.RefreshToken(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
            createdByIp: request.ClientIpAddress // Lưu lại IP gác cổng lúc tạo, audit bảo mật.
        );

        // Lưu vào cơ sở dữ liệu
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        // -------------------------------------------------------------
        // BƯỚC 6: Trả về kết quả
        // -------------------------------------------------------------
        var resp = new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresInMinutes = _jwtTokenSettings.AccessTokenExpiryMinutes,
            User = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Level = user.Level,
                Exp = user.Exp,
                Role = user.Role,
                Status = user.Status.ToString()
            }
        };

        // Tuple Result
        return (resp, refreshTokenString);
    }
}
