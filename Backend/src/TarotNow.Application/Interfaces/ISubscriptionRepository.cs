/*
 * ===================================================================
 * FILE: ISubscriptionRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao diện kết nối tới Tầng Data (Repository) chuyên quản lý các Gói Đăng Ký và Số dư Quyền Lợi (Entitlement).
 *   Lý do thiết kế: Tách rời Logic C# ra khỏi Cấu hình EF Core / PostgreSQL.
 *   Repository này đặc biệt NHẠY CẢM vì nó phải bao hàm các hàm Row-locking cho Database Transaction (Rút Consume).
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Cửa ngõ kết nối Database dành riêng cho hệ Gói Cước và Rổ Quyền Lợi.
/// </summary>
public interface ISubscriptionRepository
{
    // ==========================================
    // 1. QUẢN LÝ GÓI ĐĂNG KÝ (PLANS)
    // ==========================================
    Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct);
    Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct);
    Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct);
    Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct);

    // ==========================================
    // 2. BIÊN LAI SỞ HỮU GÓI CỦA KHÁCH (USER SUBSCRIPTIONS)
    // ==========================================
    Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct);
    Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct);
    
    /// <summary>
    /// Tìm tất cả các Gói đang "Active" nhưng đã qua thời điểm quá hạn (Lấy tới Mốc Thước thời gian cho).
    /// Phục vụ cho Background Job đụt sạch bọn hết hạn.
    /// </summary>
    Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct);
    Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct);
    Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct);

    // ==========================================
    // 3. RỔ QUYỀN LỢI (ENTITLEMENT BUCKETS)
    // ==========================================
    
    /// <summary>
    /// Lưu nguyên mẻ Rổ Đặc Quyền sau khi mua Gói Thành Công.
    /// </summary>
    Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct);

    /// <summary>
    /// [QUAN TRỌNG - RAW ROW LOCK]
    /// Lôi cổ các rổ Quyền Lợi ra nhưng BẮT BUỘC KHÓA DÒNG SQL (Select For Update).
    /// Chỉ Lấy Những Rổ Còn Hạn Sử Dụng TRONG NGÀY.
    /// Sắp xếp Ưu Tiên Rổ Nào Chết Sớm Xài Trước (Earliest-Expiry-First).
    /// </summary>
    Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId, 
        string entitlementKey, 
        DateOnly businessDate, 
        CancellationToken ct);

    /// <summary>
    /// Tính tổng số lượng hạn mức dư dả của Khách trong 1 Ngày Cụ Thể Gộp Nhanh Dành Riêng Cho Mảnh (Read).
    /// </summary>
    Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(
        Guid userId, 
        DateOnly businessDate, 
        CancellationToken ct);

    /// <summary>
    /// Dành cho Background Job: Lấy ra các Rổ Chưa Được Cập Nhật Của Ngày Mới Để Dội Reset Về 0.
    /// </summary>
    Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(
        DateOnly oldBusinessDate, 
        CancellationToken ct);

    // ==========================================
    // 4. LUẬT MẶC ĐỊNH BÙ LỖ (MAPPING RULES)
    // ==========================================
    Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(
        string sourceKey, 
        CancellationToken ct);

    // ==========================================
    // 5. NHẬT KÝ ĐỐT LƯỢT (CONSUME LOGS)
    // ==========================================
    Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct);
    Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct);

    // Lưu Cứng Xuống Ổ Cứng Server Database (Cái Cuối Cùng).
    Task SaveChangesAsync(CancellationToken ct);
}
