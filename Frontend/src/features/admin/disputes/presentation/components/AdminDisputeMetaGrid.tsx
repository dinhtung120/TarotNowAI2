'use client';

import type { AdminDisputeItemDto } from '@/features/chat/application/actions';
import { cn } from '@/lib/utils';

interface AdminDisputeMetaGridProps {
 amountLabel: string;
 item: AdminDisputeItemDto;
 itemIdLabel: string;
 payerLabel: string;
 readerLabel: string;
}

export function AdminDisputeMetaGrid({
 amountLabel,
 item,
 itemIdLabel,
 payerLabel,
 readerLabel,
}: AdminDisputeMetaGridProps) {
 return (
  <div className={cn('tn-grid-cols-1-4-md gap-3 text-xs')}>
   <div>
    <div className={cn('tn-text-tertiary uppercase tracking-widest tn-text-10')}>{itemIdLabel}</div>
    <div className={cn('font-mono tn-text-secondary break-all')}>{item.id}</div>
   </div>
   <div>
    <div className={cn('tn-text-tertiary uppercase tracking-widest tn-text-10')}>{amountLabel}</div>
    <div className={cn('font-bold tn-text-warning')}>{item.amountDiamond} 💎</div>
   </div>
   <div>
    <div className={cn('tn-text-tertiary uppercase tracking-widest tn-text-10')}>{payerLabel}</div>
    <div className={cn('font-mono tn-text-secondary break-all')}>{item.payerId}</div>
   </div>
   <div>
    <div className={cn('tn-text-tertiary uppercase tracking-widest tn-text-10')}>{readerLabel}</div>
    <div className={cn('font-mono tn-text-secondary break-all')}>{item.receiverId}</div>
   </div>
  </div>
 );
}
