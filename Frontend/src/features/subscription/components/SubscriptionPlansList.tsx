'use client';

import React from 'react';
import { useTranslations } from 'next-intl';
import { useSubscriptionPlans } from '../hooks/useSubscriptions';
import { SubscriptionPlanCard } from './SubscriptionPlanCard';
import { cn } from '@/lib/utils';

export const SubscriptionPlansList = () => {
  const t = useTranslations('Subscription');

  
  const { data: plans, isLoading, isError } = useSubscriptionPlans();

  /* Skeleton loading: hiển thị 3 khung giả khi đang fetch data từ backend */
  if (isLoading) {
    return (
      <div className={cn("grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 animate-pulse")}>
        {[1, 2, 3].map(i => (
          <div key={i} className={cn("h-96 bg-slate-800/40 rounded-2xl border border-slate-700/30")}></div>
        ))}
      </div>
    );
  }

  
  if (isError || !plans) {
    return <div className={cn("text-red-400 text-center py-10")}>{t('loadError')}</div>;
  }

  /* Empty state: backend trả về mảng rỗng (Admin chưa tạo gói nào hoặc tất cả đều inactive) */
  if (plans.length === 0) {
    return <div className={cn("text-slate-400 text-center py-10")}>{t('noPlansAvailable')}</div>;
  }

  /* Render danh sách gói: grid responsive với SubscriptionPlanCard cho mỗi gói */
  return (
    <div className={cn("grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6")}>
      {plans.map(plan => (
        <SubscriptionPlanCard key={plan.id} plan={plan} />
      ))}
    </div>
  );
};
