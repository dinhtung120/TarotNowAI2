import { Loader2, ThumbsDown, ThumbsUp } from 'lucide-react';
import type { AdminDepositOrder } from '@/features/admin/application/actions';
import { cn } from '@/lib/utils';

interface AdminDepositRowActionsProps {
 order: AdminDepositOrder;
 processingId: string | null;
 labels: {
  approveTitle: string;
  rejectTitle: string;
  notAvailable: string;
 };
 onApprove: (order: AdminDepositOrder) => void;
 onReject: (order: AdminDepositOrder) => void;
}

export function AdminDepositRowActions({
 order,
 processingId,
 labels,
 onApprove,
 onReject,
}: AdminDepositRowActionsProps) {
 const isProcessing = processingId === order.id;

 if (order.status !== 'Pending') {
  return <span className={cn('text-[10px] font-bold text-[var(--text-tertiary)] italic')}>{labels.notAvailable}</span>;
 }

 return (
  <div className={cn('flex items-center justify-center gap-2 opacity-0 group-hover/row:opacity-100 transition-opacity')}>
   <button
    type="button"
    onClick={() => onApprove(order)}
    disabled={isProcessing}
    className={cn('p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--success)]/10 text-[var(--success)] border border-[var(--success)]/20 hover:bg-[var(--success)] hover:tn-text-ink transition-all disabled:opacity-50 shadow-md group')}
    title={labels.approveTitle}
   >
    {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <ThumbsUp className={cn('w-4 h-4 group-hover:scale-110 transition-transform')} />}
   </button>
   <button
    type="button"
    onClick={() => onReject(order)}
    disabled={isProcessing}
    className={cn('p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--danger)]/10 text-[var(--danger)] border border-[var(--danger)]/20 hover:bg-[var(--danger)] hover:tn-text-primary transition-all disabled:opacity-50 shadow-md group')}
    title={labels.rejectTitle}
   >
    {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <ThumbsDown className={cn('w-4 h-4 group-hover:scale-110 transition-transform')} />}
   </button>
  </div>
 );
}
