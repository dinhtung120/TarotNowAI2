'use client';

import { ShieldAlert } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminDisputesEmptyStateProps {
 label: string;
}

export function AdminDisputesEmptyState({ label }: AdminDisputesEmptyStateProps) {
 return (
  <GlassCard className={cn('h-52 flex flex-col items-center justify-center gap-3')}>
   <ShieldAlert className={cn('w-8 h-8 tn-text-tertiary')} />
   <p className={cn('text-sm tn-text-secondary')}>{label}</p>
  </GlassCard>
 );
}
