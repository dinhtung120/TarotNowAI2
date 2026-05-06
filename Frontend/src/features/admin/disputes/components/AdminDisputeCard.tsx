'use client';

import type { AdminDisputeItemDto } from '@/features/chat/shared/actions';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';
import { AdminDisputeActions } from './AdminDisputeActions';
import { AdminDisputeMetaGrid } from './AdminDisputeMetaGrid';
import { AdminDisputeNoteInput } from './AdminDisputeNoteInput';
import { AdminDisputeSplitInput } from './AdminDisputeSplitInput';

interface AdminDisputeCardProps {
 amountLabel: string;
 isProcessing: boolean;
 item: AdminDisputeItemDto;
 itemIdLabel: string;
 note: string;
 notePlaceholder: string;
 onChangeNote: (value: string) => void;
 onChangeSplitPercent: (value: number) => void;
 onRefund: () => void;
 onRelease: () => void;
 onSplit: () => void;
 payerLabel: string;
 readerLabel: string;
 refundLabel: string;
 releaseLabel: string;
 splitLabel: string;
 splitPercent: number;
 splitPercentLabel: string;
}

export function AdminDisputeCard(props: AdminDisputeCardProps) {
 return (
  <GlassCard className={cn('space-y-4 !p-5')}>
   <AdminDisputeMetaGrid amountLabel={props.amountLabel} item={props.item} itemIdLabel={props.itemIdLabel} payerLabel={props.payerLabel} readerLabel={props.readerLabel} />
   <AdminDisputeNoteInput note={props.note} onChange={props.onChangeNote} placeholder={props.notePlaceholder} />
   <AdminDisputeSplitInput label={props.splitPercentLabel} onChange={props.onChangeSplitPercent} splitPercent={props.splitPercent} />
   <AdminDisputeActions
    isProcessing={props.isProcessing}
    onRelease={props.onRelease}
    onRefund={props.onRefund}
    onSplit={props.onSplit}
    releaseLabel={props.releaseLabel}
    refundLabel={props.refundLabel}
    splitLabel={props.splitLabel}
   />
  </GlassCard>
 );
}
