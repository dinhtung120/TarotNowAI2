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

export function AdminDepositRowActions({ labels }: AdminDepositRowActionsProps) {
 return (
  <span className={cn('tn-text-10 font-bold uppercase tracking-widest tn-text-tertiary')}>
   {labels.notAvailable}
  </span>
 );
}
