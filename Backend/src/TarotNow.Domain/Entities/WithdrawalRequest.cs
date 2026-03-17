using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarotNow.Domain.Entities;

/// <summary>
/// EF Core entity map bảng withdrawal_requests (PostgreSQL).
///
/// Yêu cầu rút tiền Reader:
/// → Min 50 Diamond, max 1 request/user/ngày (business_date_utc).
/// → Fee 10% tính bằng VND (amount_vnd * 10%).
/// → Quy đổi: 1 Diamond = 1000 VND.
/// → Admin approve → paid, reject → refund diamond.
/// </summary>
[Table("withdrawal_requests")]
public class WithdrawalRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>FK users — reader yêu cầu rút.</summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Ngày nghiệp vụ UTC — giới hạn 1 request/user/ngày.</summary>
    [Column("business_date_utc")]
    public DateOnly BusinessDateUtc { get; set; }

    /// <summary>Số Diamond rút (min 50).</summary>
    [Column("amount_diamond")]
    public long AmountDiamond { get; set; }

    /// <summary>Gross VND = amount_diamond × 1000.</summary>
    [Column("amount_vnd")]
    public long AmountVnd { get; set; }

    /// <summary>Phí 10% tính bằng VND.</summary>
    [Column("fee_vnd")]
    public long FeeVnd { get; set; }

    /// <summary>Số tiền thực nhận = amount_vnd - fee_vnd.</summary>
    [Column("net_amount_vnd")]
    public long NetAmountVnd { get; set; }

    [Column("bank_name")]
    [Required]
    public string BankName { get; set; } = string.Empty;

    [Column("bank_account_name")]
    [Required]
    public string BankAccountName { get; set; } = string.Empty;

    [Column("bank_account_number")]
    [Required]
    public string BankAccountNumber { get; set; } = string.Empty;

    /// <summary>pending | approved | rejected | paid.</summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>FK admin xử lý.</summary>
    [Column("admin_id")]
    public Guid? AdminId { get; set; }

    [Column("admin_note")]
    public string? AdminNote { get; set; }

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
