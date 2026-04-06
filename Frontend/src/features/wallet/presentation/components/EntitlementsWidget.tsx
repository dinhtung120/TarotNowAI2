'use client';

/*
 * ===================================================================
 * FILE: EntitlementsWidget.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Widget hiển thị số dư quyền lợi (Entitlements) của user trên trang Wallet.
 *   Cho user thấy: mỗi loại quyền lợi còn bao nhiêu lượt sử dụng trong ngày.
 *
 * THIẾT KẾ:
 *   - Dùng useMyEntitlements() hook để fetch từ Server Action.
 *   - Nếu user không có entitlement nào (chưa mua gói) → ẩn widget hoàn toàn.
 *   - Glassmorphism design: gradient background, border vàng nhạt, bo góc.
 *   - Grid responsive: 2 cột mobile → 4 cột desktop.
 *   - Tất cả text đều qua i18n (DEBT-5 FIX: bỏ hardcode tiếng Anh).
 * ===================================================================
 */

import React from 'react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
import { Sparkles, Crown } from 'lucide-react';
import { useMyEntitlements } from '@/features/subscription/hooks/useSubscriptions';

export const EntitlementsWidget = () => {
  const t = useTranslations('Subscription');

  /*
   * useMyEntitlements: React Query hook gọi getMyEntitlementsAction.
   * Trả về EntitlementBalance[] chứa: entitlementKey, dailyQuotaTotal, usedToday, remainingToday.
   * Nếu user chưa mua gói nào → trả về mảng rỗng → widget ẩn.
   */
  const { data: entitlements, isLoading } = useMyEntitlements();

  /* Loading state: hiển thị skeleton card với icon xoay khi đang fetch */
  if (isLoading) {
    return (
      <GlassCard className="mb-12 animate-pulse min-h-[100px] flex items-center justify-center">
        <Sparkles className="w-5 h-5 text-slate-500 animate-spin" />
      </GlassCard>
    );
  }

  /* Ẩn widget nếu user không có entitlement nào (chưa mua gói hoặc gói đã hết hạn) */
  if (!entitlements || entitlements.length === 0) {
    return null;
  }

  return (
    <GlassCard className="mb-12 border-[#F6D365]/30 bg-slate-800/40 relative overflow-hidden group">
      {/* Gradient overlay tạo hiệu ứng Premium */}
      <div className="absolute inset-0 bg-gradient-to-br from-[#F6D365]/10 to-[#FDA085]/10 opacity-50" />
      
      <div className="relative z-10 flex flex-col md:flex-row items-center justify-between gap-6">
        {/* Header: Icon Crown + tiêu đề (đã i18n hóa) */}
        <div className="flex items-center gap-4">
          <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-[#F6D365] to-[#FDA085] flex items-center justify-center shadow-lg">
            <Crown className="w-6 h-6 text-slate-900" />
          </div>
          <div>
            {/* DEBT-5 FIX: Thay hardcoded "Premium Entitlements" bằng i18n key */}
            <h3 className="text-xl font-black text-transparent bg-clip-text bg-gradient-to-r from-[#F6D365] to-[#FDA085] tracking-tight">
              {t('premiumEntitlements')}
            </h3>
            {/* DEBT-5 FIX: Thay hardcoded "Your active daily quotas and perks" bằng i18n key */}
            <p className="text-xs text-slate-400 font-medium">{t('entitlementSubtitle')}</p>
          </div>
        </div>

        {/* Grid các entitlement cards: hiển thị remaining/day cho mỗi loại */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4 w-full md:w-auto flex-grow">
          {entitlements.map(ent => (
            <div key={ent.entitlementKey} className="bg-slate-900/50 rounded-lg p-3 border border-slate-700/50">
              {/* Tên quyền lợi: tra i18n bằng key dynamid entitlement_{key} */}
              <div className="text-[10px] uppercase tracking-wider text-slate-400 mb-1 truncate">
                {t(`entitlement_${ent.entitlementKey}`, { fallback: ent.entitlementKey })}
              </div>
              {/* Số lượt còn lại hôm nay — DEBT-5 FIX: "left today" → i18n */}
              <div className="text-xl font-bold text-white">
                {ent.remainingToday} <span className="text-xs text-slate-500 font-normal">{t('leftToday')}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </GlassCard>
  );
};
