'use client';

import { Activity, Gem, TrendingUp, Zap } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
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
  <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-8')}>
   <GlassCard className={cn('!p-10 !rounded-[3rem] relative overflow-hidden group')}>
    <div className={cn('absolute top-0 right-0 p-8 opacity-[0.03] group-hover:scale-110 transition-transform duration-700 blur-sm group-hover:blur-none')}>
     <Gem size={160} className={cn('text-[var(--purple-accent)]')} />
    </div>
    <div className={cn('relative z-10 space-y-6')}>
     <div className={cn('flex items-center gap-4')}>
      <div className={cn('w-12 h-12 rounded-2xl bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 flex items-center justify-center shadow-inner')}>
       <TrendingUp className={cn('w-6 h-6 text-[var(--purple-accent)]')} />
      </div>
      <h3 className={cn('text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>
       {t('dashboard.insights.revenue_title')}
      </h3>
     </div>
     <div className={cn('text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter drop-shadow-[0_0_15px_var(--c-168-85-247-40)]')}>
      {(deposits * 50000).toLocaleString(locale)}{' '}
      <span className={cn('text-2xl text-[var(--purple-accent)]')}>{t('dashboard.insights.revenue_currency')}</span>
     </div>
     <p className={cn('text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]')}>
      {t('dashboard.insights.revenue_note', { count: deposits })}
     </p>
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-10 !rounded-[3rem] relative overflow-hidden group')}>
    <div className={cn('absolute top-0 right-0 p-8 opacity-[0.03] group-hover:scale-110 transition-transform duration-700 blur-sm group-hover:blur-none')}>
     <Activity size={160} className={cn('text-[var(--success)]')} />
    </div>
    <div className={cn('relative z-10 space-y-6')}>
     <div className={cn('flex items-center gap-4')}>
      <div className={cn('w-12 h-12 rounded-2xl bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center shadow-inner')}>
       <Zap className={cn('w-6 h-6 text-[var(--success)]')} />
      </div>
      <h3 className={cn('text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>
       {t('dashboard.insights.activity_title')}
      </h3>
     </div>
     <div className={cn('text-5xl md:text-6xl font-black tn-text-primary italic tracking-tighter drop-shadow-[0_0_15px_var(--c-16-185-129-40)]')}>
      {readings.toLocaleString(locale)}{' '}
      <span className={cn('text-2xl text-[var(--success)]')}>{t('dashboard.insights.activity_unit')}</span>
     </div>
     <p className={cn('text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]')}>
      {t('dashboard.insights.activity_note')}
     </p>
    </div>
   </GlassCard>
  </div>
 );
}
