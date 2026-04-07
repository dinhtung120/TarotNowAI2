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
  return <span className={cn('tn-text-10 font-bold tn-text-tertiary italic')}>{labels.notAvailable}</span>;
 }

 return (
  <div className={cn('flex items-center justify-center gap-2 opacity-0 group-row-hover-opacity-100')}>
   <button
    type="button"
    onClick={() => onApprove(order)}
    disabled={isProcessing}
    className={cn('p-2.5 min-h-11 min-w-11 rounded-xl border tn-btn-success-soft transition-all tn-disabled-opacity-50 shadow-md group')}
    title={labels.approveTitle}
   >
    {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <ThumbsUp className={cn('w-4 h-4 group-hover-scale-110 transition-transform')} />}
   </button>
   <button
    type="button"
    onClick={() => onReject(order)}
    disabled={isProcessing}
    className={cn('p-2.5 min-h-11 min-w-11 rounded-xl border tn-btn-danger-soft transition-all tn-disabled-opacity-50 shadow-md group')}
    title={labels.rejectTitle}
   >
    {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <ThumbsDown className={cn('w-4 h-4 group-hover-scale-110 transition-transform')} />}
   </button>
  </div>
 );
}
