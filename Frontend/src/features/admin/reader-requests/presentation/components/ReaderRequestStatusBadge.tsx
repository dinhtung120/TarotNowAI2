import { CheckCircle2, Clock, XCircle } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderRequestStatusBadgeProps {
 status: string;
 labels: {
  pending: string;
  approved: string;
  rejected: string;
 };
}

export function ReaderRequestStatusBadge({
 status,
 labels,
}: ReaderRequestStatusBadgeProps) {
 if (status === 'pending') {
  return (
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full tn-badge-warning tn-text-9 font-black uppercase tracking-widest border shadow-inner')}>
    <Clock className={cn('w-3 h-3')} />
    {labels.pending}
   </span>
  );
 }

 if (status === 'approved') {
  return (
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full tn-badge-success tn-text-9 font-black uppercase tracking-widest border shadow-inner')}>
    <CheckCircle2 className={cn('w-3 h-3')} />
    {labels.approved}
   </span>
  );
 }

 if (status === 'rejected') {
  return (
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full tn-badge-danger tn-text-9 font-black uppercase tracking-widest border shadow-inner')}>
    <XCircle className={cn('w-3 h-3')} />
    {labels.rejected}
   </span>
  );
 }

 return null;
}
