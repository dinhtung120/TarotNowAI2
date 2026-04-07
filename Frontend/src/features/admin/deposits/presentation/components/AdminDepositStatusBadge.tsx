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
 const icon = status === 'Success'
  ? <CheckCircle2 className={cn('w-3 h-3 text-[var(--success)]')} />
  : status === 'Failed'
  ? <XCircle className={cn('w-3 h-3 text-[var(--danger)]')} />
  : <Clock className={cn('w-3 h-3 text-[var(--warning)]')} />;

 const className = status === 'Success'
  ? 'bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)] shadow-inner'
  : status === 'Failed'
  ? 'bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)] shadow-inner'
  : 'bg-[var(--warning)]/10 border-[var(--warning)]/20 text-[var(--warning)] shadow-inner';

 const label = status === 'Success'
  ? labels.success
  : status === 'Failed'
  ? labels.failed
  : labels.pending;

 return (
  <div className={cn('inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border', className)}>
   {icon}
   {label}
  </div>
 );
}
