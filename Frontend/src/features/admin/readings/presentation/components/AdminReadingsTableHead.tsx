'use client';

import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export function AdminReadingsTableHead() {
 const t = useTranslations('Admin');

 return (
  <thead>
   <tr className={cn('border-b tn-border-soft tn-surface')}>
    <th className={cn('px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left')}>
     {t('readings.table.heading_timeline')}
    </th>
    <th className={cn('px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left')}>
     {t('readings.table.heading_user')}
    </th>
    <th className={cn('px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left')}>
     {t('readings.table.heading_spread')}
    </th>
    <th className={cn('px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left')}>
     {t('readings.table.heading_question')}
    </th>
    <th className={cn('px-8 py-6 text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-left')}>
     {t('readings.table.heading_status')}
    </th>
   </tr>
  </thead>
 );
}
