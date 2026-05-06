'use client';

import { Loader2 } from 'lucide-react';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

export function AdminDisputesLoadingState() {
 return (
  <GlassCard className={cn('h-52 flex items-center justify-center')}>
   <Loader2 className={cn('w-6 h-6 animate-spin tn-text-secondary')} />
  </GlassCard>
 );
}
