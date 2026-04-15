'use client';

import { memo } from 'react';
import { cn } from '@/lib/utils';
import Button from '@/shared/components/ui/Button';
import type { GachaPool } from '@/shared/infrastructure/gacha/gachaTypes';

interface GachaPoolSelectorLabels {
 pull: string;
 pulling: string;
 pity: string;
 pityRuleHint: string;
}

interface GachaPoolSelectorProps {
 pools: GachaPool[];
 locale: string;
 selectedPoolCode: string;
 isPulling: boolean;
 labels: GachaPoolSelectorLabels;
 onSelectPool: (poolCode: string) => void;
 onPull: () => void;
}

function localizePoolName(pool: GachaPool, locale: string): string {
 if (locale === 'en') return pool.nameEn;
 if (locale === 'zh') return pool.nameZh;
 return pool.nameVi;
}

function localizePoolDescription(pool: GachaPool, locale: string): string {
 if (locale === 'en') return pool.descriptionEn;
 if (locale === 'zh') return pool.descriptionZh;
 return pool.descriptionVi;
}

function GachaPoolSelectorComponent({
 pools,
 locale,
 selectedPoolCode,
 isPulling,
 labels,
 onSelectPool,
 onPull,
}: GachaPoolSelectorProps) {
 return (
  <div className={cn('grid grid-cols-1 gap-4 lg:grid-cols-3')}>
   {pools.map((pool) => {
    const isSelected = selectedPoolCode === pool.code;
    return (
     <div
      key={pool.code}
      className={cn(
       'rounded-2xl border p-4 text-left transition-all duration-200',
       isSelected
        ? 'border-emerald-500 bg-emerald-50 shadow-sm dark:bg-emerald-950/30'
        : 'border-slate-200 bg-white hover:border-emerald-300 dark:border-slate-700 dark:bg-slate-900',
      )}
     >
      <button
       type="button"
       onClick={() => onSelectPool(pool.code)}
       className={cn('w-full text-left')}
       aria-pressed={isSelected}
      >
       <p className={cn('text-sm font-semibold text-slate-900 dark:text-slate-100')}>{localizePoolName(pool, locale)}</p>
       <p className={cn('mt-1 text-xs text-slate-600 dark:text-slate-300')}>{localizePoolDescription(pool, locale)}</p>
       <div className={cn('mt-3 flex items-center justify-between text-xs text-slate-700 dark:text-slate-200')}>
        <span>{`${pool.costAmount} ${pool.costCurrency.toUpperCase()}`}</span>
       <span>{`${labels.pity}: ${pool.userCurrentPity}/${pool.hardPityCount}`}</span>
       </div>
       <p className={cn('mt-2 text-xs text-slate-500 dark:text-slate-300')}>{labels.pityRuleHint}</p>
      </button>
      {isSelected ? (
       <Button className={cn('mt-3 w-full')} onClick={onPull} isLoading={isPulling} aria-label={labels.pull}>
        {isPulling ? labels.pulling : labels.pull}
       </Button>
      ) : null}
     </div>
    );
   })}
  </div>
 );
}

const GachaPoolSelector = memo(GachaPoolSelectorComponent);
GachaPoolSelector.displayName = 'GachaPoolSelector';

export default GachaPoolSelector;
