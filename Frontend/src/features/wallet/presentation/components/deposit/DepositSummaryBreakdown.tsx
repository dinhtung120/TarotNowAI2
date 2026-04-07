import { cn } from '@/lib/utils';

interface DepositSummaryBreakdownProps {
 locale: string;
 amountVnd: number;
 baseDiamond: number;
 bonusGold: number;
 formatVnd: (value: number) => string;
 labels: {
  valueLabel: string;
  diamondReceiveLabel: string;
  promoBonusLabel: string;
  totalAssetsLabel: string;
 };
}

export function DepositSummaryBreakdown({
 locale,
 amountVnd,
 baseDiamond,
 bonusGold,
 formatVnd,
 labels,
}: DepositSummaryBreakdownProps) {
 return (
  <div className={cn('space-y-3 tn-text-11')}>
   <div className={cn('flex items-center justify-between gap-4')}>
    <span className={cn('tn-text-muted uppercase tracking-widest tn-text-9 font-black')}>{labels.valueLabel}</span>
    <span className={cn('tn-text-primary font-black')}>{formatVnd(amountVnd)}</span>
   </div>
   <div className={cn('flex items-center justify-between gap-4')}>
    <span className={cn('tn-text-muted uppercase tracking-widest tn-text-9 font-black')}>{labels.diamondReceiveLabel}</span>
    <span className={cn('tn-text-primary font-black')}>{baseDiamond.toLocaleString(locale)} 💎</span>
   </div>
   <div className={cn('flex items-center justify-between gap-4')}>
    <span className={cn('tn-text-muted uppercase tracking-widest tn-text-9 font-black')}>{labels.promoBonusLabel}</span>
    <span className={cn('tn-text-warning font-black')}>+{bonusGold.toLocaleString(locale)} 🪙</span>
   </div>
   <div className={cn('pt-3 border-t tn-border-soft flex items-center justify-between gap-4')}>
    <span className={cn('tn-text-muted uppercase tracking-widest tn-text-9 font-black')}>{labels.totalAssetsLabel}</span>
    <span className={cn('tn-text-primary font-black')}>
     {baseDiamond.toLocaleString(locale)} 💎 {bonusGold > 0 ? `+ ${bonusGold.toLocaleString(locale)} 🪙` : ''}
    </span>
   </div>
  </div>
 );
}
