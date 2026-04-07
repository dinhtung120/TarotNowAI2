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
   <div className={cn('w-16 h-16 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center mb-6 shadow-inner')}>
    <CheckCircle2 className={cn('w-8 h-8 text-[var(--success)]')} />
   </div>
   <p className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]')}>{label}</p>
  </GlassCard>
 );
}
