'use client';

import { memo } from 'react';
import { cn } from '@/lib/utils';
import type { GachaPool } from '@/features/gacha/shared/gachaTypes';
import GachaPoolCard from '@/features/gacha/play/GachaPoolCard';

export interface GachaPoolSelectorLabels {
 pull: string;
 pull10x: string;
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
 onPull: (count: number) => void;
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
  <div className={cn('grid grid-cols-1 gap-6 lg:grid-cols-3')}>
   {pools.map((pool) => (
    <GachaPoolCard
     key={pool.code}
     pool={pool}
     locale={locale}
     isSelected={selectedPoolCode === pool.code}
     isPulling={isPulling}
     labels={labels}
     onSelect={onSelectPool}
     onPull={onPull}
    />
   ))}
  </div>
 );
}

const GachaPoolSelector = memo(GachaPoolSelectorComponent);
GachaPoolSelector.displayName = 'GachaPoolSelector';

export default GachaPoolSelector;
