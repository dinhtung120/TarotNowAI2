namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract cung cấp thông số checkout PayOS cho luồng nạp tiền.
/// </summary>
public interface IDepositPayOsSettings
{
    /// <summary>
    /// Url quay về khi thanh toán thành công hoặc user đóng trang.
    /// </summary>
    string ReturnUrl { get; }

    /// <summary>
    /// Url khi user hủy thanh toán.
    /// </summary>
    string CancelUrl { get; }

    /// <summary>
    /// Số phút hiệu lực payment link.
    /// </summary>
    int LinkExpiryMinutes { get; }
}
