'use client';

import { ArrowUpRight, History, Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';
import type { PaginatedResponse } from '@/features/admin/readings/hooks/useAdminReadings';

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
  <div className={cn('tn-grid-cols-1-3-md gap-6')}>
   <GlassCard className={cn('!p-8 group flex flex-col justify-between tn-minh-160 text-left tn-hover-border-accent-30 transition-all')}>
    <div className={cn('tn-text-overline tn-text-secondary')}>
     {t('readings.insights.total_title')}
    </div>
    <div className={cn('flex items-end justify-between')}>
     <div className={cn('text-4xl font-black tn-text-primary italic tracking-tighter drop-shadow-md')}>
      {data.totalCount}
     </div>
     <History className={cn('w-12 h-12 tn-text-accent -mb-2 -mr-2 tn-group-raise-rotate transition-transform duration-500')} />
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-8 flex items-center gap-6 group tn-hover-border-success-30 transition-all text-left')}>
    <div className={cn('w-14 h-14 rounded-2xl tn-overlay flex items-center justify-center border tn-border tn-group-raise transition-transform shadow-inner')}>
     <Sparkles className={cn('w-6 h-6 tn-text-success')} />
    </div>
    <div>
     <div className={cn('tn-text-overline tn-text-secondary')}>
      {t('readings.insights.update_label')}
     </div>
     <div className={cn('text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1')}>
      {t('readings.insights.update_value')}
     </div>
    </div>
   </GlassCard>

   <GlassCard className={cn('!p-8 flex items-center justify-between group overflow-hidden relative text-left tn-hover-border-brand-30 transition-all')}>
    <div className={cn('relative z-10 text-left')}>
     <div className={cn('tn-text-overline tn-text-secondary')}>
      {t('readings.insights.monitor_label')}
     </div>
     <div className={cn('text-sm font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md mt-1')}>
      {t('readings.insights.monitor_value')}
     </div>
    </div>
    <ArrowUpRight className={cn('w-12 h-12 tn-text-secondary absolute right-8 tn-hover-text-brand transition-colors duration-500')} />
    <div className={cn('absolute inset-0 tn-grad-brand-soft tn-group-slide-up')} />
   </GlassCard>
  </div>
 );
}
