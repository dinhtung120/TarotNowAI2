using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

// Command tạo yêu cầu rút tiền bằng diamond.
public class CreateWithdrawalCommand : IRequest<Guid>
{
    // Định danh user gửi yêu cầu rút.
    public Guid UserId { get; set; }

    // Số diamond muốn rút.
    public long AmountDiamond { get; set; }

    // Khóa idempotency để chống tạo yêu cầu trùng khi retry.
    public string IdempotencyKey { get; set; } = string.Empty;

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Tên chủ tài khoản ngân hàng.
    public string BankAccountName { get; set; } = string.Empty;

    // Số tài khoản ngân hàng.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Mã MFA của user để xác thực thao tác rút.
    public string MfaCode { get; set; } = string.Empty;
}

// Handler điều phối luồng tạo yêu cầu rút tiền.
public partial class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalCommand, Guid>
{
    // Kế hoạch quy đổi rút tiền từ diamond sang VND và phí.
    private readonly record struct WithdrawalPlan(long AmountVnd, long FeeVnd, long NetAmountVnd);

    private readonly IWithdrawalRepository _withdrawalRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler tạo withdrawal request.
    /// Luồng xử lý: nhận repository/service để validate user, trừ ví, tạo yêu cầu rút và commit transaction.
    /// </summary>
    public CreateWithdrawalCommandHandler(
        IWithdrawalRepository withdrawalRepo,
        IWalletRepository walletRepo,
        IUserRepository userRepo,
        IMfaService mfaService,
        ITransactionCoordinator transactionCoordinator)
    {
        _withdrawalRepo = withdrawalRepo;
        _walletRepo = walletRepo;
        _userRepo = userRepo;
        _mfaService = mfaService;
        _transactionCoordinator = transactionCoordinator;
    }

    /// <summary>
    /// Xử lý command tạo yêu cầu rút.
    /// Luồng xử lý: lấy user, chạy các bước validation nghiệp vụ, build kế hoạch rút và thực thi workflow tạo yêu cầu trong transaction.
    /// </summary>
    public async Task<Guid> Handle(CreateWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var user = await GetUserOrThrowAsync(request.UserId, cancellationToken);
        await ValidateRequestAsync(request, user, today, cancellationToken);
        // Chặn toàn bộ trường hợp không hợp lệ trước khi bắt đầu tác vụ trừ ví.

        var plan = BuildWithdrawalPlan(request.AmountDiamond);
        var createdRequest = await ExecuteWithdrawalAsync(request, today, plan, cancellationToken);
        // Workflow trả về request đã tạo (hoặc request cũ theo idempotency).

        return createdRequest.Id;
    }
}
