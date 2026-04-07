import { ArrowRight, Diamond } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WithdrawHistoryAmountRowProps {
 locale: string;
 amountDiamond: number;
 netAmountVnd: number;
}

export function WithdrawHistoryAmountRow({
 locale,
 amountDiamond,
 netAmountVnd,
}: WithdrawHistoryAmountRowProps) {
 return (
  <div className={cn('flex flex-wrap items-center gap-2 sm:gap-3')}>
   <div className={cn('flex items-center gap-1.5 bg-[var(--warning)]/10 px-2 py-1 rounded text-[var(--warning)] font-black text-sm italic')}>
    {amountDiamond} <Diamond className={cn('w-3.5 h-3.5')} />
   </div>
   <ArrowRight className={cn('w-3.5 h-3.5 tn-text-muted')} />
   <div className={cn('font-black text-[var(--success)] text-lg italic tracking-tighter')}>
    {netAmountVnd.toLocaleString(locale)}{' '}
    <span className={cn('text-[10px] not-italic text-[var(--success)]/60')}>₫</span>
   </div>
  </div>
 );
}
