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
  <div className={cn('flex flex-wrap items-center tn-gap-2-3-sm')}>
   <div className={cn('flex items-center gap-1.5 tn-bg-warning-10 px-2 py-1 rounded tn-text-warning font-black text-sm italic')}>
    {amountDiamond} <Diamond className={cn('w-3.5 h-3.5')} />
   </div>
   <ArrowRight className={cn('w-3.5 h-3.5 tn-text-muted')} />
   <div className={cn('font-black tn-text-success text-lg italic tracking-tighter')}>
    {netAmountVnd.toLocaleString(locale)}{' '}
    <span className={cn('tn-text-10 not-italic tn-text-success-60')}>₫</span>
   </div>
  </div>
 );
}
