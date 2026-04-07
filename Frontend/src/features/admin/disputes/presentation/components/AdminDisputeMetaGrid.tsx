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
  <div className={cn('grid grid-cols-1 md:grid-cols-4 gap-3 text-xs')}>
   <div>
    <div className={cn('text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]')}>{itemIdLabel}</div>
    <div className={cn('font-mono text-[var(--text-secondary)] break-all')}>{item.id}</div>
   </div>
   <div>
    <div className={cn('text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]')}>{amountLabel}</div>
    <div className={cn('font-bold text-[var(--warning)]')}>{item.amountDiamond} 💎</div>
   </div>
   <div>
    <div className={cn('text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]')}>{payerLabel}</div>
    <div className={cn('font-mono text-[var(--text-secondary)] break-all')}>{item.payerId}</div>
   </div>
   <div>
    <div className={cn('text-[var(--text-tertiary)] uppercase tracking-widest text-[10px]')}>{readerLabel}</div>
    <div className={cn('font-mono text-[var(--text-secondary)] break-all')}>{item.receiverId}</div>
   </div>
  </div>
 );
}
