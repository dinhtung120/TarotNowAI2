using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Services;

// Orchestrator điều phối debit ví và tạo reading session trả phí theo luồng an toàn.
public sealed partial class ReadingSessionOrchestrator : IReadingSessionOrchestrator
{
    // Chỉ thị debit cho một loại tiền cụ thể, giúp tái sử dụng logic debit chung.
    private readonly record struct DebitInstruction(
        Guid UserId,
        string SpreadType,
        string Currency,
        long Amount,
        string IdempotencyKey,
        CancellationToken CancellationToken);

    // Repository lưu phiên đọc bài sau khi trừ ví thành công.
    private readonly IReadingSessionRepository _readingSessionRepository;
    // Repository ví dùng để debit/credit trong luồng trả phí.
    private readonly IWalletRepository _walletRepository;

    /// <summary>
    /// Khởi tạo orchestrator đọc bài trả phí.
    /// Luồng inject repository giúp điều phối transaction nghiệp vụ giữa session và wallet.
    /// </summary>
    public ReadingSessionOrchestrator(
        IReadingSessionRepository readingSessionRepository,
        IWalletRepository walletRepository)
    {
        _readingSessionRepository = readingSessionRepository;
        _walletRepository = walletRepository;
    }

    /// <summary>
    /// Bắt đầu phiên đọc bài trả phí với cơ chế rollback khi tạo session thất bại.
    /// Luồng debit từng loại tiền, tạo session, và nếu lỗi thì hoàn tiền theo trạng thái debit thực tế.
    /// </summary>
    public async Task<(bool Success, string ErrorMessage)> StartPaidSessionAsync(
        StartPaidSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var idempotencyKey = $"read_{request.Session.Id}";
        var goldDebited = await DebitAsync(new DebitInstruction(
            request.UserId,
            request.SpreadType,
            CurrencyType.Gold,
            request.CostGold,
            idempotencyKey,
            cancellationToken));
        // Debit kim cương độc lập để hỗ trợ combo nhiều loại tiền trong cùng phiên.
        var diamondDebited = await DebitAsync(new DebitInstruction(
            request.UserId,
            request.SpreadType,
            CurrencyType.Diamond,
            request.CostDiamond,
            idempotencyKey,
            cancellationToken));

        try
        {
            // Chỉ tạo session sau khi hoàn tất debit để tránh mở phiên chưa thanh toán.
            await _readingSessionRepository.CreateAsync(request.Session, cancellationToken);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            // Nếu lưu session lỗi, rollback phần đã debit để giữ nhất quán số dư ví.
            await RollbackDebitsAsync(new RollbackContext(
                request.UserId,
                request.Session,
                request.CostGold,
                request.CostDiamond,
                goldDebited,
                diamondDebited,
                ex));
            return (false, "start_paid_session_failed");
        }
    }

    /// <summary>
    /// Thực hiện debit cho một lệnh thanh toán cụ thể.
    /// Luồng bỏ qua amount <= 0 và map transaction type theo currency để ghi ledger đúng mục đích.
    /// </summary>
    private async Task<bool> DebitAsync(DebitInstruction instruction)
    {
        if (instruction.Amount <= 0)
        {
            // Không debit khi chi phí bằng 0 để tránh sinh giao dịch rác.
            return false;
        }

        // Mapping loại tiền sang transaction type để báo cáo tài chính đúng chiều.
        var type = instruction.Currency == CurrencyType.Gold
            ? TransactionType.ReadingCostGold
            : TransactionType.ReadingCostDiamond;

        var debitResult = await _walletRepository.DebitWithResultAsync(
            instruction.UserId,
            instruction.Currency,
            type,
            instruction.Amount,
            "Reading",
            $"Tarot_{instruction.SpreadType}",
            $"Phiên rút Tarot {instruction.SpreadType}",
            metadataJson: null,
            idempotencyKey: instruction.IdempotencyKey,
            cancellationToken: instruction.CancellationToken);

        // Trả cờ executed để rollback chỉ hoàn những khoản đã trừ thực tế.
        return debitResult.Executed;
    }
}
