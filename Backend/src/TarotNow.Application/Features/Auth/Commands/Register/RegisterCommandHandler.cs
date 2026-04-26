

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.Register;

// Handler xử lý luồng đăng ký tài khoản mới.
public class RegisterCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RegisterCommandHandlerRequestedDomainEvent>
{
    // Mã lỗi nghiệp vụ khi email đã tồn tại.
    private const string EmailExistsCode = "EMAIL_ALREADY_EXISTS";
    // Mã lỗi nghiệp vụ khi username đã tồn tại.
    private const string UsernameExistsCode = "USERNAME_ALREADY_EXISTS";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Khởi tạo handler đăng ký.
    /// Luồng xử lý: nhận user repository và password hasher để kiểm tra trùng, tạo user mới an toàn.
    /// </summary>
    public RegisterCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Xử lý command đăng ký tài khoản.
    /// Luồng xử lý: kiểm tra trùng email/username, băm mật khẩu, dựng user entity và lưu vào repository.
    /// </summary>
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await EnsureEmailAndUsernameAreAvailableAsync(request, cancellationToken);
        // Băm mật khẩu trước khi tạo user để không lưu mật khẩu thô.
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var newUser = BuildUser(request, hashedPassword);
        await _userRepository.AddAsync(newUser, cancellationToken);
        return newUser.Id;
    }

    protected override async Task HandleDomainEventAsync(
        RegisterCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra email và username chưa tồn tại trước khi đăng ký.
    /// Luồng xử lý: kiểm tra lần lượt email, username và ném BusinessRuleException khi trùng.
    /// </summary>
    private async Task EnsureEmailAndUsernameAreAvailableAsync(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            // Rule nghiệp vụ: email phải duy nhất trong hệ thống.
            throw new BusinessRuleException(
                EmailExistsCode,
                $"The email '{request.Email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            // Rule nghiệp vụ: username phải duy nhất để tránh xung đột định danh.
            throw new BusinessRuleException(
                UsernameExistsCode,
                $"The username '{request.Username}' is already taken.");
        }
    }

    /// <summary>
    /// Dựng user entity mới từ command và mật khẩu đã băm.
    /// Luồng xử lý: set display name fallback, chuẩn hóa DateOfBirth về UTC và tạo entity User.
    /// </summary>
    private static User BuildUser(RegisterCommand request, string hashedPassword)
    {
        var newUser = new User(
            email: request.Email,
            username: request.Username,
            passwordHash: hashedPassword,
            displayName: string.IsNullOrWhiteSpace(request.DisplayName) ? request.Username : request.DisplayName,
            dateOfBirth: DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
            hasConsented: request.HasConsented
        );

        return newUser;
    }
}
