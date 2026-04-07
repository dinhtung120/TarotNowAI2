import { Coins } from 'lucide-react';
import { cn } from '@/lib/utils';

interface DepositPromotionPreview {
 bonusDiamond: number;
}

interface DepositPresetGridProps {
 presetAmounts: readonly number[];
 isCustom: boolean;
 selectedAmount: number;
 exchangeRate: number;
 locale: string;
 formatVnd: (value: number) => string;
 bonusLabel: (amount: number) => string;
 onSelectPreset: (value: number) => void;
 getPromotion: (value: number) => DepositPromotionPreview | null;
}

export function DepositPresetGrid({
 presetAmounts,
 isCustom,
 selectedAmount,
 exchangeRate,
 locale,
 formatVnd,
 bonusLabel,
 onSelectPreset,
 getPromotion,
}: DepositPresetGridProps) {
 return (
  <div className={cn('tn-grid-1-2-3-responsive gap-3')}>
   {presetAmounts.map((value) => {
    const selected = !isCustom && selectedAmount === value;
    const promo = getPromotion(value);
    return (
     <button
      key={value}
      type="button"
      onClick={() => onSelectPreset(value)}
      className={cn(
       'relative p-4 rounded-2xl border transition-all text-left',
       selected
        ? 'tn-panel-strong tn-deposit-preset-selected'
        : 'tn-panel tn-deposit-preset',
      )}
     >
      <div className={cn('text-xs font-black tn-text-primary')}>{formatVnd(value)}</div>
      <div className={cn('tn-text-10 font-bold tn-text-muted mt-1')}>
       {Math.floor(value / exchangeRate).toLocaleString(locale)} 💎
      </div>
      {promo && promo.bonusDiamond > 0 ? (
       <div className={cn('absolute top-3 right-3 inline-flex items-center gap-1 px-2 py-0.5 rounded-full tn-deposit-promo-badge border tn-text-8 font-black uppercase tracking-widest')}>
        <Coins className={cn('w-3 h-3')} />
        {bonusLabel(promo.bonusDiamond)}
       </div>
      ) : null}
     </button>
    );
   })}
  </div>
 );
}
