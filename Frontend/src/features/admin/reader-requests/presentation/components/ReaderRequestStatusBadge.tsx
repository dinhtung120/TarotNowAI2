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
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--warning)]/10 text-[var(--warning)] text-[9px] font-black uppercase tracking-widest border border-[var(--warning)]/20 shadow-inner')}>
    <Clock className={cn('w-3 h-3')} />
    {labels.pending}
   </span>
  );
 }

 if (status === 'approved') {
  return (
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--success)]/10 text-[var(--success)] text-[9px] font-black uppercase tracking-widest border border-[var(--success)]/20 shadow-inner')}>
    <CheckCircle2 className={cn('w-3 h-3')} />
    {labels.approved}
   </span>
  );
 }

 if (status === 'rejected') {
  return (
   <span className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--danger)]/10 text-[var(--danger)] text-[9px] font-black uppercase tracking-widest border border-[var(--danger)]/20 shadow-inner')}>
    <XCircle className={cn('w-3 h-3')} />
    {labels.rejected}
   </span>
  );
 }

 return null;
}
