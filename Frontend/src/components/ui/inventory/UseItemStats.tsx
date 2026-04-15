'use client';

import { cn } from '@/lib/utils';

interface UseItemStatsProps {
 quantityLabel: string;
 effectValueLabel: string;
 quantity: number;
 effectValue: number;
}

export default function UseItemStats({
 quantityLabel,
 effectValueLabel,
 quantity,
 effectValue,
}: UseItemStatsProps) {
 return (
  <div className={cn('grid grid-cols-2 gap-3 text-sm')}>
   <div className={cn('rounded-xl border border-slate-200 bg-slate-50/70 p-3 dark:border-slate-700 dark:bg-slate-900/40')}>
    <p className={cn('text-slate-500 dark:text-slate-400')}>{quantityLabel}</p>
    <p className={cn('font-semibold text-slate-900 dark:text-slate-100')}>x{quantity}</p>
   </div>
   <div className={cn('rounded-xl border border-slate-200 bg-slate-50/70 p-3 dark:border-slate-700 dark:bg-slate-900/40')}>
    <p className={cn('text-slate-500 dark:text-slate-400')}>{effectValueLabel}</p>
    <p className={cn('font-semibold text-slate-900 dark:text-slate-100')}>{effectValue}</p>
   </div>
  </div>
 );
}
