namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial public API cho thao tác release escrow từ payer sang receiver.
public partial class WalletRepository
{
    /// <summary>
    /// Giải ngân escrow từ payer sang receiver.
    /// Luồng xử lý: đóng gói dữ liệu thành ReleaseRequest rồi chuyển sang ExecuteReleaseAsync để xử lý transaction/idempotency.
    /// </summary>
    public Task ReleaseAsync(
        Guid payerId,
        Guid receiverId,
        long amount,
        string? referenceSource = null,
        string? referenceId = null,
        string? description = null,
        string? metadataJson = null,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        var request = new ReleaseRequest(
            payerId,
            receiverId,
            amount,
            "Release",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteReleaseAsync(request, cancellationToken);
        // Release là nghiệp vụ 2 ví nên bắt buộc đi qua luồng xử lý chuyên biệt.
    }
}
