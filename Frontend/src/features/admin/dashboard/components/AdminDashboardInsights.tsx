'use client';

import { Activity, Gem, TrendingUp, Zap } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface AdminDashboardInsightsProps {
 deposits: number;
 readings: number;
 locale: string;
}

export function AdminDashboardInsights({
 deposits,
 readings,
 locale,
}: AdminDashboardInsightsProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('tn-grid-cols-1-2-md gap-8')}>
   <GlassCard className={cn('!p-10 tn-rounded-3xl relative overflow-hidden group')}>
    <div className={cn('absolute top-0 right-0 p-8 opacity-5 tn-group-raise transition-transform duration-700 blur-sm')}>
     <Gem size={160} className={cn('tn-text-accent')} />
    </div>
    <div className={cn('relative z-10 space-y-6')}>
     <div className={cn('flex items-center gap-4')}>
      <div className={cn('w-12 h-12 rounded-2xl tn-bg-accent-10 border tn-border-accent-20 flex items-center justify-center shadow-inner')}>
       <TrendingUp className={cn('w-6 h-6 tn-text-accent')} />
      </div>
      <h3 className={cn('text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>
       {t('dashboard.insights.revenue_title')}
      </h3>
     </div>
     <div className={cn('tn-text-hero-md font-black tn-text-primary italic tracking-tighter tn-drop-shadow-accent')}>
      {(deposits * 50000).toLocaleString(locale)}{' '}
      <span className={cn('text-2xl tn-text-accent')}>{t('dashboard.insights.revenue_currency')}</span>
     </div>
     <p className={cn('tn-text-overline tn-text-secondary')}>
      {t('dashboard.insights.revenue_note', { count: deposits })}
     </p>
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-10 tn-rounded-3xl relative overflow-hidden group')}>
    <div className={cn('absolute top-0 right-0 p-8 opacity-5 tn-group-raise transition-transform duration-700 blur-sm')}>
     <Activity size={160} className={cn('tn-text-success')} />
    </div>
    <div className={cn('relative z-10 space-y-6')}>
     <div className={cn('flex items-center gap-4')}>
      <div className={cn('w-12 h-12 rounded-2xl tn-bg-success-10 border tn-border-success-20 flex items-center justify-center shadow-inner')}>
       <Zap className={cn('w-6 h-6 tn-text-success')} />
      </div>
      <h3 className={cn('text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>
       {t('dashboard.insights.activity_title')}
      </h3>
     </div>
     <div className={cn('tn-text-hero-md font-black tn-text-primary italic tracking-tighter tn-drop-shadow-success')}>
      {readings.toLocaleString(locale)}{' '}
      <span className={cn('text-2xl tn-text-success')}>{t('dashboard.insights.activity_unit')}</span>
     </div>
     <p className={cn('tn-text-overline tn-text-secondary')}>
      {t('dashboard.insights.activity_note')}
     </p>
    </div>
   </GlassCard>
  </div>
 );
}
