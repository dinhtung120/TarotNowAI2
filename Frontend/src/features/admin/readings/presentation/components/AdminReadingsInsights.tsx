'use client';

import { ArrowUpRight, History, Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/application/useAdminReadings';

interface AdminReadingsInsightsProps {
 data: PaginatedResponse | null | undefined;
 loading: boolean;
}

export function AdminReadingsInsights({ data, loading }: AdminReadingsInsightsProps) {
 const t = useTranslations('Admin');

 if (loading || !data) {
  return null;
 }

 return (
  <div className={cn('grid grid-cols-1 md:grid-cols-3 gap-6')}>
   <GlassCard className={cn('!p-8 group flex flex-col justify-between min-h-[160px] text-left hover:border-[var(--purple-accent)]/30 transition-all')}>
    <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
     {t('readings.insights.total_title')}
    </div>
    <div className={cn('flex items-end justify-between')}>
     <div className={cn('text-4xl font-black tn-text-primary italic tracking-tighter drop-shadow-md')}>
      {data.totalCount}
     </div>
     <History className={cn('w-12 h-12 text-[var(--purple-accent)]/20 -mb-2 -mr-2 group-hover:scale-110 group-hover:-rotate-12 transition-transform duration-500')} />
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-8 flex items-center gap-6 group hover:border-[var(--success)]/30 transition-all text-left')}>
    <div className={cn('w-14 h-14 rounded-2xl tn-overlay flex items-center justify-center border tn-border group-hover:scale-110 transition-transform shadow-inner')}>
     <Sparkles className={cn('w-6 h-6 text-[var(--success)]')} />
    </div>
    <div>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
      {t('readings.insights.update_label')}
     </div>
     <div className={cn('text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1')}>
      {t('readings.insights.update_value')}
     </div>
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-8 flex items-center justify-between group overflow-hidden relative text-left hover:border-[var(--accent)]/30 transition-all')}>
    <div className={cn('relative z-10 text-left')}>
     <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]')}>
      {t('readings.insights.monitor_label')}
     </div>
     <div className={cn('text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1')}>
      {t('readings.insights.monitor_value')}
     </div>
    </div>
    <ArrowUpRight className={cn('w-12 h-12 text-[var(--text-secondary)] absolute right-8 group-hover:text-[var(--accent)] transition-colors duration-500')} />
    <div className={cn('absolute inset-0 bg-gradient-to-tr from-transparent to-[var(--accent)]/5 translate-y-full group-hover:translate-y-0 transition-transform duration-700')} />
   </GlassCard>
  </div>
 );
}
