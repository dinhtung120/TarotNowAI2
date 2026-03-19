/*
 * ===================================================================
 * FILE: WalletDtos.cs
 * NAMESPACE: TarotNow.Application.Features.Wallet.Queries
 * ===================================================================
 * MỤC ĐÍCH:
 *   Chứa các cấu trúc dữ liệu trả về (Data Transfer Objects) 
 *   cho module Ví (Wallet) và Sổ cái (Ledger).
 * ===================================================================
 */

using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Wallet.Queries;

/// <summary>
/// Dữ liệu trả về khi truy vấn số dư ví hiện tại của User.
/// </summary>
public class WalletBalanceDto
{
    /// <summary>Số dư Vàng (Dùng để rút bài Tarot cơ bản).</summary>
    public long GoldBalance { get; set; }
    
    /// <summary>Số Kim Cương có thể tiêu xài (Dùng cho Escrow/Chat).</summary>
    public long DiamondBalance { get; set; }
    
    /// <summary>Số Kim Cương đang bị đóng băng trong các giao dịch Escrow chưa Release.</summary>
    public long FrozenDiamondBalance { get; set; }
}

/// <summary>
/// Dữ liệu biểu diễn một dòng lịch sử giao dịch (Transaction History) trong Sổ cái.
/// </summary>
public class WalletTransactionDto
{
    public Guid Id { get; set; }
    
    /// <summary>Loại tiền tệ (Gold hoặc Diamond).</summary>
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>Phân loại giao dịch (Deposit, Withdrawal, Reward, EscrowFreeze...).</summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>Số lượng biến động (Kèm theo chiều dương hoặc âm tùy ngữ cảnh trình bày).</summary>
    public long Amount { get; set; }
    
    /// <summary>Số dư trước khi biến động.</summary>
    public long BalanceBefore { get; set; }
    
    /// <summary>Số dư sau khi biến động.</summary>
    public long BalanceAfter { get; set; }
    
    /// <summary>Ghi chú hoặc lý do giao dịch để hiển thị cho User dễ hiểu.</summary>
    public string? Description { get; set; }
    
    /// <summary>Thời điểm chốt sổ giao dịch.</summary>
    public DateTime CreatedAt { get; set; }
}
