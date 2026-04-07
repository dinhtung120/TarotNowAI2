'use client';

import type { ReactNode } from 'react';
import { cn } from '@/lib/utils';
import type { GachaRarityStyle } from './rarityConfig';

interface GachaSingleResultLayoutProps {
 borderClass: string;
 glowClass: string;
 name: string;
 rarity: string;
 rarityStyle: GachaRarityStyle;
 renderRewardIcon: (sizeClass: string) => ReactNode;
}

export function GachaSingleResultLayout({
 borderClass,
 glowClass,
 name,
 rarity,
 rarityStyle,
 renderRewardIcon,
}: GachaSingleResultLayoutProps) {
 return (
  <div className={cn('tn-gacha-shell p-1 items-center relative border-2 backdrop-blur-xl transition-all animate-in zoom-in-95 duration-700', glowClass, borderClass)}>
   <div className={cn('tn-gacha-left p-8 select-none pointer-events-none')}>
    {renderRewardIcon('w-full h-full tn-gacha-icon-scale')}
   </div>
   <div className={cn('tn-gacha-right px-8 relative')}>
    <div className={cn('tn-gacha-top-badge')}>
     <span className={cn('px-4 py-1 rounded-full border tn-text-overline tn-tracking-02 shadow-inner', rarityStyle.text, rarityStyle.border, 'bg-black/40')}>{rarity}</span>
    </div>
    <div className={cn('tn-gacha-bottom-name space-y-1')}>
     <p className={cn('tn-text-overline tn-tracking-03 opacity-40 text-stone-200')}>Reward Unlocked</p>
     <h3 className={cn('tn-text-3-4-md font-extrabold italic tracking-tighter uppercase drop-shadow-lg', rarityStyle.text)}>{name}</h3>
    </div>
   </div>
  </div>
 );
}
