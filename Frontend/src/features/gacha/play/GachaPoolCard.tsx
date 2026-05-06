import { useState } from 'react';
import { Info } from 'lucide-react';
import { cn } from '@/lib/utils';
import Button from '@/shared/ui/Button';
import GlassCard from '@/shared/ui/GlassCard';
import Badge from '@/shared/ui/Badge';
import type { GachaPool } from '@/features/gacha/shared/gachaTypes';
import { localizeGachaDescription, localizeGachaName } from '@/features/gacha/shared/gachaLocalization';
import type { GachaPoolSelectorLabels } from '@/features/gacha/play/GachaPoolSelector';

interface GachaPoolCardProps {
 pool: GachaPool;
 locale: string;
 isSelected: boolean;
 isPulling: boolean;
 labels: GachaPoolSelectorLabels;
 onSelect: (poolCode: string) => void;
 onPull: (count: number) => void;
}

const PITY_HINT = 'Khi đầy tiến trình chắc chắn rớt item Legendary/Mythic. Nhận vật phẩm Legendary/Mythic sẽ đặt lại tiến trình này.';

export default function GachaPoolCard({ pool, locale, isSelected, isPulling, labels, onSelect, onPull }: GachaPoolCardProps) {
 const [showPityNote, setShowPityNote] = useState(false);
 const cost10x = Math.floor(pool.costAmount * 10 * 0.9);
 const pityProgress = pool.hardPityCount > 0
  ? Math.max(0, Math.min(100, Math.round((pool.userCurrentPity / pool.hardPityCount) * 100)))
  : 0;

 return (
  <GlassCard
   variant={isSelected ? 'elevated' : 'interactive'}
   padding="none"
   onClick={() => onSelect(pool.code)}
   className={cn('relative flex flex-col transition-all duration-500', isSelected ? 'ring-2 ring-emerald-500/40 shadow-[0_0_30px_rgba(16,185,129,0.1)]' : 'tn-panel-soft')}
  >
   <div className={cn('flex flex-col px-6 pt-6')}>
    <div className={cn('mb-2 flex items-center justify-between gap-2')}>
     <h3 className={cn('text-lg font-black tracking-tight tn-text-primary')}>{localizeGachaName(locale, pool)}</h3>
     <Badge variant={isSelected ? 'success' : 'default'} size="sm">{`${pool.costAmount} ${pool.costCurrency.toUpperCase()}`}</Badge>
    </div>
    <p className={cn('mb-4 line-clamp-2 text-xs font-medium leading-relaxed tn-text-secondary')}>{localizeGachaDescription(locale, pool)}</p>
   </div>

   <div className={cn('mt-auto border-t bg-white/[0.03] p-6 pt-4 tn-border-soft')}>
    <div className={cn('mb-3 flex items-center justify-between')}>
     <div className={cn('flex items-center gap-1.5')}>
      <span className={cn('text-[10px] font-black uppercase tracking-widest tn-text-muted')}>{labels.pity}</span>
      <button
       type="button"
       onClick={(event) => {
        event.stopPropagation();
        setShowPityNote((current) => !current);
       }}
       className={cn('flex h-4 w-4 items-center justify-center rounded-full transition-colors', showPityNote ? 'bg-emerald-500/20 text-emerald-400' : 'hover:tn-text-primary tn-text-muted')}
      >
       <Info size={12} strokeWidth={3} />
      </button>
     </div>
     <span className={cn('text-sm font-bold', isSelected ? 'tn-text-success' : 'tn-text-primary')}>
      {pool.userCurrentPity} / {pool.hardPityCount}
     </span>
    </div>

    {showPityNote ? (
     <div className={cn('mb-4 animate-in rounded-xl border border-emerald-500/20 bg-emerald-500/10 p-3 duration-300 fade-in slide-in-from-top-1')}>
      <p className={cn('text-[10px] font-bold leading-relaxed tn-text-success')}>{PITY_HINT}</p>
     </div>
    ) : null}

    <progress
     className={cn(
      'mb-4 tn-progress tn-progress-sm',
      isSelected ? 'tn-progress-success' : 'tn-progress-slate',
     )}
     max={100}
     value={pityProgress}
    />

    {isSelected ? (
     <div className={cn('grid grid-cols-2 gap-3 animate-in duration-300 fade-in zoom-in')}>
      <Button variant="secondary" size="md" className={cn('flex min-h-[54px] flex-col gap-0')} onClick={(event) => { event.stopPropagation(); onPull(1); }} isLoading={isPulling}>
       <span className={cn('text-[11px]')}>{labels.pull}</span>
       <span className={cn('mt-0.5 text-[9px] font-bold uppercase tracking-widest opacity-60')}>{pool.costAmount} {pool.costCurrency}</span>
      </Button>
      <Button variant="brand" size="md" className={cn('relative flex min-h-[54px] flex-col gap-0 overflow-hidden')} onClick={(event) => { event.stopPropagation(); onPull(10); }} isLoading={isPulling}>
       <span className={cn('text-[11px]')}>{labels.pull10x}</span>
       <span className={cn('mt-0.5 text-[9px] font-bold uppercase tracking-widest opacity-90')}>{cost10x} {pool.costCurrency}</span>
       <div className={cn('absolute -right-5 -top-1 rotate-45 bg-amber-400 px-5 py-0.5 shadow-lg')}>
        <span className={cn('text-[8px] font-black text-black')}>-10%</span>
       </div>
      </Button>
     </div>
    ) : null}
   </div>
  </GlassCard>
 );
}
