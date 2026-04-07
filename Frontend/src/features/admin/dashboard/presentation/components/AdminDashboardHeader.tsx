'use client';

import { Activity, TrendingUp } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

export function AdminDashboardHeader() {
 const t = useTranslations('Admin');

 return (
  <div className={cn('flex flex-col tn-md-flex-row tn-md-items-end justify-between gap-6')}>
   <SectionHeader
    tag={t('dashboard.header.tag')}
    tagIcon={<TrendingUp className={cn('w-3 h-3 tn-text-success')} />}
    title={t('dashboard.header.title')}
    subtitle={t('dashboard.header.subtitle')}
    className={cn('mb-0 text-left items-start')}
   />
   <div className={cn('flex items-center gap-4 shrink-0')}>
    <div className={cn('px-6 py-3 rounded-2xl tn-panel')}>
     <div className={cn('tn-text-8 font-black uppercase tracking-widest tn-text-muted text-left')}>
      {t('dashboard.health.title')}
     </div>
     <div className={cn('flex items-center gap-2 text-xs font-bold tn-text-success')}>
      <Activity className={cn('w-3 h-3 animate-pulse')} />
      {t('dashboard.health.ok')}
     </div>
    </div>
   </div>
  </div>
 );
}
