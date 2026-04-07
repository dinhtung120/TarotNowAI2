'use client';

import { Diamond } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminWithdrawalAmountInfoProps {
 amountDiamond: number;
 feeLabel: string;
 feeVnd: string;
 grossLabel: string;
 grossVnd: string;
 netLabel: string;
 netVnd: string;
}

export function AdminWithdrawalAmountInfo({
 amountDiamond,
 feeLabel,
 feeVnd,
 grossLabel,
 grossVnd,
 netLabel,
 netVnd,
}: AdminWithdrawalAmountInfoProps) {
 return (
  <div className={cn('flex items-center gap-4')}>
   <div className={cn('w-12 h-12 rounded-[1rem] bg-[var(--warning)]/10 border border-[var(--warning)]/20 flex items-center justify-center shadow-inner group-hover:scale-110 transition-transform')}>
    <Diamond className={cn('w-6 h-6 text-[var(--warning)]')} />
   </div>
   <div>
    <div className={cn('text-2xl font-black tn-text-primary italic tracking-tighter drop-shadow-sm')}>{amountDiamond} <span className={cn('text-xl text-[var(--warning)]')}>💎</span></div>
    <div className={cn('text-[10px] font-bold text-[var(--text-secondary)] mt-1 flex gap-2 items-center')}>
     <span>{grossLabel}: {grossVnd}</span>
     <span className={cn('opacity-50')}>|</span> <span>{feeLabel}: {feeVnd}</span>
     <span className={cn('opacity-50')}>|</span> <span className={cn('text-[var(--success)] drop-shadow-sm')}>{netLabel}: {netVnd}</span>
    </div>
   </div>
  </div>
 );
}
