'use client';

import { cn } from '@/lib/utils';

interface GachaPityProgressProps {
 currentPity: number;
 guaranteedLabel: string;
 hardPityCount: number;
 pityLabel: string;
 pityPercentage: number;
}

export function GachaPityProgress({
 currentPity,
 guaranteedLabel,
 hardPityCount,
 pityLabel,
 pityPercentage,
}: GachaPityProgressProps) {
 return (
  <div className={cn('space-y-3')}>
   <div className={cn('flex justify-between text-[10px] uppercase font-black text-stone-500 tracking-widest')}>
    <span>{pityLabel}</span>
    <span className={cn('text-stone-300')}>{currentPity} / {hardPityCount}</span>
   </div>
   <div className={cn('h-2 w-full bg-stone-950 rounded-full overflow-hidden border border-stone-800/50 p-[1px]')}>
    <div className={cn('h-full rounded-full transition-all duration-1000 ease-out shadow-[0_0_8px_rgba(168,85,247,0.4)]', pityPercentage > 90 ? 'bg-amber-400' : 'bg-gradient-to-r from-indigo-500 to-purple-500')} style={{ width: `${pityPercentage}%` }} />
   </div>
   <p className={cn('text-[9px] font-bold uppercase tracking-wide text-stone-600 text-right')}>{guaranteedLabel}</p>
  </div>
 );
}
