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
  <div className={cn('w-full max-w-2xl min-h-[450px] md:min-h-0 md:h-72 rounded-[2.5rem] p-1 flex flex-col md:flex-row items-center relative border-2 backdrop-blur-xl transition-all animate-in zoom-in-95 duration-700', glowClass, borderClass)}>
   <div className={cn('w-full md:w-1/2 h-64 md:h-full flex items-center justify-center p-8 select-none pointer-events-none')}>
    {renderRewardIcon('w-full h-full transform scale-125 md:scale-150 transition-transform duration-1000 rotate-3 hover:rotate-0')}
   </div>
   <div className={cn('w-full md:w-1/2 h-40 md:h-full flex flex-col justify-center px-8 relative')}>
    <div className={cn('absolute top-4 md:top-8 right-8')}>
     <span className={cn('px-4 py-1 rounded-full border text-[10px] uppercase font-black tracking-[0.2em] shadow-inner', rarityStyle.text, rarityStyle.border, 'bg-black/40')}>{rarity}</span>
    </div>
    <div className={cn('absolute bottom-6 md:bottom-8 right-8 text-right space-y-1')}>
     <p className={cn('text-[10px] font-black uppercase tracking-[0.3em] opacity-40 text-stone-200')}>Reward Unlocked</p>
     <h3 className={cn('text-3xl md:text-4xl font-extrabold italic tracking-tighter uppercase drop-shadow-lg', rarityStyle.text)}>{name}</h3>
    </div>
   </div>
  </div>
 );
}
