'use client';

import { CheckCircle2 } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminWithdrawalsEmptyStateProps {
 label: string;
}

export function AdminWithdrawalsEmptyState({
 label,
}: AdminWithdrawalsEmptyStateProps) {
 return (
  <GlassCard className={cn('flex flex-col items-center justify-center py-20 text-center')}>
   <div className={cn('w-16 h-16 rounded-full tn-bg-success-10 border tn-border-success-20 flex items-center justify-center mb-6 shadow-inner')}>
    <CheckCircle2 className={cn('w-8 h-8 tn-text-success')} />
   </div>
   <p className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-tertiary')}>{label}</p>
  </GlassCard>
 );
}
