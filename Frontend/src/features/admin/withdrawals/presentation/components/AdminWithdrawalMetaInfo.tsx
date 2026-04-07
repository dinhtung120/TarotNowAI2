'use client';

import { Clock, User } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminWithdrawalMetaInfoProps {
 createdAt: string;
 idLabel: string;
 locale: string;
}

export function AdminWithdrawalMetaInfo({
 createdAt,
 idLabel,
 locale,
}: AdminWithdrawalMetaInfoProps) {
 return (
  <div className={cn('flex flex-col items-start md:items-end gap-2 text-left md:text-right tn-surface p-3 rounded-2xl border tn-border-soft shadow-inner shrink-0')}>
   <div className={cn('flex items-center gap-2 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter')}>
    <Clock className={cn('w-3.5 h-3.5')} />
    {new Date(createdAt).toLocaleString(locale)}
   </div>
   <div className={cn('flex items-center gap-2 text-[10px] font-bold text-[var(--text-tertiary)] italic')}>
    <User className={cn('w-3.5 h-3.5')} />
    {idLabel}
   </div>
  </div>
 );
}
