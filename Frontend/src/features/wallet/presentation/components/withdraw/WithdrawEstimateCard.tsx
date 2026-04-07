import { cn } from '@/lib/utils';

interface WithdrawEstimateCardProps {
 locale: string;
 grossVnd: number;
 feeVnd: number;
 netVnd: number;
 labels: {
  gross: string;
  fee: string;
  net: string;
 };
}

export function WithdrawEstimateCard({
 locale,
 grossVnd,
 feeVnd,
 netVnd,
 labels,
}: WithdrawEstimateCardProps) {
 return (
  <div className={cn('p-5 bg-[var(--success)]/5 border border-[var(--success)]/20 rounded-2xl space-y-3 animate-in fade-in duration-300')}>
   <div className={cn('flex justify-between items-center text-sm')}>
    <span className={cn('text-[var(--success)]/80 text-[10px] font-black uppercase tracking-widest')}>{labels.gross}</span>
    <span className={cn('tn-text-primary font-bold')}>{grossVnd.toLocaleString(locale)} ₫</span>
   </div>
   <div className={cn('flex justify-between items-center text-sm')}>
    <span className={cn('text-[var(--danger)]/80 text-[10px] font-black uppercase tracking-widest')}>{labels.fee}</span>
    <span className={cn('text-[var(--danger)] font-bold')}>-{feeVnd.toLocaleString(locale)} ₫</span>
   </div>
   <div className={cn('border-t border-[var(--success)]/15 pt-3 flex justify-between items-end')}>
    <span className={cn('text-[var(--success)] text-[11px] font-black uppercase tracking-widest')}>{labels.net}</span>
    <span className={cn('text-[var(--success)] font-black text-2xl italic')}>{netVnd.toLocaleString(locale)} ₫</span>
   </div>
  </div>
 );
}
