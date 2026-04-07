'use client';

import { Building2 } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminWithdrawalBankInfoProps {
 bankAccountName: string;
 bankAccountNumber: string;
 bankName: string;
}

export function AdminWithdrawalBankInfo({
 bankAccountName,
 bankAccountNumber,
 bankName,
}: AdminWithdrawalBankInfoProps) {
 return (
  <div className={cn('flex items-center gap-4 p-4 tn-panel-soft rounded-2xl shadow-inner')}>
   <div className={cn('w-8 h-8 rounded-full tn-surface flex items-center justify-center shrink-0')}>
    <Building2 className={cn('w-4 h-4 text-[var(--text-secondary)]')} />
   </div>
   <div className={cn('text-xs text-[var(--text-tertiary)] flex flex-wrap items-center gap-2')}>
    <span className={cn('font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm')}>{bankName}</span>
    <span className={cn('text-[color:var(--c-61-49-80-20)]')}>&bull;</span>
    <span className={cn('font-bold')}>{bankAccountName}</span>
    <span className={cn('text-[color:var(--c-61-49-80-20)]')}>&bull;</span>
    <span className={cn('font-mono text-[var(--text-secondary)] tn-surface px-2 py-0.5 rounded-md border tn-border')}>{bankAccountNumber}</span>
   </div>
  </div>
 );
}
