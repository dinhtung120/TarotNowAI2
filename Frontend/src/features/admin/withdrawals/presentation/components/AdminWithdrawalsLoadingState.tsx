'use client';

import { Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminWithdrawalsLoadingStateProps {
 label: string;
}

export function AdminWithdrawalsLoadingState({
 label,
}: AdminWithdrawalsLoadingStateProps) {
 return (
  <div className={cn('flex items-center justify-center py-20')}>
   <div className={cn('flex flex-col items-center justify-center space-y-4')}>
    <Loader2 className={cn('w-8 h-8 animate-spin tn-text-success')} />
    <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>{label}</span>
   </div>
  </div>
 );
}
