'use client';

import { useTranslations } from 'next-intl';
import { useSubscriptionPlans } from '../hooks/useSubscriptions';
import { SubscriptionPlanCard } from './SubscriptionPlanCard';
import { cn } from '@/lib/utils';

export const SubscriptionPlansList = () => {
  const t = useTranslations('Subscription');

  
  const { data: plans, isLoading, isError } = useSubscriptionPlans();

  
  if (isLoading) {
    return (
      <div className={cn("tn-grid-1-2-3-responsive gap-6 animate-pulse")}>
        {[1, 2, 3].map(i => (
          <div key={i} className={cn("h-96 bg-slate-800/40 rounded-2xl border border-slate-700/30")}></div>
        ))}
      </div>
    );
  }

  
  if (isError || !plans) {
    return <div className={cn("text-red-400 text-center py-10")}>{t('loadError')}</div>;
  }

  
  if (plans.length === 0) {
    return <div className={cn("text-slate-400 text-center py-10")}>{t('noPlansAvailable')}</div>;
  }

  
  return (
    <div className={cn("tn-grid-1-2-3-responsive gap-6")}>
      {plans.map(plan => (
        <SubscriptionPlanCard key={plan.id} plan={plan} />
      ))}
    </div>
  );
};
