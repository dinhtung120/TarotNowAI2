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
  <div className={cn('p-5 tn-bg-success-5 border tn-border-success-20 rounded-2xl space-y-3 animate-in fade-in duration-300')}>
   <div className={cn('flex justify-between items-center text-sm')}>
    <span className={cn('tn-text-success-80 tn-text-10 font-black uppercase tracking-widest')}>{labels.gross}</span>
    <span className={cn('tn-text-primary font-bold')}>{grossVnd.toLocaleString(locale)} ₫</span>
   </div>
   <div className={cn('flex justify-between items-center text-sm')}>
    <span className={cn('tn-text-danger-80 tn-text-10 font-black uppercase tracking-widest')}>{labels.fee}</span>
    <span className={cn('tn-text-danger font-bold')}>-{feeVnd.toLocaleString(locale)} ₫</span>
   </div>
   <div className={cn('border-t tn-border-success-15 pt-3 flex justify-between items-end')}>
    <span className={cn('tn-text-success tn-text-11 font-black uppercase tracking-widest')}>{labels.net}</span>
    <span className={cn('tn-text-success font-black text-2xl italic')}>{netVnd.toLocaleString(locale)} ₫</span>
   </div>
  </div>
 );
}
