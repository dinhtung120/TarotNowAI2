import { Building2, Clock } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WithdrawHistoryMetaRowProps {
 locale: string;
 bankName: string;
 bankAccountNumber: string;
 createdAt: string;
}

export function WithdrawHistoryMetaRow({
 locale,
 bankName,
 bankAccountNumber,
 createdAt,
}: WithdrawHistoryMetaRowProps) {
 return (
  <div className={cn('flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-[10px] text-[var(--text-tertiary)] font-medium')}>
   <div className={cn('flex items-center gap-1.5 uppercase tracking-widest break-all')}>
    <Building2 className={cn('w-3 h-3 tn-text-muted')} />
    {bankName} • {bankAccountNumber}
   </div>
   <div className={cn('flex items-center gap-1.5 font-bold tn-text-muted')}>
    <Clock className={cn('w-3 h-3')} />
    {new Date(createdAt).toLocaleDateString(locale)}
   </div>
  </div>
 );
}
