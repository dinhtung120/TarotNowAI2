'use client';

import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export function AdminDashboardLoading() {
 const t = useTranslations('Admin');

 return (
  <div className={cn('tn-h-60vh flex flex-col items-center justify-center space-y-6')}>
   <div className={cn('relative group')}>
    <div className={cn('tn-dashboard-loading-glow absolute inset-x-0 top-0 h-40 w-40 rounded-full animate-pulse')} />
    <Loader2 className={cn('w-12 h-12 animate-spin tn-text-danger relative z-10')} />
   </div>
   <div className={cn('tn-text-10 font-black uppercase tn-tracking-03 tn-text-secondary')}>
    {t('dashboard.loading')}
   </div>
  </div>
 );
}
