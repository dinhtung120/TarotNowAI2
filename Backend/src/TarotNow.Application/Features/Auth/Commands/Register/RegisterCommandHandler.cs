/*
 * ===================================================================
 * FILE: RegisterCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Register
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trung tâm khởi tạo "Account Người Dùng Mới".
 *   
 * LUỒNG NGHIỆP VỤ:
 *   1. Chặn trùng lặp (Email và Username phải là duy nhất).
 *   2. Băm mật khẩu (Hash) nhằm bảo vệ Database khi rò rỉ (không lưu plaintext password).
 *   3. Khởi tạo đối tượng Domain `User`.
 *   4. Lưu vào Hệ cơ sở dữ liệu (PostgreSQL).
 *
 * MẬT MÃ ARGON2ID:
 *   IPasswordHasher tại sao lại quan trọng? Hệ thống TarotNow dùng thuật toán
 *   "Argon2id" (Khuyến cáo do tổ chức OWASP đề xuất). Nó kháng lại tấn công 
 *   Brute-Force bằng GPU (card đồ hoạ) và đánh cắp bộ nhớ.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// Xử lý logic nghiệp vụ cho RegisterCommand.
/// Dùng Argon2id để băm mật khẩu, kiểm tra trùng lặp email/username.
/// Khởi tạo User mới ở trạng thái Pending.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // --------------------------------------------------------------------
        // BƯỚC 1: RÀ SOÁT TRÙNG LẶP SỐ LIỆU ĐỊNH DANH (Email, Username)
        // Hệ thống sẽ trả DomainException 400 Bad Request ngay nếu đã có người khác lấy.
        // --------------------------------------------------------------------
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new DomainException("EMAIL_ALREADY_EXISTS", $"The email '{request.Email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new DomainException("USERNAME_ALREADY_EXISTS", $"The username '{request.Username}' is already taken.");
        }

        // --------------------------------------------------------------------
        // BƯỚC 2: MÃ HOÁ (HASHING)
        // Gọi thư viện PasswordHasher trộn Password + Muối (Salt). Trả về chuỗi băm.
        // KHÔNG BAO GIỜ Lưu "123456" trần trụi dưới Repo.
        // --------------------------------------------------------------------
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // --------------------------------------------------------------------
        // BƯỚC 3: KHỞI TẠO USER (NHƯNG CHƯA KHÍCH HOẠT PENDING)
        // Default Cấu trúc của Class User tự động gán Status = Pending (Chỉ đợi check-mail verify).
        // --------------------------------------------------------------------
        var newUser = new User(
            email: request.Email,
            username: request.Username,
            passwordHash: hashedPassword,
            
            // Xử lý tự làm mượt: Nếu người dùng lười gõ DisplayName (để trống)
            // Hệ thống sẽ lấy luôn Username làm Biệt Danh Mặc Định.
            displayName: string.IsNullOrWhiteSpace(request.DisplayName) ? request.Username : request.DisplayName,
            
            // Ép kiểu Date của .Net qua Chuẩn giờ chung UTC (Tránh lệch mũi giờ Việt Nam vs USA Database).
            dateOfBirth: DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
            
            hasConsented: request.HasConsented
        );

        // --------------------------------------------------------------------
        // BƯỚC 4: LƯU VÀO CƠ SỞ DỮ LIỆU
        // --------------------------------------------------------------------
        await _userRepository.AddAsync(newUser, cancellationToken);

        /*
         * TẠI SAO CHƯA GỞI MAIL XÁC THỰC MÀ ĐÃ ĐÓNG BLOCK LÀ SAO?
         * Đáp án:
         * Xú hướng Microservices và Asynchronous hiện đại:
         * - Luồng gửi thư (Mail) là luồng chậm ngoại vi (chậm cả s). 
         * - Đặt gửi thư ở đây dễ làm treo Front-End.
         * -> Vì thế Frontend sẽ giữ Id này và chủ động PING qua endpoint `send-verification-email` 
         * -> Giúp UI loading xoay mượt mà, Frontend tự chủ cơ chế "Re-send Email" tuỳ biến thời gian.
         */

        // 5. Trả về Id định danh CSDL của con người này (Guid).
        return newUser.Id;
    }
}
