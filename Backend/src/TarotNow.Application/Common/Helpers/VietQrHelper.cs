namespace TarotNow.Application.Common.Helpers;

/// <summary>
/// Helper dựng URL QR chuyển khoản theo chuẩn VietQR image endpoint.
/// </summary>
public static class VietQrHelper
{
    private const string VietQrBaseUrl = "https://img.vietqr.io/image";

    /// <summary>
    /// Tạo URL QR để admin quét chuyển khoản nhanh.
    /// </summary>
    public static string BuildTransferQrImageUrl(
        string bankBin,
        string bankAccountNumber,
        long amountVnd,
        string transferContent,
        string accountHolder)
    {
        var encodedContent = Uri.EscapeDataString(transferContent);
        var encodedAccountHolder = Uri.EscapeDataString(accountHolder);
        return $"{VietQrBaseUrl}/{bankBin}-{bankAccountNumber}-compact2.png?amount={amountVnd}&addInfo={encodedContent}&accountName={encodedAccountHolder}";
    }
}
