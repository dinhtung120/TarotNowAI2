using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

// Handler điều phối luồng chốt AI stream: transaction, settlement, session update, telemetry và gamification.
public partial class CompleteAiStreamCommandHandler : IRequestHandler<CompleteAiStreamCommand, bool>
{
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IAiProvider _aiProvider;
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler complete AI stream.
    /// Luồng xử lý: nhận đầy đủ repository/service để thực hiện chốt request theo giao dịch và phát sinh side-effects sau commit.
    /// </summary>
    public CompleteAiStreamCommandHandler(
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator,
        IAiProvider aiProvider,
        IReadingSessionRepository readingRepo,
        IDomainEventPublisher domainEventPublisher)
    {
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
        _aiProvider = aiProvider;
        _readingRepo = readingRepo;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command hoàn tất AI stream.
    /// Luồng xử lý: validate status, chạy completion trong transaction, sau đó xử lý side-effects ngoài transaction như streak/gamification/telemetry.
    /// </summary>
    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        if (!AiStreamFinalStatuses.IsSupported(request.FinalStatus))
        {
            // Chặn trạng thái không thuộc whitelist để tránh chốt billing bằng trạng thái không xác định.
            return false;
        }

        var context = new CompletionContext();
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => ProcessCompletionAsync(request, context, transactionCt),
            cancellationToken);
        // Bọc các thao tác trạng thái/billing chính trong transaction để đảm bảo tính nhất quán dữ liệu.

        if (!context.Processed || string.IsNullOrWhiteSpace(context.RequestId))
        {
            // Edge case: record không tồn tại hoặc không được xử lý thì trả trạng thái thực tế, không phát sinh side-effect thêm.
            return context.Processed;
        }

        if (request.FinalStatus == AiStreamFinalStatuses.Completed)
        {
            await _domainEventPublisher.PublishAsync(
                new Domain.Events.ReadingCompletedDomainEvent
                {
                    UserId = request.UserId
                },
                cancellationToken);
            // Chỉ publish event hoàn tất khi stream completed để handler hậu xử lý streak/gamification chạy đúng ngữ cảnh.
        }

        await LogTelemetrySafeAsync(request, context);
        // Ghi telemetry sau completion để quan sát vận hành; lỗi telemetry không làm fail nghiệp vụ chính.

        return true;
    }

    // Context truyền giữa các bước trong transaction và hậu xử lý.
    private sealed class CompletionContext
    {
        // Cờ cho biết request đã được xử lý thành công trong transaction hay chưa.
        public bool Processed { get; set; }

        // Mã request dùng cho telemetry.
        public string? RequestId { get; set; }

        // Mã session liên quan request dùng cho telemetry.
        public string? SessionRef { get; set; }

        // Trạng thái telemetry tổng hợp (completed/failed).
        public string TelemetryStatus { get; set; } = "failed";

        // Mã lỗi telemetry khi thất bại (nếu có).
        public string? TelemetryErrorCode { get; set; }
    }
}
