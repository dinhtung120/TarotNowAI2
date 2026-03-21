/*
 * ===================================================================
 * FILE: IChatFinanceRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Giao Tiếp (Interface) với hệ lưu trữ Liên Quan Tới Túi Tiền Quản Lý Trong Quá Trình Chat Găm Tiền (Escrow).
 *   Tuyệt Đối Không Thêm Bớt Wallet Ở Đây, Chỉ Giải Quyết Khâu Khúc Nào Chuyển Bị Nộp Tiền, Khúc Nào Hoàn Tiền Thôi.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Thỏa thuận Thường Trực Về Tiền Nong Dưới DB (SQL).
/// Trực chiến cùng Chức Năng Băng Chuyền Giam Tiền Ký Quỹ.
/// </summary>
public interface IChatFinanceRepository
{
    // === XỬ LÝ PHIÊN GIAO DỊCH LỚN (Đóng Phí Khởi Điểm Chat) ===
    Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default);
    Task<List<ChatFinanceSession>> GetSessionsByConversationRefsAsync(IEnumerable<string> conversationRefs, CancellationToken ct = default);
    Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default);
    
    // Cái Lock Bất Tử Này Dành Riêng Cho Đóng Khóa Mềm Cho Giao Dịch Chậm Ở Dòng Đó Kẻo Bi Banh. (Pessimistic Locking)
    Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default);
    Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default);
    Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default);

    // === XỬ LÝ TỪNG CÂU HỎI NHỎ LẺ (User Nạp Token Trả Liền Cho Nhát Thứ 2, Thứ 3) ===
    Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default);
    Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default); // Cũng Là Mã Tự Khóa Select For Update
    Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default);
    Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
    Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default);
    Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default);

    // === THẰNG MÁY CHÉM ĐỀU ĐẶN - QUÉM BỚT CRON JOB ĐỂ XÓA THEO GIỜ ===
    
    // Khách Hỏi Xong Mà Thầy Tarot Gà Gật Không Thèm Bấm Dấu Chấp Nhận Kèo (Offer Expired).
    Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default);

    // Thầy Trả Lời Đi Mất Khách Đợi Quá Ư Là Lâu (24 Tiếng). Tòa Trả Lại Tiền Đi Nào.
    Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default);

    // Thầy Đã Trả Lời Hay Chảy Nước Mắt, Khách Im Re (Auto-Done Sau 24h ko Kiện Cáo Dispute).
    Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default);

    // Chốt Sổ Xuống Đĩa SQL 1 Nhát DB. (Unit Of Work Pattern Nhúng Trực Tiếp Save Đi Kèm).
    Task SaveChangesAsync(CancellationToken ct = default);
}
