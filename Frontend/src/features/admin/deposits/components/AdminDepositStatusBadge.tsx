import { CheckCircle2, Clock, XCircle } from 'lucide-react';
import { cn } from '@/lib/utils';

interface AdminDepositStatusBadgeProps {
 status: string;
 labels: {
  success: string;
  failed: string;
  pending: string;
 };
}

export function AdminDepositStatusBadge({
 status,
 labels,
}: AdminDepositStatusBadgeProps) {
 const normalized = status.toLowerCase();
 const icon = normalized === 'success'
  ? <CheckCircle2 className={cn('w-3 h-3 tn-text-success')} />
  : normalized === 'failed'
  ? <XCircle className={cn('w-3 h-3 tn-text-danger')} />
  : <Clock className={cn('w-3 h-3 tn-text-warning')} />;

 const statusClass = normalized === 'success'
  ? 'tn-bg-success-10 tn-border-success-20 tn-text-success shadow-inner'
  : normalized === 'failed'
  ? 'tn-bg-danger-soft tn-border-danger tn-text-danger shadow-inner'
  : 'tn-bg-warning-10 tn-border-warning-20 tn-text-warning shadow-inner';

 const label = normalized === 'success'
  ? labels.success
  : normalized === 'failed'
  ? labels.failed
  : labels.pending;

 return (
  <div className={cn('inline-flex items-center gap-2 px-3 py-1.5 rounded-xl tn-text-9 font-black uppercase tracking-widest border', statusClass)}>
   {icon}
   {label}
  </div>
 );
}
