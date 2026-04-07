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
  <div className={cn('grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3')}>
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
        ? 'tn-panel-strong border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
        : 'tn-panel border-[var(--border-subtle)] hover:border-[var(--border-hover)] hover:bg-[var(--bg-surface-hover)]',
      )}
     >
      <div className={cn('text-xs font-black tn-text-primary')}>{formatVnd(value)}</div>
      <div className={cn('text-[10px] font-bold tn-text-muted mt-1')}>
       {Math.floor(value / exchangeRate).toLocaleString(locale)} 💎
      </div>
      {promo && promo.bonusDiamond > 0 ? (
       <div className={cn('absolute top-3 right-3 inline-flex items-center gap-1 px-2 py-0.5 rounded-full bg-[var(--warning)]/15 border border-[var(--warning)]/20 text-[8px] font-black uppercase tracking-widest text-[var(--warning)]')}>
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
