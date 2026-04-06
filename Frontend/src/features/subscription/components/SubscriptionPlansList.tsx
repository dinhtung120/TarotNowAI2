'use client';

/*
 * ===================================================================
 * FILE: SubscriptionPlansList.tsx
 * ===================================================================
 * MỤC ĐÍCH:
 *   Component wrapper hiển thị danh sách tất cả gói đăng ký đang mở bán.
 *   Quản lý 3 trạng thái: loading (skeleton), error, và danh sách rỗng/có dữ liệu.
 *
 * THIẾT KẾ:
 *   - Dùng useSubscriptionPlans() hook để fetch data từ Server Action.
 *   - Loading state: hiển thị 3 skeleton cards với animate-pulse.
 *   - Error state: hiển thị thông báo lỗi i18n (key: 'loadError').
 *   - Empty state: hiển thị thông báo không có gói (key: 'noPlansAvailable').
 *   - Grid responsive: 1 cột mobile → 2 cột tablet → 3 cột desktop.
 * ===================================================================
 */

import React from 'react';
import { useTranslations } from 'next-intl';
import { useSubscriptionPlans } from '../hooks/useSubscriptions';
import { SubscriptionPlanCard } from './SubscriptionPlanCard';

export const SubscriptionPlansList = () => {
  const t = useTranslations('Subscription');

  /*
   * useSubscriptionPlans: React Query hook gọi getSubscriptionPlansAction (Server Action).
   * staleTime = 5 phút → tránh gọi API lại liên tục khi chuyển tab.
   * data trả về: SubscriptionPlan[] (đã parse từ ActionResult).
   */
  const { data: plans, isLoading, isError } = useSubscriptionPlans();

  /* Skeleton loading: hiển thị 3 khung giả khi đang fetch data từ backend */
  if (isLoading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 animate-pulse">
        {[1, 2, 3].map(i => (
          <div key={i} className="h-96 bg-slate-800/40 rounded-2xl border border-slate-700/30"></div>
        ))}
      </div>
    );
  }

  /* 
   * Error state: hiển thị khi Server Action trả về {success: false}.
   * Thông báo lỗi được i18n hóa qua key 'loadError'.
   */
  if (isError || !plans) {
    return <div className="text-red-400 text-center py-10">{t('loadError')}</div>;
  }

  /* Empty state: backend trả về mảng rỗng (Admin chưa tạo gói nào hoặc tất cả đều inactive) */
  if (plans.length === 0) {
    return <div className="text-slate-400 text-center py-10">{t('noPlansAvailable')}</div>;
  }

  /* Render danh sách gói: grid responsive với SubscriptionPlanCard cho mỗi gói */
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {plans.map(plan => (
        <SubscriptionPlanCard key={plan.id} plan={plan} />
      ))}
    </div>
  );
};
