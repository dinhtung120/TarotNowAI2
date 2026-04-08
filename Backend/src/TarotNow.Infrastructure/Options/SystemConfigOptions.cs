namespace TarotNow.Infrastructure.Options;

// Options cấu hình tham số hệ thống và bảng giá đọc bài.
public sealed class SystemConfigOptions
{
    // Cấu hình giá các loại trải bài.
    public PricingOptions Pricing { get; set; } = new();

    // Hạn mức AI mỗi ngày.
    public int DailyAiQuota { get; set; } = 3;

    // Số request AI đồng thời tối đa.
    public int InFlightAiCap { get; set; } = 3;

    // Khoảng cách tối thiểu giữa các request đọc bài (giây).
    public int ReadingRateLimitSeconds { get; set; } = 30;

    // Cấu hình chi tiết giá theo từng loại trải bài.
    public sealed class PricingOptions
    {
        // Giá Gold cho trải bài 3 lá.
        public long Spread3Gold { get; set; } = 50;

        // Giá Diamond cho trải bài 3 lá.
        public long Spread3Diamond { get; set; } = 5;

        // Giá Gold cho trải bài 5 lá.
        public long Spread5Gold { get; set; } = 100;

        // Giá Diamond cho trải bài 5 lá.
        public long Spread5Diamond { get; set; } = 10;

        // Giá Gold cho trải bài 10 lá.
        public long Spread10Gold { get; set; } = 500;

        // Giá Diamond cho trải bài 10 lá.
        public long Spread10Diamond { get; set; } = 50;
    }
}
