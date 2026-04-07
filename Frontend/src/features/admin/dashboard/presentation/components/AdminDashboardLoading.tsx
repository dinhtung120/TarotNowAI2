'use client';

import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export function AdminDashboardLoading() {
 const t = useTranslations('Admin');

 return (
  <div className={cn('h-[60vh] flex flex-col items-center justify-center space-y-6')}>
   <div className={cn('relative group')}>
    <div className={cn('absolute inset-x-0 top-0 h-40 w-40 bg-[var(--danger)]/20 blur-[60px] rounded-full animate-pulse')} />
    <Loader2 className={cn('w-12 h-12 animate-spin text-[var(--danger)] relative z-10')} />
   </div>
   <div className={cn('text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]')}>
    {t('dashboard.loading')}
   </div>
  </div>
 );
}
