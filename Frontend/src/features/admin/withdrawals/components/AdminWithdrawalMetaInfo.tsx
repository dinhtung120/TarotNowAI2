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
  <div className={cn('flex flex-col tn-withdraw-meta-layout gap-2 tn-surface p-3 rounded-2xl border tn-border-soft shadow-inner shrink-0')}>
   <div className={cn('flex items-center gap-2 tn-text-10 font-black tn-text-secondary uppercase tracking-tighter')}>
    <Clock className={cn('w-3.5 h-3.5')} />
    {new Date(createdAt).toLocaleString(locale)}
   </div>
   <div className={cn('flex items-center gap-2 tn-text-10 font-bold tn-text-tertiary italic')}>
    <User className={cn('w-3.5 h-3.5')} />
    {idLabel}
   </div>
  </div>
 );
}
